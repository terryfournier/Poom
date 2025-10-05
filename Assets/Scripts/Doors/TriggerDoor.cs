using UnityEngine;

public class TriggerDoor : MonoBehaviour
{
    [SerializeField] private DoorBehaviour doorBehaviour;
    [SerializeField] private bool isOpen = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isOpen)
        {
            isOpen = true;
            doorBehaviour.OpenDoorAnimation();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isOpen)
        {
            isOpen = false;
            doorBehaviour.OpenDoorAnimation();
        }
    }
}
