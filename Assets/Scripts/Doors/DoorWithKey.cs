using UnityEngine;

public class DoorWithKey : MonoBehaviour
{

    [SerializeField] private DoorBehaviour doorBehaviour;
    [SerializeField] private bool isOpen = false;
    [SerializeField] private GameObject[] key;
    private bool shouldOpen;

    private void Start()
    {
        // Initialize the door state
        isOpen = false;
    }

    private void Update()
    {
        if (!isOpen)
        {
            foreach (GameObject go in key)
            {
                isOpen = true;
                if (go != null)
                {
                    isOpen = false;
                    return;
                }

            }

            if (isOpen)
            {
                doorBehaviour.OpenDoorAnimation();
                isOpen = true;
            }
        }
    }
}