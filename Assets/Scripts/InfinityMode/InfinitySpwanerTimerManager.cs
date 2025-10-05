using UnityEngine;

public class InfinitySpwanerTimerManager : MonoBehaviour
{
    private EnemySpawnPointInfinity[] enemySpawnPoints;
    [SerializeField] TimerInfinityMode timerInfinityMode;

    float floorToReach;

    private void Start()
    {
        enemySpawnPoints = GetComponentsInChildren<EnemySpawnPointInfinity>();
        floorToReach = 5 * 60;
    }
    // Update is called once per frame
    void Update()
    {
        if(timerInfinityMode.timer >= floorToReach)
        {
            foreach(EnemySpawnPointInfinity swpaner in enemySpawnPoints)
            {
                swpaner.coolDownSpawn -= 0.2f;
            }
            floorToReach += 5 * 60;
        }
    }
}
