using UnityEngine;

public class DoorLock : MonoBehaviour
{

    [SerializeField] private DoorBehaviour doorBehaviour;
    [SerializeField] private bool isOpen = false;

    [SerializeField] private GameObject key;
    private BoxCollider boxCollider;



    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }


    private void Update()
    {
        if (key == null)
        {
            isOpen = true;
            doorBehaviour.OpenDoorAnimation();
            boxCollider.enabled = false;
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isOpen)
        {
            isOpen = true;
            doorBehaviour.CloseDoorAnimation();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !isOpen)
        {
            isOpen = false;
            doorBehaviour.CloseDoorAnimation();
        }
    }
}
