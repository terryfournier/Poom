using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundDetector : MonoBehaviour
{
    #region Constants
    private const float SLOPE_EPSILON = 5f;
    #endregion

    #region Members
    #region [SerializeField]
    [SerializeField] private CapsuleCollider playerFirstCapsuleCollider = null;
    [SerializeField] private PlayerController playerController = null;
    [SerializeField] private Vector3 baseSize = new Vector3(0.7f, 0.5f, 0.7f);
    [SerializeField] private float maxStepDistance = 0f;
    [SerializeField] private float stepRayCount = 0f;
    #endregion

    #region private
    private Transform groundTransform = null;
    private List<Collider> grounds = new List<Collider>();
    private PlayerSoundManager playerSoundManager = null;
    private Ray groundRay = new Ray();
    private Ray stairRayHigh = new Ray();
    private Ray stairRayLow = new Ray();
    private Vector3 rayPos = Vector3.zero;
    private Vector3 groundNormal = Vector3.zero;
    private Vector3 groundPos = Vector3.zero;
    private float rayLength = 0.05f;
    private float dotPlayerGround = 0f;
    private float tankAttackAdaptationTimer = 0f;
    private bool isColliding = false;
    private bool isOnSlope = false;
    private bool adaptToSlope = false;
    private bool isOnMovingGround = false;
    private bool justFetchedAudioManager = false;
    #endregion

    #region public
    [HideInInspector] public bool TankAttackAdaptationCopy = false;
    #endregion

    #region Properties
    public Vector3 BaseSize
    {
        get => baseSize;
    }
    public float DotPlayerGround
    {
        get => dotPlayerGround;
    }
    public bool IsColliding
    {
        get => isColliding;
    }
    public bool IsOnSlope
    {
        get => isOnSlope;
    }
    public bool IsOnMovingGround
    {
        get => isOnMovingGround;
    }
    #endregion
    #endregion

    #region Methods
    #region Unity
    // Awake is called once before all other methods, when the GameObject is created
    private void Awake()
    {
        playerSoundManager = GetComponent<PlayerSoundManager>();
        rayPos = 0.975f * Vector3.down + base.transform.position;
        groundRay = new Ray(rayPos, Vector3.down);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if (!playerController)
        {
            playerController = base.transform.parent.GetComponent<PlayerController>();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (TankAttackAdaptationCopy)
        {
            isColliding = false;
            tankAttackAdaptationTimer += Time.deltaTime;
            
            if (tankAttackAdaptationTimer > 0.083333333f)
            {
                TankAttackAdaptationCopy = false;
            }
        }

        if (!isOnMovingGround)
        {
            // Make it so that Player doesn't get pushed by moving ground when not on moving ground
            AdjustPlayerVelocity();
        }
    }

    // OnTriggerEnter is called once when a trigger collision started
    private void OnTriggerEnter(Collider _other)
    {
        if ((!playerController.IsAdaptingToTank) &&
            !_other.CompareTag("Weapon") && !_other.GetComponent<SyringueOnFloor>() &&
            !_other.CompareTag("Player"))
        {
            if (_other.GetComponent<ConveyorBelt>())
            {
                isOnMovingGround = true;
            }

            BouncePlayerOnEnemies(_other);

            playerController.TellSoundManagerToReset();
            playerController.IsGrounded = true;
        }
    }

    // OnTriggerStay is called every frame when the game object is colliding with a trigger collision
    private void OnTriggerStay(Collider _other)
    {
        if ((!playerController.IsAdaptingToTank) &&
            !_other.CompareTag("Weapon") && !_other.GetComponent<SyringueOnFloor>() &&
            !_other.CompareTag("Player"))
        {
            RaycastHit raycastHit = new RaycastHit();

            if (Physics.Raycast(groundRay, out raycastHit, rayLength, ~LayerMask.GetMask("Player")))
            {
                groundPos = raycastHit.point;
            }

            Vector3 position = playerController.transform.position;

            dotPlayerGround = Vector3.Dot(position, groundPos);
            groundTransform = _other.transform;
            playerController.IsGrounded = true;
            isColliding = true;

            Vector3 eulerAngles = groundTransform.rotation.eulerAngles;

            isOnSlope =
                (Helper.IsAtAllBut(eulerAngles.x, 5f, 180f, 1f, 10f) ||
                Helper.IsAtAllBut(eulerAngles.z, 5f, 180f, 1f, 10f));

            float y = playerController.Rb.linearVelocity.y;

            if (isOnSlope && y > 0f && !playerController.IsSliding && !_other.CompareTag("Enemy"))
            {
                AdaptPlayerToSlope(_other);
            }
        }
    }

    // OnTriggerExit is called once when a trigger collision ended
    private void OnTriggerExit(Collider _other)
    {
        if (!playerController.IsAdaptingToTank)
        {
            if (_other.GetComponent<ConveyorBelt>())
            {
                isOnMovingGround = false;
            }

            playerController.IsGrounded = false;
            groundTransform = null;
            isColliding = false;
            isOnSlope = false;
        }
    }
    #endregion

    #region Custom
    #region private
    private void BouncePlayerOnEnemies(Collider _other)
    {
        MeshFilter component = _other.GetComponent<MeshFilter>();

        if (component && !_other.CompareTag("Enemy"))
        {
            Mesh mesh = component.mesh;

            if (mesh && mesh.isReadable)
            {
                groundNormal = Helper.GetNormalOfMesh(mesh);
            }
        }

        if (_other.CompareTag("Enemy"))
        {
            PhysicsMaterial physicsMaterial = new PhysicsMaterial("Bounce");
            physicsMaterial.staticFriction = 0f;
            physicsMaterial.dynamicFriction = 0f;
            physicsMaterial.bounciness = 0.33f;
            physicsMaterial.frictionCombine = PhysicsMaterialCombine.Minimum;
            physicsMaterial.bounceCombine = PhysicsMaterialCombine.Maximum;

            playerFirstCapsuleCollider.material = physicsMaterial;
            return;
        }

        if (playerFirstCapsuleCollider.material)
        {
            playerFirstCapsuleCollider.material = null;
        }
    }

    private void AdaptPlayerToSlope(Collider _other)
    {
        Vector3 eulerAngles = _other.transform.rotation.eulerAngles;
        Rigidbody rb = playerController.Rb;
        Vector3 vector = rb.linearVelocity;
        vector = (vector - groundPos) * Vector3.Dot(vector, groundPos);

        float num = 1f;
        bool flag = true;

        if (Helper.EpsilonTestIn(vector.y, num, flag))
        {
            rb.linearVelocity = vector;
        }
    }

    private void AdjustPlayerVelocity()
    {
        if (playerController.IsGrounded)
        {
            if (Helper.Greater(playerController.VelocityAdd, Vector3.zero))
            {
                playerController.VelocityAdd -= playerController.VelocityAdd * 8f * Time.deltaTime;
                return;
            }

            if (Helper.Less(playerController.VelocityAdd, Vector3.zero))
            {
                playerController.VelocityAdd = Vector3.zero;
                return;
            }

            return;
        }

        if (Helper.Greater(playerController.VelocityAdd, Vector3.zero))
        {
            playerController.VelocityAdd -= playerController.VelocityAdd * 2f * Time.deltaTime;
            return;
        }

        if (Helper.Less(playerController.VelocityAdd, Vector3.zero))
        {
            playerController.VelocityAdd = Vector3.zero;
            return;
        }
    }
    #endregion
    #endregion
    #endregion
}
