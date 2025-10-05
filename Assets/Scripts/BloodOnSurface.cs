using Unity.VisualScripting;
using UnityEngine;

public class BloodOnSurface : MonoBehaviour
{
    [SerializeField] private GameObject bloodBuffer;
    [SerializeField] private BufferHandler bufferHandler;

    private void Start()
    {
        this.AddComponent<Rigidbody>();
        bufferHandler = GameObject.Find("UniversalBuffer").GetComponent<BufferHandler>();
        bloodBuffer = bufferHandler.transform.Find("BloodBuffer").gameObject;

        //raycast on ground below
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, ~LayerMask.GetMask("Enemy", "Player")))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                // Use the actual point and normal of the hit
                Vector3 contactPoint = hit.point;
                Vector3 contactNormal = hit.normal;

                bufferHandler.AddBloodParticle(contactPoint, contactNormal);
            }
        }

    }

}
