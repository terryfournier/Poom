using UnityEngine;

public class HeadShot : MonoBehaviour
{
    [SerializeField] GameObject m_headShotPrefabs;
    private EnemyBehaviour m_enemy;

    private void Start()
    {
        m_enemy = GetComponentInParent<EnemyBehaviour>();
    }

    public void VFXActivation()
    {
        switch (m_enemy.Type)
        {
            case EnemyType.TANK:
                Instantiate(m_headShotPrefabs, transform.position + Vector3.up * 0.5f, Quaternion.identity);
                break;
            default:
                Instantiate(m_headShotPrefabs, transform.position, Quaternion.identity);
                break;
        }

    }
}
