using UnityEngine;

public class RotatingDoor : MonoBehaviour
{
    public bool isOpen = false;
    [SerializeField] public float rotationSpeed = 90f; // degrees per second.









    private void Update()
    {
        if (isOpen)
        {
            RotateDoor();
        }
    }

    private void RotateDoor()
    {
        // Rotate the door around its Y-axis.
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 90, 0), Time.deltaTime * rotationSpeed);
    }
}
