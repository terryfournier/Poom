using System.Collections;
using UnityEngine;

public class EnemySpawnPointInfinity : MonoBehaviour
{
    [SerializeField] private GameObject prefabEnemy;
    [SerializeField] private int numEnemies;
    [SerializeField] public float coolDownSpawn;
    private GameObject enemies;
    private float timer;
    private EnemyData data;
    private Coroutine coro;
    private bool isSpawning = true;
    
    private void Start()
    {
        enemies = GameObject.Find("Enemies");
        if(enemies == null)
        {
            enemies = new GameObject("Enemies");
        }
    }
    private void Update()
    {
        if(isSpawning)
        {
            coro = StartCoroutine(SpawnerCoroutine());
            isSpawning = false;
        }
        if(coro != null)
        {
            timer -= Time.deltaTime;
        }
    }

    //Active the generation will be called by the box
    public void ActiveGeneration()
    {
        isSpawning = true;
    }

    //Spawner that will make the enemy spawned really good 
    private IEnumerator SpawnerCoroutine()
    {
        yield return new WaitForSeconds(5.0f);

        while (numEnemies > 0)
        {
            numEnemies--;
            GameObject childEnemy = Instantiate(prefabEnemy, transform.position, Quaternion.identity);
            childEnemy.transform.SetParent(enemies.transform);
            yield return new WaitForSeconds(coolDownSpawn);
        }
        coro = null;
    }
}
