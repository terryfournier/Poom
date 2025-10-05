using UnityEngine;

public class EnemyBleeding : MonoBehaviour
{
    private BloodSpotManager m_bloodSpot;
    private PlayerController m_player;
    private EnemyHealthManager m_enemyHealthManager;
    private float m_distanceMax = 8.0f;

    private void Start()
    {
        m_bloodSpot = FindAnyObjectByType<BloodSpotManager>();
        m_player = FindAnyObjectByType<PlayerController>();
        m_enemyHealthManager = GetComponent<EnemyHealthManager>();
        m_enemyHealthManager.OnDamageSet += BleedingDeath;

    }

    private void OnDestroy()
    {
        if (m_enemyHealthManager)
            m_enemyHealthManager.OnDamageSet -= BleedingDeath;
    }

    public void BleedingDeath()
    {
        if (Vector3.Distance(transform.position, m_player.transform.position) < m_distanceMax)
        {
            m_bloodSpot.ActivateBloodSpot();
        }
    }
}
