using System.Collections;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField] private GameObject m_prefabEnemy;
    public int m_numEnemies;
    public float m_coolDownSpawn;
    private GameObject m_enemies;
    public int m_wave;
    
    private void Start()
    {
        m_enemies = GameObject.Find("Enemies");
        if(m_enemies == null)
        {
            m_enemies = new GameObject("Enemies");
        }
    }

    //Active the generation will be called by the box
    public void ActiveGeneration()
    {
        StartCoroutine(SpawnerCoroutine());
    }

    //Spawner that will make the enemy spawned really good 
    private IEnumerator SpawnerCoroutine()
    {
        yield return new WaitForSeconds(5.0f);

        while (m_numEnemies > 0)
        {
            m_numEnemies--;
            GameObject childEnemy = Instantiate(m_prefabEnemy, transform.position, Quaternion.identity);
            if(m_wave != 0)
            {
                childEnemy.GetComponentInChildren<EnemyBehaviour>().m_Multipler = m_wave;
            }
            childEnemy.transform.SetParent(m_enemies.transform);
            yield return new WaitForSeconds(m_coolDownSpawn);
        }
    }
}
