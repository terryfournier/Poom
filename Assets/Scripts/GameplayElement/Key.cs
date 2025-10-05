using UnityEngine;

public class Key : MonoBehaviour
{

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
