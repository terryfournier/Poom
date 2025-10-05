using FMOD.Studio;
using FMODUnity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] CorruptionFill corupFill;
    public int activeWeaponIndex = 0;
    public int syringeNb;


    private int weaponSlots = 3;

    delegate void OnWeaponChange(int index);

    private BoxCollider boxCollider;

    private CreateWeapon createWeapon;
    [SerializeField] private GameObject[] weapons;
    [SerializeField] private EventReference eventHeal;
    private EventInstance eventInstanceHeal;
    [SerializeField] private GameObject armsHeal;

    private AudioManager audioManager = null;
    private float animTimeHeal;
    private float durationAnimHeal;
    private bool isHealing;

    private void Awake()
    {
        this.AddComponent<CreateWeapon>();
        weapons = new GameObject[3];
        createWeapon = GetComponent<CreateWeapon>();
    }

    private void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();

        eventInstanceHeal = FMODUnity.RuntimeManager.CreateInstance(eventHeal);
        audioManager.AddSound(eventHeal, eventInstanceHeal);
        boxCollider = GetComponent<BoxCollider>();
        animTimeHeal = 0.0f;
        durationAnimHeal = 5.0f;
        isHealing = false;
    }

    private void Update()
    {
        animTimeHeal += Time.deltaTime;
        eventInstanceHeal.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        if ((Helper.CurrentKeyboard.qKey.wasPressedThisFrame
            || (Helper.GamepadUsable && ControlsManager.HasPressed_Gamepad(GamepadActions.HEAL, false))
            ) && syringeNb > 0)
        {
            syringeNb--;
            armsHeal.SetActive(true);
            eventInstanceHeal.start();
            corupFill.ReduceCorruption(50);
            animTimeHeal = 0.0f;
            isHealing = true;

            GameObject activeWeapon = weapons[activeWeaponIndex];

            if (activeWeapon)
            {
                activeWeapon.SetActive(false);
            }
        }

        if(animTimeHeal >= durationAnimHeal && isHealing)
        {
            isHealing = false;
            armsHeal.SetActive(false);
            weapons[activeWeaponIndex].SetActive(true);
        }





#if UNITY_EDITOR
        if (Helper.CurrentKeyboard.tKey.wasPressedThisFrame)
        {
            for (int i = 0; i < 3; i++)
            {
                if (weapons[i] != null)
                {
                    //Delete on build
                    weapons[i].GetComponent<Weapon>().AddAmmo();
                }
            }
        }
#endif
    }

    private void FixedUpdate()
    {
        if (Helper.GamepadUsable)
        {
            Gamepad current = Gamepad.current;

            if (ControlsManager.HasPressed_Gamepad(GamepadActions.SCROLL_WPN_UP, false))
            {
                ++activeWeaponIndex;
                activeWeaponIndex %= 3;
                WeaponSwitched();
            }
            else if (ControlsManager.HasPressed_Gamepad(GamepadActions.SCROLL_WPN_DOWN, false))
            {
                --activeWeaponIndex;
                activeWeaponIndex = activeWeaponIndex < 0 ? weaponSlots - 1 : activeWeaponIndex;
                WeaponSwitched();
            }

            if (ControlsManager.HasPressed_Gamepad(GamepadActions.PICK_WPN_1, false))
            {
                activeWeaponIndex = 0;
                WeaponSwitched();
            }
            else if (ControlsManager.HasPressed_Gamepad(GamepadActions.PICK_WPN_2, false))
            {
                activeWeaponIndex = 2;
                WeaponSwitched();
            }
            else if (ControlsManager.HasPressed_Gamepad(GamepadActions.PICK_WPN_3, false))
            {
                activeWeaponIndex = 1;
                WeaponSwitched();
            }
        }
        else
        {
            if (Helper.MouseUsable)
            {
                if (Helper.GetAxis_Mouse(false, true) > 0f)
                {
                    ++activeWeaponIndex;
                    activeWeaponIndex %= 3;
                    WeaponSwitched();
                }
                else if (Helper.GetAxis_Mouse(false, true) < 0f)
                {
                    --activeWeaponIndex;
                    activeWeaponIndex = activeWeaponIndex < 0 ? weaponSlots - 1 : activeWeaponIndex;
                    WeaponSwitched();
                }
            }


            if (Helper.KeyboardUsable)
            {
                if (ControlsManager.HasPressed_Keyboard(KeyboardActions.PICK_WPN_1, false))
                {
                    activeWeaponIndex = 0;
                    WeaponSwitched();
                }
                else if (ControlsManager.HasPressed_Keyboard(KeyboardActions.PICK_WPN_2, false))
                {
                    activeWeaponIndex = 2;
                    WeaponSwitched();
                }
                else if (ControlsManager.HasPressed_Keyboard(KeyboardActions.PICK_WPN_3, false))
                {
                    activeWeaponIndex = 1;
                    WeaponSwitched();
                }
            }
        }
    }

    private void WeaponSwitched()
    {
        for (int i = 0; i < 3; ++i)
        {
            if (weapons[i] != null)
            {
                if (i == activeWeaponIndex && weapons[i] != null)
                {
                    weapons[i].SetActive(true);
                }
                else
                {
                    weapons[i].SetActive(false);
                }
            }
        }
    }

    public void AddWeapon(in GameObject _weaponObj)
    {
        if (_weaponObj)
        {
            Weapon weapon = _weaponObj.GetComponent<Weapon>();
            if (weapon == null) return;
            weapon.OnPickup();
            if (weapon.CheckType() == Type.Mellee)
            {
                if (weapons[0] == null) { weapons[0] = _weaponObj; }
                else { DropWeapon(0); weapons[0] = _weaponObj; }
                activeWeaponIndex = 0;
            }
            else if (weapon.CheckType() == Type.Gun)
            {
                if (weapons[1] == null) { weapons[1] = _weaponObj; }
                else { DropWeapon(1); weapons[1] = _weaponObj; }
                activeWeaponIndex = 1;
            }
            else
            {
                if (weapons[2] == null) { weapons[2] = _weaponObj; }
                else { DropWeapon(2); weapons[2] = _weaponObj; }
                activeWeaponIndex = 2;
            }
            WeaponSwitched();
        }
    }

    public void RemoveWeapon(in GameObject _weaponObj)
    {
        createWeapon.OnDropCurrentWeapon(weapons[activeWeaponIndex]);
    }

    private void DropWeapon(int _index)
    {
        BoxCollider[] boxCollider = weapons[_index].GetComponents<BoxCollider>();
        for (int i = 0; i < boxCollider.Length; i++)
        {
            boxCollider[i].enabled = true;
        }
        createWeapon.OnDropCurrentWeapon(weapons[_index]);

        //raycast drop weapon
        Ray ray = new Ray(this.transform.position, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, LayerMask.GetMask("Ground")))
        {
            weapons[_index].transform.position = hit.point;
            weapons[_index].transform.rotation = Quaternion.identity;

        }

        weapons[_index].GetComponent<Weapon>().OnDrop();
        weapons[_index]= null;
    }

    private void OnTriggerEnter(Collider other)
    {
        ICollectable collectable = other.GetComponent<ICollectable>();
        if (collectable != null) { collectable.Collect(this); }
    }

    public GameObject GetWeaponHeld()
    {
        return weapons[activeWeaponIndex];
    }

    public void Shoot()
    {
        if (weapons[activeWeaponIndex] == null) return;
        weapons[activeWeaponIndex].GetComponent<Weapon>().AnimShoot();
    }

    public GameObject[] GetWeapons()
    {
        return weapons;
    }

}
