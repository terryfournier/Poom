using UnityEngine;

public class TriggerBoxEnemyGenerator : MonoBehaviour
{
    [SerializeField] private GameObject m_spawnPointGO;
    private bool m_generationActivate = false;
    private EnemySpawnPoint[] m_spawnPoint;

    // The gameObject Permit to get all the spawnPoint in his childrens
    private void Start()
    {
        m_spawnPoint = m_spawnPointGO.GetComponentsInChildren<EnemySpawnPoint>();
    }

    // When the player enter in the trigger box the generation will be activate
    // Then you cant activate the generation again
    private void OnTriggerEnter(Collider other)
    {
        if(!m_generationActivate && other.tag == "Player")
        {
            foreach(EnemySpawnPoint spPoint in m_spawnPoint)
            {
                spPoint.ActiveGeneration();
            }
            m_generationActivate = true;
        }
    }
}
