using UnityEngine;
using System.Collections.Generic;

public class hitboxmellee : MonoBehaviour
{
    [SerializeField] private Weapon thisWeapon;
    private BloodSpotManager m_bloodSpot;
    public float damageAmount = 25.0f;


    private HashSet<IDamageable> hitEntities = new HashSet<IDamageable>();
    private bool canTrackHits = false;

    private void Start()
    {
        m_bloodSpot = FindAnyObjectByType<BloodSpotManager>();
    }

    public void BeginSwing()
    {
        hitEntities.Clear();
        canTrackHits = true;
    }

    public void ApplySwingDamage()
    {
        foreach (var obj in hitEntities)
        {
            obj.TakeDamage(damageAmount);
        }

        canTrackHits = false;
        thisWeapon.MelleeCanSwing();

    }

    public void DetectEnemiesHit()
    {

        //sphere cast
        RaycastHit[] hits = Physics.SphereCastAll(Camera.main.transform.position, 1f, Camera.main.transform.forward, 3f);

        foreach (RaycastHit hit in hits)
        {
            IDamageable damageable = hit.transform.GetComponentInParent<IDamageable>();

            if (damageable != null && !hit.transform.CompareTag("Player"))
            {
                hitEntities.Add(damageable);
                if (!hit.transform.CompareTag("Destructible"))
                {
                    thisWeapon.CreateBlood(hit.transform.position);
                    m_bloodSpot.ActivateBloodSpot();
                }
            }

        }
    }
}


