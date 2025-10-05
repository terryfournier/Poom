using UnityEngine;

public class WayPointFloattingText : MonoBehaviour
{
    Transform mainCamTransform;
    Transform wayPointTransform;
    Transform canvaWorldSpace;

    public Vector3 offset;

    private void Start()
    {
        wayPointTransform = transform;
        mainCamTransform = Camera.main.transform;
        canvaWorldSpace = GetComponentInChildren<Canvas>().transform;
    }

    private void Update()
    {
        // look at the camera
        // transform.rotation = Quaternion.LookRotation(transform.position - mainCamTransform.transform.position);
        transform.rotation = Camera.main.transform.rotation;

        //if(transform.eulerAngles.x >= 15)
        //{
        //    transform.rotation = Quaternion.Euler(15, transform.rotation.y, transform.rotation.z);
        //}

        // transform.position = wayPointTransform.position + offset;
    }
}
