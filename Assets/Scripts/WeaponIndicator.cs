using NUnit.Framework.Constraints;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class WeaponIndicator : MonoBehaviour
{
    [SerializeField] public GameObject player;
    [SerializeField] public TMP_Text weaponName;
    [SerializeField] public TMP_Text ammunition;
    private RawImage ammuSprite;
    private PlayerInventory inventory;


    private void Start()
    {
        //Get the delegate from the player inventory
        player.GetComponent<WeaponIndicator>();
        inventory = player.GetComponent<PlayerInventory>();
    }



    private void FixedUpdate()
    {
        if (inventory.GetWeaponHeld() == null)
        {
            weaponName.text = "Unarmed";
            ammunition.text = " 0 / 0 ";
        }
        else
        {
            weaponName.text = inventory.GetWeaponHeld().name;
            ammunition.text = inventory.GetWeaponHeld().GetComponent<Weapon>().w_currentMagazine + " / " + inventory.GetWeaponHeld().GetComponent<Weapon>().w_maxAmmo;
        }
    }

    private void UpdateWeaponIndicator()
    {

    }



}

