using UnityEngine;

public class PlayerWallDetector : MonoBehaviour
{
    [SerializeField] private PlayerController playerController = null;
    [SerializeField] private Vector3 baseSize = new Vector3(1.3f, 1.5f, 1.3f);

    private bool isColliding = false;
    private bool wallIsTilted = false;

    public Vector3 BaseSize
    {
        get => baseSize;
    }
    public bool IsColliding
    {
        get { return isColliding; }
    }
    public bool WallIsTilted
    {
        get { return wallIsTilted; }
    }

    // Awake is called once before all other methods, when the GameObject is created
    private void Awake()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        boxCollider.center = Vector3.zero;

        if (!boxCollider.enabled)
        {
            boxCollider.enabled = true;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if (!playerController) { playerController = transform.parent.GetComponent<PlayerController>(); }
    }

    // OnTriggerEnter is called once when a trigger collision started
    private void OnTriggerEnter(Collider _other)
    {
        if ((!_other.CompareTag("Weapon") &&
            !_other.GetComponent<SyringueOnFloor>()) &&
            _other.CompareTag("Untagged") || _other.CompareTag("Ground") &&
            !_other.CompareTag("Player"))
        {
            isColliding = true;
            wallIsTilted =
                (_other.transform.rotation.eulerAngles.x != 0f ||
                 _other.transform.rotation.eulerAngles.z != 0f);
        }
    }

    // OnTriggerExit is called once when a trigger collision ended
    private void OnTriggerExit(Collider _other)
    {
        if ((!_other.CompareTag("Weapon") &&
            !_other.GetComponent<SyringueOnFloor>()) &&
            _other.CompareTag("Untagged") || _other.CompareTag("Ground") &&
            !_other.CompareTag("Player"))
        {
            isColliding = false;
            wallIsTilted = false;
        }
    }
}
