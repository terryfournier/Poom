using System.Collections;
using UnityEngine;

public class ShootHandler : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private Light lightMuzle;

    public void Shoot()
    {
        if (weapon != null)
        {
            weapon.Shoot();

            StartCoroutine(LightFadeMuzle());
        }
    }

    public void EndShoot()
    {
        if (weapon != null)
        {
            weapon.AllowShooting();
        }
    }

    IEnumerator LightFadeMuzle()
    {
        lightMuzle.enabled = true;
        yield return new WaitForSeconds(0.1f);
        lightMuzle.enabled = false;
    }


}
