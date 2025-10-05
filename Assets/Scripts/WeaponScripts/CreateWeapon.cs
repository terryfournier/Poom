using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CreateWeapon : MonoBehaviour
{
    [SerializeField] private string weaponName;

    private void Update()
    {
        if (Helper.CurrentKeyboard.cKey.IsPressed())
        {
            DropCreatedWeapon("RIFLE");

        }

    }


    void DropCreatedWeapon(string _weaponName)
    {
        GameObject newWeaponFromPrefab = (GameObject)Instantiate(Resources.Load(_weaponName), this.transform.position, Quaternion.identity);

        newWeaponFromPrefab.AddComponent<Weapon>();
        Weapon weapon = newWeaponFromPrefab.GetComponent<Weapon>();


    }

    public void OnDropCurrentWeapon(GameObject _current)
    {
        _current.transform.parent = null;   
    }

}
