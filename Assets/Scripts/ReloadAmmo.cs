using UnityEngine;

public class ReloadAmmo : MonoBehaviour
{
    [SerializeField] private Weapon weapon;

    private void Start()
    {
        if (weapon == null)
        {
            weapon = GetComponent<Weapon>();
        }
    }

    public void Reload()
    {
        weapon.ReloadAmmunition();
    }

    public void ShotgunShell()
    {
        weapon.ShotgunReload();
    }

}
