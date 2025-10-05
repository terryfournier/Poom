using UnityEngine;

public class InputTest : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            foreach(EnemyHealthManager en in FindObjectsByType<EnemyHealthManager>(FindObjectsSortMode.None))
            {
                en.TakeDamage(5);
            }
        }
    }
}
