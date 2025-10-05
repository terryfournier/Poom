using FMODUnity;
using UnityEngine;
using UnityEngine.VFX;

public class MunitionBag : MonoBehaviour
{
    private float existDuration;
    private float m_maxLife = 90.0f;
    private VisualEffect m_vfxHi;

    private void Start()
    {
        m_vfxHi = GetComponent<VisualEffect>();
        m_vfxHi.SetFloat("CircleLifeTime", m_maxLife);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();


            GameObject[] weapons = inventory.GetWeapons();
            foreach (var weapon in weapons)
            {
                if(weapon != null)
                {
                    weapon.GetComponent<Weapon>().AddAmmo();
                }
               
            }

            Destroy(gameObject);
        }
    }

    private void Update()
    {
        existDuration += Time.deltaTime;
        if (existDuration >= m_maxLife)
        {
            Destroy(GetComponent<StudioEventEmitter>());
            Destroy(gameObject);
        }
    }
}
