using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    [SerializeField] Transform playerPos;
    [SerializeField] PlayerHealthManager healthManager;
    [SerializeField] float limitY;

    // Update is called once per frame
    void Update()
    {
        if(playerPos.position.y <= limitY)
        {
            playerPos.position = transform.position;
            healthManager.TakeDamage(10);
        }
    }
}
