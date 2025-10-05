using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    [SerializeField] bool isOpen = false;
    [SerializeField] bool openRight = false;
    [SerializeField] bool openLeft = false;
    [SerializeField] bool openDown = false;
    [SerializeField] bool openUp = false;
    [SerializeField] float slideDistance = 1f;
    [SerializeField] float slideDuration = 1f; // how long the door takes to open

    private Vector3 startPosition;
    private Vector3 targetPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    public void OpenDoorAnimation()
    {
        if (!isOpen)
        {
            isOpen = true;

            Vector3 direction = Vector3.zero;
            if (openRight) direction = Vector3.right;
            else if (openLeft) direction = Vector3.left;
            else if (openUp) direction = Vector3.up;
            else if (openDown) direction = Vector3.down;

            targetPosition = startPosition + direction * slideDistance;

            StartCoroutine(SlideDoor());
        }
    }

    public void CloseDoorAnimation()
    {
        if (isOpen)
        {
            isOpen = false;

            targetPosition = startPosition;

            StartCoroutine(SlideDoor());
        }
    }

    private System.Collections.IEnumerator SlideDoor()
    {
        float elapsed = 0f;
        Vector3 initialPos = transform.position;
        while (elapsed < slideDuration)
        {
            float t = elapsed / slideDuration;
            // Apply ease-in/ease-out smoothing
            t = t * t * (3f - 2f * t);
            transform.position = Vector3.Lerp(initialPos, targetPosition, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition; // Snap to final position
    }

}
