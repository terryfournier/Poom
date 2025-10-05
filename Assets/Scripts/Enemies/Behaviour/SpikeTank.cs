using UnityEngine;

public class SpikeTank : MonoBehaviour
{
    private float m_damage = 5;
    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        
        if(other.TryGetComponent<IDamageable>(out IDamageable dmg))
        {
            dmg.TakeDamage(m_damage);
        }
    }
}
