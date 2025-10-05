using FMOD.Studio;
using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerPickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] private GameObject weaponAnchor;
    [SerializeField] private GameObject floatingTextPrefab;
    [SerializeField] private EventReference eventPickUp;
    [SerializeField] private GameObject[] itemPickupOnStart = null;

    private EventInstance instanceEventPickUp;

    private GameObject floatingText;
    private TMP_Text floatingTextTMP;

    private List<GameObject> nearbyWeapons = new List<GameObject>();
    private GameObject currentWeapon;

    private PlayerInventory playerInventory;

    private AudioManager audioManager;

    public GameObject WeaponToPickup => currentWeapon;

    private TMP_Text interract;

    private void Awake()
    {
        playerInventory = GetComponent<PlayerInventory>();
    }

    private void Start()
    {
        instanceEventPickUp = FMODUnity.RuntimeManager.CreateInstance(eventPickUp);

        interract = GameObject.Find("Interract").GetComponent<TMP_Text>();

        audioManager = FindAnyObjectByType<AudioManager>();

        if (audioManager)
        {
            audioManager.AddSound(eventPickUp, instanceEventPickUp);
        }

        if (itemPickupOnStart != null && itemPickupOnStart.Length > 0)
        {
            PickupOnStart();
        }
    }

    private void Update()
    {
        instanceEventPickUp.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));

        if (nearbyWeapons.Count > 0)
        {
            UpdateCurrentWeapon();

            if (floatingText != null && currentWeapon != null)
                floatingText.transform.position = currentWeapon.transform.position + Vector3.up * 0.5f;

            if ((Helper.CurrentKeyboard.eKey.wasPressedThisFrame || (Helper.GamepadUsable && Gamepad.current.buttonWest.wasPressedThisFrame)))
            {
                if (interract != null)
                    interract.enabled = false;
                PickupWeapon();
            }
        }
    }

    private void PickupWeapon()
    {
        if (currentWeapon == null) return;

        instanceEventPickUp.start();

        // Disable colliders
        foreach (var collider in currentWeapon.GetComponents<Collider>())
            collider.enabled = false;

        playerInventory.AddWeapon(currentWeapon);

        currentWeapon.transform.SetParent(weaponAnchor.transform);
        currentWeapon.transform.localScale = Vector3.one;
        currentWeapon.transform.position = weaponAnchor.transform.position;
        currentWeapon.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);

        currentWeapon.GetComponent<Weapon>().ToggleChildRenderer(true);

        nearbyWeapons.Remove(currentWeapon);
        currentWeapon = null;

        UpdateFloatingText();
    }

    private void PickupOnStart()
    {
        foreach (GameObject item in itemPickupOnStart)
        {
            instanceEventPickUp.start();

            // Disable colliders
            if (item)
            {
                foreach (Collider collider in item.GetComponents<Collider>())
                {
                    collider.enabled = false;
                }

                playerInventory.AddWeapon(item);
                item.transform.SetParent(weaponAnchor.transform);
                item.transform.localScale = Vector3.one;
                item.transform.position = weaponAnchor.transform.position;
                item.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
                item.GetComponent<Weapon>().ToggleChildRenderer(true);
                currentWeapon = null;
            }

        }

    }



    private void UpdateCurrentWeapon()
    {
        if (nearbyWeapons.Count == 0)
        {
            currentWeapon = null;
            UpdateFloatingText();
            return;
        }

        Transform cam = Camera.main.transform;
        float bestDot = 0.85f; // ~30 degrees cone
        GameObject lookingAt = null;

        // First, try to find weapon being looked at
        foreach (var weapon in nearbyWeapons)
        {
            if (weapon == null) continue;

            Vector3 toWeapon = (weapon.transform.position - cam.position).normalized;
            float dot = Vector3.Dot(cam.forward, toWeapon);

            if (dot > bestDot)
            {
                bestDot = dot;
                lookingAt = weapon;
            }
        }

        // Prioritize look-at, fallback to closest
        if (lookingAt != null)
        {
            currentWeapon = lookingAt;
        }
        else
        {
            float closestDist = float.MaxValue;
            GameObject closest = null;

            foreach (var weapon in nearbyWeapons)
            {
                if (weapon == null) continue;

                float dist = Vector3.Distance(transform.position, weapon.transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closest = weapon;
                }
            }

            currentWeapon = closest;
        }

        UpdateFloatingText();
    }

    private void UpdateFloatingText()
    {
        if (currentWeapon == null)
        {
            if (floatingTextTMP != null)
                floatingTextTMP.text = "";

            if (floatingText != null)
                Destroy(floatingText);

            return;
        }

        if (floatingText != null)
            Destroy(floatingText);

        floatingText = Instantiate(floatingTextPrefab);
        floatingText.transform.position = currentWeapon.transform.position /*+ Vector3.up * 0.5f*/;

        floatingTextTMP = floatingText.GetComponentInChildren<TMP_Text>();
        if (floatingTextTMP != null)
        {
            string inputKey = Helper.GamepadUsable ? "X" : "E";
            floatingTextTMP.fontSize = 10;
            floatingTextTMP.text = $"Press {inputKey} to pick up {currentWeapon.name}";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon") && !nearbyWeapons.Contains(other.gameObject))
        {
            if (interract != null)
                interract.enabled = true;
            nearbyWeapons.Add(other.gameObject);
            UpdateCurrentWeapon();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            interract.enabled = false;
            nearbyWeapons.Remove(other.gameObject);
            UpdateCurrentWeapon();
        }
    }
}
