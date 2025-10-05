using System;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [SerializeField] EnemySpawnPoint[] spawnersTank;
    [SerializeField] EnemySpawnPoint[] spawnerDistance;
    [SerializeField] EnemySpawnPoint[] spawnerClassic;

    [SerializeField]  GameObject enemiesContainer;

    int wave;

    public Action newWave;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        wave = 1;

        foreach(EnemySpawnPoint st in spawnersTank)
        {
            st.m_numEnemies = 1;
            st.m_coolDownSpawn = 1;
            st.m_wave = wave;
            st.ActiveGeneration();
        }
        foreach(EnemySpawnPoint sd in spawnerDistance)
        {
            sd.m_numEnemies = 3;
            sd.m_coolDownSpawn = 0.45f;
            sd.m_wave = wave;
            sd.ActiveGeneration();
        }
        foreach(EnemySpawnPoint sc in spawnerClassic)
        {
            sc.m_numEnemies = 5;
            sc.m_coolDownSpawn = 0.30f;
            sc.m_wave = wave;
            sc.ActiveGeneration();
        }
    }

    // Update is called once per frame
    void Update()
    {
       if(enemiesContainer.transform.childCount <= 0 && SpawnersEndedSpawn())
        {
            wave++;

            foreach (EnemySpawnPoint st in spawnersTank)
            {
                st.m_numEnemies = wave % 10 + 1;
                st.m_coolDownSpawn = 1 - 0.05f * wave;
                st.m_wave = wave;
                st.ActiveGeneration();
            }
            foreach (EnemySpawnPoint sd in spawnerDistance)
            {
                sd.m_numEnemies = wave % 3 + 3;
                sd.m_coolDownSpawn = 0.45f - 0.025f * wave;
                sd.m_wave = wave;
                sd.ActiveGeneration();
            }
            foreach (EnemySpawnPoint sc in spawnerClassic)
            {
                sc.m_numEnemies = wave % 5 + 5;
                sc.m_coolDownSpawn = 0.30f - 0.01f * wave;
                sc.m_wave = wave;
                sc.ActiveGeneration();
            }

            newWave?.Invoke();
        }
    }

    private bool SpawnersEndedSpawn()
    {
        foreach (EnemySpawnPoint st in spawnersTank)
        {
            if(st.m_numEnemies != 0)
            {
                return false;
            }

        }
        foreach (EnemySpawnPoint sd in spawnerDistance)
        {
            if (sd.m_numEnemies != 0)
            {
                return false;
            }
        }
        foreach (EnemySpawnPoint sc in spawnerClassic)
        {
            if (sc.m_numEnemies != 0)
            {
                return false;
            }
        }
        return true;
    }

    public int GetWave()
    {
        return wave;
    }
}
