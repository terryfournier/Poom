using UnityEngine;
using UnityEngine.Events;

public class DestructibleBox : MonoBehaviour, IDamageable
{
    [SerializeField] private UnityEvent m_OnDestructable;

    public void TakeDamage(float _damage)
    {
        if (m_OnDestructable != null)
            m_OnDestructable.Invoke();
        Destroy(gameObject);
    }
}
