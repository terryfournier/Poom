using UnityEngine;

public class DoorManagedByEnnemiGenerator : MonoBehaviour
{
    [SerializeField] EnemySpawnPoint enemySpawnPoint;
    [SerializeField] Transform transformToMoveAt;

    // Update is called once per frame
    void Update()
    {
        // stay active if there's ennemies 
        if (enemySpawnPoint.m_numEnemies > 0)
        {
            gameObject.SetActive(true);
        }
        else
        {
            // move to the choosen position
            Vector3.Lerp(transform.position, transformToMoveAt.position, Time.deltaTime);

            // if we are close to the wanted position
            if (Vector3.Distance(transform.position, transformToMoveAt.position) <= 0.5f)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
