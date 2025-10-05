using FMOD.Studio;
using FMODUnity;
using Newtonsoft.Json;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;
using Random = UnityEngine.Random;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private EventReference eventShoot;
    private EventInstance instanceEventShoot;
    [SerializeField]
    private EventReference eventReload;
    private EventInstance instanceEventReload;
    private AudioManager audioManager = null;



    #region Stats

    private bool w_canReload = true;
    public int w_currentMagazine;
    public int w_maxAmmo;
    private string w_name;
    private float w_cooldown;
    private bool w_isCoolingDown = false;

    [SerializeField] private float w_shotsPerSecond;
    [SerializeField] private int w_damagePerBullet;
    [SerializeField] private int w_bulletPerShot;
    [SerializeField] private int w_bulletPerMagazine;
    [SerializeField] private int w_bulletPerStock;
    [SerializeField] private int w_dropRange;
    [SerializeField] private float w_spreadAngle = 0.0f;
    [SerializeField] private float w_spreadRadius = 0.0f;
    [SerializeField] private float w_recoil = 0.0f;


    private string w_assetName;
    [SerializeField] private string w_description;
    public Type w_type;
    private AmmoType w_damageType;
    [SerializeField] private bool w_canShoot = true;
    private float w_reloadTime;

    #endregion

    #region Objects

    [SerializeField] private VFXBehaviour vfxmanagerBlood;
    [SerializeField] private VFXBehaviour vfxmanagerMuzle;

    [SerializeField] private GameObject animationObject;
    [SerializeField] private GameObject floatingObject;
    [SerializeField] private VisualEffect fire02;
    [SerializeField] private VisualEffect fire01;
    private int w_shotgunShells;

    [SerializeField] private WeaponAnimationHandler w_animHandler;
    [SerializeField] private RecoilRotator cameraRecoil;

    #endregion


    private void Start()
    {
        w_currentMagazine = w_bulletPerMagazine;
        w_maxAmmo = w_bulletPerStock;
        audioManager = FindAnyObjectByType<AudioManager>();
        instanceEventShoot = FMODUnity.RuntimeManager.CreateInstance(eventShoot);
        audioManager.AddSound(eventShoot, instanceEventShoot);
        instanceEventReload = FMODUnity.RuntimeManager.CreateInstance(eventReload);
        audioManager.AddSound(eventReload, instanceEventReload);

        InitializeWeapon();

        cameraRecoil = Camera.main.transform.parent.transform.GetComponent<RecoilRotator>();
        w_cooldown = 1.0f / w_shotsPerSecond;
    }

    private void Update()
    {
        // Update Shot position
        instanceEventShoot.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        instanceEventReload.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));


    }

    public void AnimShoot()
    {
        if (w_canShoot && w_isCoolingDown == false)
        {
            if (w_type != Type.Mellee)
            {
                w_animHandler.TriggerShoot();
            }
            else
            {
                w_animHandler.TriggerMelee();
            }
            w_canShoot = false;
        }
    }

    public void Shoot()
    {
        if (w_type == Type.Mellee)
        {
            HandleMellee();
            return;

        }
        else if (w_type == Type.Shotgun)
        {
            instanceEventShoot.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
            instanceEventShoot.start();
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RayCheck(ray);
            cameraRecoil.AddRecoil(new Vector3(-w_recoil, Random.Range(-w_recoil / 2, w_recoil / 2), 0f));
            w_currentMagazine = 0;
            w_canReload = true;
            HideShotgunLights();
            Reload();
            return;
        }
        if (w_currentMagazine > 0)
        {
            // shot play 
            instanceEventShoot.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
            instanceEventShoot.start();
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if (w_currentMagazine > 0)
            {
                RayCheck(ray);
                w_currentMagazine--;
            }
            else
            {
                Reload();
            }
            cameraRecoil.AddRecoil(new Vector3(-w_recoil, Random.Range(-w_recoil / 2, w_recoil / 2), 0f));
        }
    }

    private void HandleMellee()
    {
        if (w_animHandler)
        {
            w_animHandler.TriggerMelee();
        }
    }

    private void RayCheck(Ray _ray)
    {
        if (vfxmanagerMuzle) vfxmanagerMuzle.CreateVFXWithTimer(1.0f);
        RaycastHit hit;
        for (int i = 0; i < (w_bulletPerShot >= 1 ? w_bulletPerShot : 1); i++)
        {
            Vector3 spreadDirection = _ray.direction;
            spreadDirection += new Vector3(
                Random.Range(-w_spreadRadius, w_spreadRadius),
                Random.Range(-w_spreadRadius, w_spreadRadius),
                Random.Range(-w_spreadRadius, w_spreadRadius)
            );
            spreadDirection.Normalize();
            Ray spreadRay = new Ray(_ray.origin, spreadDirection);

            // Raycast using the spread direction
            Physics.Raycast(spreadRay, out hit, w_type == Type.Shotgun ? 25.0f : 100.0f, ~LayerMask.GetMask("Player", "Ignore Raycast", "LinkClimber", "Projectile", "Weapon"));
            if (hit.transform != null)
            {
                IDamageable damageable = hit.transform.GetComponentInParent<IDamageable>();

                if (damageable != null)
                {
                    if (hit.transform.CompareTag("Player")) return;

                    if (hit.transform.CompareTag("Enemy"))
                    {
                        damageable.TakeDamage(w_damagePerBullet);
                        vfxmanagerBlood.CreateVFX(hit.point);

                    }
                    else if (hit.transform.CompareTag("Head"))
                    {
                        damageable.TakeDamage(w_damagePerBullet * 2/*Damage multiplicator of headshot */);
                        hit.transform.GetComponent<HeadShot>().VFXActivation();
                        vfxmanagerBlood.CreateVFX(hit.point);
                    }
                    else if (hit.transform.CompareTag("Destructible"))
                    {
                        damageable.TakeDamage(w_damagePerBullet);
                    }
                }

                if (hit.transform.CompareTag("Unbreakable"))
                {
                    hit.transform.gameObject.GetComponent<Unbreakable>().VFXActivation(hit.point);
                }
            }
        }
    }

    public void ReloadAmmunition()
    {
        w_canReload = true;
        if (w_currentMagazine < w_bulletPerMagazine && w_maxAmmo > 0)
        {
            int missingAmmo = w_bulletPerMagazine - w_currentMagazine;
            if (w_maxAmmo >= missingAmmo)
            {
                w_maxAmmo -= missingAmmo;
                w_currentMagazine += missingAmmo;
            }
            else
            {
                w_currentMagazine += w_maxAmmo;
                w_maxAmmo = 0;
            }
        }
        w_canShoot = true;
    }

    public void ShotgunReload()
    {
        if (w_maxAmmo > 0)
        {
            w_maxAmmo--;
            w_currentMagazine++;
        }
        if (w_currentMagazine >= 2)
        {
            w_canReload = true;
            w_canShoot = true;
        }
        UpdateShotgunLights();
    }

    public void AllowShooting()
    {
        if (w_currentMagazine > 0)
            w_canShoot = true;
    }
    public void MelleeCanSwing()
    {
        w_canShoot = true;
    }

    public void Reload()
    {
        if (w_canReload && (w_maxAmmo > 0) && (w_currentMagazine < w_bulletPerMagazine))
        {
            w_canReload = false;
            if (w_animHandler)
            {
                w_animHandler.TriggerReload();
            }
            instanceEventReload.start();
            w_canShoot = false;
        }
    }

    public void ReloadShotgun()
    {
        w_canReload = false;
        if (w_animHandler)
        {
            w_animHandler.TriggerReload();
        }
        instanceEventReload.start();
        w_canShoot = false;
    }

    public void AddAmmo()
    {
        if (w_maxAmmo > w_bulletPerStock - w_bulletPerMagazine)
        {
            w_maxAmmo = w_bulletPerStock;
        }
        else
        {
            w_maxAmmo += w_bulletPerMagazine;
        }
    }

    private void InitializeWeapon()
    {
        w_assetName = gameObject.name;
        string path = Path.Combine(Application.streamingAssetsPath, "Weapons", w_assetName + ".json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            WeaponStats weaponStats = JsonConvert.DeserializeObject<WeaponStats>(json);
            w_name = weaponStats.name;
            w_shotsPerSecond = weaponStats.shotsPerSecond;
            w_damagePerBullet = weaponStats.damagePerBullet;
            w_bulletPerShot = weaponStats.bulletPerShot;
            w_bulletPerMagazine = weaponStats.bulletPerMagazine;
            w_bulletPerStock = weaponStats.bulletPerStock;
            w_dropRange = weaponStats.dropRange;
            w_assetName = weaponStats.assetName;
            w_description = weaponStats.description;
            w_type = weaponStats.type;
            w_damageType = weaponStats.damageType;
        }
    }

    public Type CheckType()
    {
        return w_type;
    }

    public void OnPickup()
    {
        if (animationObject)
            this.animationObject.SetActive(true);
        if (floatingObject)
            this.floatingObject.SetActive(false);
    }

    public void OnDrop()
    {
        if (animationObject)
            this.animationObject.SetActive(false);
        if (floatingObject)
            this.floatingObject.SetActive(true);
    }

    public void ToggleChildRenderer(bool _a)
    {
        animationObject.SetActive(_a);
    }

    public void HideShotgunLights()
    {
        if (fire01 != null)
            fire01.enabled = false;
        if (fire02 != null)
            fire02.enabled = false;
    }

    public void UpdateShotgunLights()
    {
        switch (w_currentMagazine)
        {
            case 0:
                fire01.enabled = false;
                fire02.enabled = false;
                break;
            case 1:
                fire01.enabled = true;
                fire02.enabled = false;
                break;
            case 2:
                fire01.enabled = true;
                fire02.enabled = true;
                break;
        }
    }

    private void OnEnable()
    {
        w_canReload = true;
        w_canShoot = true;
    }

    public void CreateBlood(Vector3 _position)
    {
        vfxmanagerBlood.CreateVFX(_position);
    }
}