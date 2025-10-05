using UnityEngine;

public class DoorManagedByKeys : MonoBehaviour
{
    [SerializeField] Key[] keys;

    // Update is called once per frame
    void Update()
    {
        if(keys == null)
        {
            gameObject.SetActive(false);
        }
    }
}
