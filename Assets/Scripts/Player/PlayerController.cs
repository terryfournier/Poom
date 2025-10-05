using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class PlayerController : MonoBehaviour
{
    #region Constants
    private const float BASE_SPEED = 1f;
    private const float SPRINT_SPEED = 1.5f;
    private const float SLIDE_SPEED = 1.75f;
    private const float SLIDE_COOLDOWN = 0.5f;
    private const float DASH_SPEED = 3f;
    private const float DASH_DURATION = 0.25f;
    private const float DASH_COOLDOWN = 1.25f;
    private const float SLIDE_DURATION = 0.4f;
    private const float SLIDE_ROTATION_SPEED = 180f;
    private const float SLIDE_ANGLE = 5f;
    private const float SLIDE_EPSILON = 5f;
    private const float VELOCITY_EPSILON = 0.025f;
    private const float COYOTE_WALK = 0.15f;
    private const float COYOTE_SPRINT = 0.2f;
    #endregion

    #region Variables
    #region Actions and Delegates
    public event Action OnDestroyPlayer;
    #endregion

    #region Members
    #region static
    #region public
    public static bool StopJump = false;
    #endregion
    #endregion

    #region Normal
    #region [SerializedField]
    [SerializeField] private GameObject flashlightObj = null;
    [SerializeField] private CapsuleCollider capsuleColliderFriction = null;
    [SerializeField] private CapsuleCollider capsuleColliderNoFriction = null;
    [SerializeField] private VisualEffect jumpVisualEffect;
    [SerializeField] private PlayerGroundDetector groundDetector = null;
    [SerializeField] private PlayerWallDetector wallDetector = null;
    [SerializeField] private PlayerView playerView = null;
    [SerializeField] private Vector3 maxVelocity = new Vector3(30f, 30f, 30f);
    [SerializeField] private float rotationSpeed = 120f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float speedFactor = 1f;
    [SerializeField] private float outOfBreathStartOffset = 2f;
    // [SerializeField] private VFXEventAttribute vfxEventJump;
    #endregion

    #region private
    private Rigidbody rb = null;
    private Transform utilityManager = null;
    private PlayerInventory playerInventory = null;
    private PlayerSoundManager playerSoundManager = null;
    private ControlsManager keysManager = null;
    private MouseManager mouseManager = null;
    private PauseManager pauseManager = null;
    private Quaternion startingRotation = Quaternion.identity;
    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 linearVelocity = Vector3.zero;
    private Vector3 lastTraveledDistance = Vector3.zero;
    private Vector3 staticPos = Vector3.zero;
    private Vector3 startingPos = Vector3.zero;
    private Vector3 keyboardAxis = Vector3.zero;
    private string activeSceneName = "";
    private float horizontalViewAxis = 0f;
    private float rawAxisX = 0f;
    private float rawAxisY = 0f;
    private float sprintFactor = 0f;
    private float slideTimer = 0f;
    private float gravityAugment = 0f;
    private float jumpTimer = 0f;
    private float dashTimer = 0f;
    private float slideCooldownTimer = 0f;
    private float dashCooldownTimer = 0f;
    private float outOfBreathTimer = 0f;
    private bool isSprinting = false;
    private bool isSliding = false;
    private bool canSlide = false;
    private bool hasPressedSlide = false;
    private bool stopFlag = false;
    private bool slideEndFlag = false;
    private bool isMoving = false;
    private bool hasRetrievedManagers = false;
    private bool hasRetrievedMainManager = false;
    private bool camSlideBool1 = false;
    private bool camSlideBool2 = false;
    private bool isInJumpWindow = true;
    private bool isInDash = false;
    private bool hasPressedJump = false;
    private bool canPlay = false;
    private bool justRetrievedManagers = false;
    private bool isLookingAtEnemyOrDestructible = false;
    private bool hasToAdaptAimSpeed = false;
    private bool outOfBreathSoundIsPlaying = false;

    //For Hugo and the camera recoil
    private RecoilCameraController cameraRecoil;


    #endregion
    #endregion

    #region public
    #region [HideInInspector]
    [HideInInspector] public bool HasToggledSprint = false;
    [HideInInspector] public bool CanChangeView = false;
    [HideInInspector] public bool IsGrounded = false;
    [HideInInspector] public bool IsAdaptingToTank = false;
    #endregion

    #region Normal
    public Vector3 VelocityAdd = Vector3.zero;
    public float AimFactorWhenLookingAtEnemy = 0.5f;
    private float yaw;
    private float pitch;
    #endregion
    #endregion
    #endregion

    #region Properties
    public Rigidbody Rb
    {
        get => rb;
    }
    public Transform UtilityManager
    {
        get => utilityManager;
    }
    public Quaternion Rotation
    {
        get => Quaternion.Euler(rotation);
    }
    public Vector3 LinearVelocity
    {
        get => rb.linearVelocity;
    }
    public Vector3 MaxVelocity
    {
        get => maxVelocity;
    }
    public Vector3 LastTraveledDistance
    {
        get => lastTraveledDistance;
    }
    public Vector3 EulerRotation
    {
        get => rotation;
    }
    public float AbsSpeedXValue
    {
        get => Mathf.Abs(Helper.GetAxis_Gamepad(true, true));
    }
    public float AbsSpeedZValue
    {
        get => Mathf.Abs(Helper.GetAxis_Gamepad(false, true));
    }
    public float AbsTotalSpeedValue
    {
        get => 0.5f + 0.5f * (AbsSpeedXValue + AbsSpeedZValue);
    }
    public float RotationSpeed
    {
        get => rotationSpeed * mouseManager.AimFactor;
    }
    public bool IsSprinting
    {
        get { return isSprinting; }
    }
    public bool IsMoving
    {
        get { return isMoving; }
    }
    public bool IsSliding
    {
        get { return isSliding; }
    }
    public bool IsInDash
    {
        get { return isInDash; }
    }
    public bool HasPressedJump
    {
        get => hasPressedJump;
    }
    public bool IsLookingAtEnemy
    {
        get => isLookingAtEnemyOrDestructible;
    }
    public bool HasToAdaptAimSpeed
    {
        get => hasToAdaptAimSpeed;
    }
    #endregion
    #endregion

    #region Methods
    #region Unity Methods
    // Awake is called once before all other methods, when the GameObject is created
    private void Awake()
    {
        SceneManager.activeSceneChanged += GetActiveSceneName;

        if (!Helper.PrefabExists("Utility Managers"))
        {
            utilityManager = Instantiate(Resources.Load<UtilityManagers>("Utility Managers")).transform;
            hasRetrievedMainManager = true;
        }

        StartCoroutine(RetrieveManagers());

        rb = GetComponent<Rigidbody>();
        playerInventory = GetComponent<PlayerInventory>();
        playerSoundManager = GetComponent<PlayerSoundManager>();
        cameraRecoil = Camera.main.GetComponent<RecoilCameraController>();

        startingPos = transform.position;
        startingRotation = transform.rotation;

        isSprinting = AlwaysRunToggle.IsOn;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        jumpVisualEffect.Stop();

        if (!playerView)
        {
            playerView = Camera.main.GetComponent<PlayerView>();
        }
    }

    // FixedUpdate is called at fixed intervals, independently from framerate
    private void FixedUpdate()
    {
        if (canPlay)
        {
            Vector3 linearVelCpy = linearVelocity;

            // When not in pause
            linearVelocity = linearVelCpy;

            isMoving = (Helper.EpsilonTestOut(velocity.x, VELOCITY_EPSILON) || Helper.EpsilonTestOut(velocity.z, VELOCITY_EPSILON));
            canSlide = (slideCooldownTimer <= 0f) &&
                ((IsAdaptingToTank)
                ? false
                : (!isSliding && isMoving && groundDetector.IsColliding));

            AdaptFriction();

            // Move
            if (!isSliding)
            {
                HandleMovement();
            }

            // Prepare for slide
            // Begin slide
            if (canSlide && hasPressedSlide && velocity.z >= 0f)
            {
                AdjustSlide(true);
            }

            // Revert to a not slide state
            if (stopFlag)
            {
                AdjustSlide(false);
            }

            // End slide
            if (slideEndFlag)
            {
                EndSlide();
            }

            ClampVelocity();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        canPlay =
            (hasRetrievedManagers &&
            (!pauseManager.isGamePaused || activeSceneName != "GameOver"));

        if (justRetrievedManagers)
        {
            StopCoroutine(RetrieveManagers());
            justRetrievedManagers = false;
        }

        // Make it so it doesn't retrieve current scene's name every frame
        if (activeSceneName != SceneManager.GetActiveScene().name)
        {
            hasRetrievedManagers = false;
        }

        // When not in game over and game is not paused
        if (canPlay)
        {
            CanChangeView = groundDetector.IsColliding;
            IsGrounded = groundDetector.IsColliding;

            if (StopJump && ControlsManager.HasReleased_Gamepad(GamepadActions.JUMP))
            {
                StopJump = false;
            }

            if (LinearVelocity.y > 0f &&
                (ControlsManager.HasPressed_Keyboard(KeyboardActions.JUMP, false) ||
                 ControlsManager.HasPressed_Gamepad(GamepadActions.JUMP, false)))
            {
                IsAdaptingToTank = true;
            }

            ManageActions();
            if (isMoving) { ManageOutOfBreathSound(); }
            AdaptHitboxes();

            if (!isInDash && isSliding) { HandleSlide(); }
            else { GetAxes(); }

            if (isInDash)
            {
                float previousSpeed = 0f;

                // Get previous speed to reset after dash
                if (sprintFactor != DASH_SPEED)
                {
                    previousSpeed = (isSprinting) ? SPRINT_SPEED : BASE_SPEED;
                    sprintFactor = DASH_SPEED;
                }

                dashTimer += Time.deltaTime;

                // Correct speed if necessary
                if (sprintFactor > previousSpeed)
                {
                    sprintFactor -= dashTimer;
                }

                // End dash
                // Reset speed to the one before dash
                if (dashTimer > DASH_DURATION)
                {
                    sprintFactor = previousSpeed;
                    dashTimer = 0f;
                    isInDash = false;
                }
            }

            // Manage Slide timer
            if (slideCooldownTimer > 0f)
            {
                slideCooldownTimer -= Time.deltaTime;
            }

            // Manage Dash timer
            if (!isInDash && dashCooldownTimer > 0f)
            {
                dashCooldownTimer -= Time.deltaTime;
            }

            CalculateDistances();

#if UNITY_EDITOR
            if (Helper.KeyboardUsable)
            {
                // Reset player
                if (Helper.CurrentKeyboard.deleteKey.wasReleasedThisFrame)
                {
                    transform.position = startingPos;
                    transform.rotation = startingRotation;
                    rb.linearVelocity = Vector3.zero;
                    rotationSpeed = 0f;

                    if (isSliding)
                    {
                        transform.Translate(0.1f * Vector3.up);
                        isSliding = false;
                    }
                }
            }
#endif
        }
    }

    // OnCollisionEnter is called once when a collision started
    private void OnCollisionEnter(Collision _collision)
    {
        if (!_collision.gameObject.CompareTag("Weapon") && !_collision.gameObject.GetComponent<SyringueOnFloor>() &&
            !_collision.gameObject.CompareTag("Player"))
        {
            IsAdaptingToTank = false;
        }
    }

    private void OnDestroy()
    {
        OnDestroyPlayer?.Invoke();
    }
    #endregion

    #region Custom Methods
    #region Directly related to Player
    private void HandleMovement()
    {
        // Determines speed based on sprint if on ground
        if (!isInDash && groundDetector.IsColliding)
        {
            sprintFactor = (isSprinting) ? SPRINT_SPEED : BASE_SPEED;
            jumpVisualEffect.Stop();
        }

        linearVelocity = rb.linearVelocity;

        MoveHorizontally();
        HandleJump();
        HandleRotation();

        // Makes player fall faster when not on ground => simulate gravity
        gravityAugment = (groundDetector.IsColliding) ? 0f : gravityAugment + 0.125f * Time.fixedDeltaTime;

        Vector3 fullLinearVel = (rb.rotation * linearVelocity * sprintFactor) * Time.fixedDeltaTime;
        fullLinearVel += VelocityAdd * Time.deltaTime;
        fullLinearVel.y = linearVelocity.y - gravityAugment;

        rb.linearVelocity = fullLinearVel;
    }

    private void MoveHorizontally()
    {
        float speedValue = 400f * speedFactor;

        Material speedLinesMat = playerView.SpeedLinesMat;
        float centerOffsetX = speedLinesMat.GetFloat("_CenterOffsetX");

        // When using Gamepad
        if (Helper.GamepadUsable)
        {
            float xAxis = Helper.GetAxis_Gamepad(true, !LeftHandModeToggle.IsOn);
            float yAxis = Helper.GetAxis_Gamepad(false, !LeftHandModeToggle.IsOn);

            // Move Left/Right
            if (Helper.EpsilonTestOut(velocity.x, VELOCITY_EPSILON))
            {
                linearVelocity.x += speedValue * xAxis;
            }

            // Move Forward/Back
            if (Helper.EpsilonTestOut(velocity.z, VELOCITY_EPSILON))
            {
                linearVelocity.z += speedValue * yAxis;
            }

            speedLinesMat = playerView.SpeedLinesMat;
            centerOffsetX = speedLinesMat.GetFloat("_CenterOffsetX");
            float absXAxis = Mathf.Abs(xAxis);

            if (xAxis < -0.25f)
            {
                if (centerOffsetX > 0.33f) { centerOffsetX -= absXAxis * 1.5f * Time.deltaTime; }
            }
            else if (xAxis < 0.25f)
            {
                if (!Mathf.Approximately(centerOffsetX, 0.5f)) { centerOffsetX += (0.5f - centerOffsetX) * absXAxis * Time.deltaTime; }
            }
            else
            {
                if (centerOffsetX < 0.66f) { centerOffsetX += absXAxis * 1.5f * Time.deltaTime; }
            }

            Mathf.Clamp(centerOffsetX, 0.33f, 0.66f);

            speedLinesMat.SetFloat("_CenterOffsetX", centerOffsetX);

            return;
        }

        // When using Keyboard
        // Move Left/Right
        if (velocity.x > 0f)
        {
            linearVelocity.x += speedValue;
        }
        if (velocity.x < 0f)
        {
            linearVelocity.x -= speedValue;
        }

        // Move Forward/Back
        if (velocity.z > 0f)
        {
            linearVelocity.z += speedValue;
        }
        if (velocity.z < 0f)
        {
            linearVelocity.z -= speedValue;
        }

        // Adjust speed when moving Diagonally
        // Keep speed constant
        if (velocity.x != 0f && velocity.z != 0f)
        {
            linearVelocity.x *= 0.71f;
            linearVelocity.z *= 0.71f;
        }

        speedLinesMat = playerView.SpeedLinesMat;
        centerOffsetX = speedLinesMat.GetFloat("_CenterOffsetX");

        // When moving Left
        if (velocity.x < -0.125f)
        {
            if (centerOffsetX > 0.33f) { centerOffsetX -= 2f * Time.deltaTime; }
        }
        // When not moving or moving Forward/Backward
        else if (velocity.x < 0.125f)
        {
            if (!Mathf.Approximately(centerOffsetX, 0.5f)) { centerOffsetX += (0.5f - centerOffsetX) * 1.5f * Time.deltaTime; }
        }
        // When moving Right
        else
        {
            if (centerOffsetX < 0.66f) { centerOffsetX += 2f * Time.deltaTime; }
        }

        Mathf.Clamp(centerOffsetX, 0.33f, 0.66f);

        speedLinesMat.SetFloat("_CenterOffsetX", centerOffsetX);
    }

    private void HandleJump()
    {
        if (!StopJump && !IsAdaptingToTank)
        {
            // Manage coyote time
            if (!groundDetector.IsColliding)
            {
                jumpTimer += Time.deltaTime;

                if (jumpTimer > ((isSprinting) ? COYOTE_SPRINT : COYOTE_WALK))
                {
                    isInJumpWindow = false;
                }
            }
            else if (!isInJumpWindow)
            {
                jumpTimer = 0f;
                hasPressedJump = false;
                isInJumpWindow = true;
            }

            // Manage jump
            if (groundDetector.IsColliding || isInJumpWindow)
            {
                // When playing with Keyboard
                // Detect only Keyboard input
                if (!Helper.GamepadUsable && Helper.KeyboardUsable &&
                    ControlsManager.HasPressed_Keyboard(KeyboardActions.JUMP, true))
                {
                    Jump();
                    return;
                }

                // When playing with Gamepad
                // Detect only Gamepad input
                if (Helper.GamepadUsable &&
                    ControlsManager.HasPressed_Gamepad(GamepadActions.JUMP, true))
                {
                    Jump();
                }
            }
        }
    }

    private void Jump()
    {
        // Stop corresponding sound between Walk and Sprint
        // Depending on if we were Walking or Sprinting before
        if (IsMoving)
        {
            if (IsSprinting) { playerSoundManager.StopSound(PlayerSound.SPRINT); }
            else { playerSoundManager.StopSound(PlayerSound.WALK); }
        }

        // Play Jump sound
        playerSoundManager.RestartSound(PlayerSound.JUMP);

        // Apply Jump
        linearVelocity.y = jumpForce;
        jumpVisualEffect.Play();
        hasPressedJump = true;

        // Avoid getting snapped back to ground
        AdaptForTankAttack();
    }

    private void HandleRotation()
    {
        TestIfHasToAdaptAimSpeed();

        float aimSpeed = rotationSpeed;

        if (hasToAdaptAimSpeed)
        {
            FacilitateAiming(out aimSpeed);
        }

        // Rotate Left/Right
        rotation = rb.rotation.eulerAngles + horizontalViewAxis * aimSpeed * mouseManager.AimFactor * Time.fixedDeltaTime * Vector3.up;
        transform.rotation = Quaternion.Euler(rotation);
    }

    private void ToggleSprint()
    {
        isSprinting = !isSprinting;
        HasToggledSprint = true;
    }

    /// <summary>
    /// Adjust timer, velocity and flags for sliding
    /// </summary>
    /// <param name="_isAtBeginning">Used to determine if slide starts or ends after calling this function</param>
    private void AdjustSlide(in bool _isAtBeginning)
    {
        hasPressedSlide = false;
        slideCooldownTimer = SLIDE_COOLDOWN;

        if (_isAtBeginning)
        {
            if (isSprinting) { playerSoundManager.StopSound(PlayerSound.SPRINT); }
            else { playerSoundManager.StopSound(PlayerSound.WALK); }

            playerSoundManager.StartSound(PlayerSound.SLIDE);
            rb.linearVelocity *= SLIDE_SPEED;
            slideTimer = 0f;
            isSliding = true;

            return;
        }

        rb.linearVelocity /= SLIDE_SPEED;
        velocity.y = 0f;
        isSliding = false;

        stopFlag = false;
        slideEndFlag = true;
    }

    private void HandleSlide()
    {
        if (slideTimer < SLIDE_DURATION)
        {
            if (!camSlideBool1)
            {
                Camera.main.transform.position -= Vector3.up * 0.5f;

                camSlideBool2 = false;
                camSlideBool1 = true;
            }

            float xAngle = transform.rotation.eulerAngles.x;

            if (xAngle <= SLIDE_EPSILON || xAngle >= 360f - SLIDE_ANGLE + SLIDE_EPSILON)
            {
                transform.Rotate(SLIDE_ROTATION_SPEED * Time.deltaTime * Vector3.left);
            }

            slideTimer += Time.deltaTime;
            return;
        }

        stopFlag = true;
    }

    private void EndSlide()
    {
        transform.Rotate(SLIDE_ROTATION_SPEED * Time.fixedDeltaTime * Vector3.right);

        float xAngle = transform.rotation.eulerAngles.x;

        if (xAngle > 360f - SLIDE_EPSILON || xAngle < SLIDE_EPSILON)
        {
            if (!camSlideBool2)
            {
                Camera.main.transform.position += Vector3.up * 0.5f;

                camSlideBool1 = false;
                camSlideBool2 = true;
            }

            Vector3 rotation = transform.rotation.eulerAngles;
            rotation.x = 0f;
            transform.rotation = Quaternion.Euler(rotation);

            slideEndFlag = false;
        }
    }

    private void ManageActions()
    {
        if (keysManager)
        {
            // When playing with Keyboard and Mouse
            if (!Helper.GamepadUsable)
            {
                // Keyboard actions
                if (Helper.KeyboardUsable)
                {
                    // Manage sprint
                    if (!isSliding)
                    {
                        if (ControlsManager.HasPressed_Keyboard(KeyboardActions.SPRINT, false))
                        {
                            if (AlwaysRunToggle.IsOn)
                            {
                                isSprinting = true;
                            }
                            else
                            {
                                ToggleSprint();
                            }

                            playerSoundManager.CanSwitchSound = true;
                        }
                    }

                    // Start slide
                    if (!isInDash &&
                        ControlsManager.HasPressed_Keyboard(KeyboardActions.SLIDE, false))
                    {
                        hasPressedSlide = true;
                    }

                    // Dash
                    if (isMoving && dashCooldownTimer <= 0f &&
                        !isSliding && ControlsManager.HasPressed_Keyboard(KeyboardActions.DASH, false))
                    {
                        playerSoundManager.RestartSound(PlayerSound.DASH);
                        dashCooldownTimer = DASH_COOLDOWN;
                        isInDash = true;
                    }

                    //Reload Ammo
                    if (ControlsManager.HasPressed_Keyboard(KeyboardActions.RELOAD, true))
                    {
                        GameObject heldWeaponObj = playerInventory.GetWeaponHeld();

                        if (heldWeaponObj)
                        {
                            heldWeaponObj.GetComponent<Weapon>().Reload();
                        }
                    }
                }

                // Mouse actions
                if (Helper.MouseUsable)
                {
                    // Toggle flashlight
                    if (ControlsManager.HasPressed_Mouse(MouseActions.TOGGLE_FLASHLIGHT, false))
                    {
                        playerSoundManager.RestartSound(PlayerSound.TORCH);
                        flashlightObj.SetActive(!flashlightObj.activeSelf);
                    }

                    // Shoot
                    if (ControlsManager.HasPressed_Mouse(MouseActions.SHOOT, true))
                    {
                        playerInventory.Shoot();
                    }
                }

                return;
            }

            // When playing with Gamepad
            // Toggle flashlight
            if (ControlsManager.HasPressed_Gamepad(GamepadActions.TOGGLE_FLASHLIGHT, false))
            {
                playerSoundManager.RestartSound(PlayerSound.TORCH);
                flashlightObj.SetActive(!flashlightObj.activeSelf);
            }

            // Manage sprint
            if (!isSliding)
            {
                if (ControlsManager.HasPressed_Gamepad(GamepadActions.SPRINT, false))
                {
                    if (AlwaysRunToggle.IsOn)
                    {
                        isSprinting = true;
                    }
                    else
                    {
                        ToggleSprint();
                    }

                    playerSoundManager.CanSwitchSound = true;
                }
            }

            // Start slide
            if (!isInDash &&
                ControlsManager.HasPressed_Gamepad(GamepadActions.SLIDE, false))
            {
                hasPressedSlide = true;
            }

            // Dash
            if (isMoving && dashCooldownTimer <= 0f &&
                !isSliding && ControlsManager.HasPressed_Gamepad(GamepadActions.DASH, false))
            {
                playerSoundManager.RestartSound(PlayerSound.DASH);
                dashCooldownTimer = DASH_COOLDOWN;
                isInDash = true;
            }

            // Shoot
            if (ControlsManager.HasPressed_Gamepad(GamepadActions.SHOOT, true))
            {
                playerInventory.Shoot();
            }

            //Reload Ammo
            if (ControlsManager.HasPressed_Gamepad(GamepadActions.RELOAD, true))
            {
                GameObject heldWeapon = playerInventory.GetWeaponHeld();

                if (heldWeapon)
                {
                    heldWeapon.GetComponent<Weapon>().Reload();
                }

            }
        }
    }

    private void AdaptHitboxes()
    {
        // Makes it easier for the player to go up without getting stuck when between multiple platforms
        wallDetector.GetComponent<BoxCollider>().size = wallDetector.BaseSize /
            ((groundDetector.IsColliding)
            ? 1f
            : 1.444444444f);

        // Avoids a fast jump succession so that it doesn't look like jump is sometimes infinite and inconsistent
        groundDetector.GetComponent<BoxCollider>().size = groundDetector.BaseSize /
            ((groundDetector.IsColliding)
            ? 1f
            : 1.4f);
    }

    private void AdaptFriction()
    {
        // No friction
        if (groundDetector.IsOnSlope || wallDetector.IsColliding)
        {
            capsuleColliderFriction.enabled = false;
            capsuleColliderNoFriction.enabled = true;

            return;
        }

        // Base friction
        capsuleColliderFriction.enabled = true;
        capsuleColliderNoFriction.enabled = false;
    }

    private void ClampVelocity()
    {
        Vector3 linearVel = rb.linearVelocity;

        if (!Helper.Approximately(linearVel, maxVelocity))
        {
            Helper.Clamp(ref linearVel, -maxVelocity, maxVelocity);

            rb.linearVelocity = linearVel;
        }
    }

    /// <summary>
    /// <b>Idle : </b>0
    /// <br><b>Walk : </b>1</br>
    /// <br><b>Sprint : </b>2</br>
    /// <br><b>Slide : </b>3</br>
    /// <br><b>Dash : </b>4</br>
    /// </summary>
    /// <returns></returns>
    public float GetMovementState()
    {
        if (!isMoving) { return 0f; }
        else
        {
            if (isSprinting) { return 2f; }
            else if (isSliding) { return 3f; }
            else if (isInDash) { return 4f; }
            else { return 1f; }
        }
    }

    private void CalculateDistances()
    {
        // When moving
        if (isMoving)
        {
            lastTraveledDistance = Helper.Abs(transform.position - staticPos);
            return;
        }

        // When not moving
        if (lastTraveledDistance.x != 0f)
        {
            staticPos = transform.position;
            lastTraveledDistance = Vector3.zero;
        }
    }

    /// <summary>
    /// Tests if Player is holding a weapon that can shoot
    /// and is looking at en enemy
    /// </summary>
    private void TestIfHasToAdaptAimSpeed()
    {
        if (!Helper.GamepadUsable)
        {
            hasToAdaptAimSpeed = false;
            return;
        }

        float aimSpeed = mouseManager.AimFactor;
        GameObject heldWeapon = playerInventory.GetWeaponHeld();

        if (heldWeapon)
        {
            if (heldWeapon.GetComponent<Weapon>().w_type != Type.Mellee)
            {
                hasToAdaptAimSpeed = true;
            }

            return;
        }

        hasToAdaptAimSpeed = false;
    }

    /// <summary>
    /// Slows down Player view to facilitate aiming to enemies
    /// </summary>
    /// <param name="_aimSpeed"></param>
    private void FacilitateAiming(out float _aimSpeed)
    {
        const float DISTANCE = 250f;

        _aimSpeed = rotationSpeed;

        Vector3 origin = Camera.main.transform.position;
        Vector3 direction = Camera.main.transform.forward;

        isLookingAtEnemyOrDestructible =
            (!Physics.Raycast(origin, direction, DISTANCE, ~LayerMask.GetMask("Enemy") | ~LayerMask.GetMask("Destructible")) &&
              Physics.Raycast(origin, direction, DISTANCE, LayerMask.GetMask("Enemy") | LayerMask.GetMask("Destructible")));

        // When looking at en Enemy directly (i.e. with view not obstructed)
        if (isLookingAtEnemyOrDestructible)
        {
            _aimSpeed = rotationSpeed * AimFactorWhenLookingAtEnemy;
        }
    }

    private void ManageOutOfBreathSound()
    {
        PlayerSound outOfBreathPlayerSound = PlayerSound.OUT_OF_BREATH;

        if (outOfBreathSoundIsPlaying && !isSprinting)
        {
            outOfBreathTimer = 0f;
            outOfBreathSoundIsPlaying = false;
            return;
        }

        if (!outOfBreathSoundIsPlaying)
        {
            if (outOfBreathTimer < outOfBreathStartOffset + UnityEngine.Random.Range(-1f, 1f))
            {
                outOfBreathTimer += Time.deltaTime;
                return;
            }

            playerSoundManager.RestartSound(outOfBreathPlayerSound);
            outOfBreathSoundIsPlaying = true;
            return;
        }

        if (outOfBreathSoundIsPlaying && playerSoundManager.SoundEnded(outOfBreathPlayerSound))
        {
            playerSoundManager.RestartSound(outOfBreathPlayerSound);
        }
    }

    public void AdaptForTankAttack()
    {
        IsAdaptingToTank = true;
        isInJumpWindow = false;
        groundDetector.TankAttackAdaptationCopy = true;
    }

    public void ToggleIsSprinting()
    {
        Helper.ToggleBool(ref isSprinting);
    }

    public void TellSoundManagerToReset()
    {
        playerSoundManager.ResetChangeState();
    }
    #endregion

    #region Other
    private void GetAxes()
    {
        float horizontalSensitivity = mouseManager.Sensitivity.x;

        if (InvertViewToggle.IsOn_Horizontal) { horizontalSensitivity *= -1f; }

        // When playing with Gamepad
        if (Helper.GamepadUsable)
        {
            float stickXValue = Helper.GetAxis_Gamepad(true, !LeftHandModeToggle.IsOn);
            float stickYValue = Helper.GetAxis_Gamepad(false, !LeftHandModeToggle.IsOn);

            velocity.x = stickXValue;
            velocity.z = stickYValue;

            horizontalViewAxis = (Helper.GetAxis_Gamepad(true, LeftHandModeToggle.IsOn) * 25f) * horizontalSensitivity;

            return;
        }

        // When playing with Keyboard and Mouse
        string leftOrRightHanded = " (" + ((LeftHandModeToggle.IsOn) ? "Left" : "Right") + "-handed)";
        string horizontalAxis = "Horizontal" + leftOrRightHanded;
        string verticalAxis = "Vertical" + leftOrRightHanded;

        velocity.x = Helper.GetAxis_Keyboard(true);
        velocity.z = Helper.GetAxis_Keyboard(false);

        horizontalViewAxis = Helper.GetAxis_Mouse(true) * horizontalSensitivity;
    }

    private void GetActiveSceneName(Scene _current, Scene _next)
    {
        activeSceneName =
            (_current.name != _next.name)
            ? _next.name
            : _current.name;
    }
    #endregion
    #endregion

    #region Coroutines
    private IEnumerator RetrieveManagers()
    {
        while (!justRetrievedManagers)
        {
            // Retrieve main manager
            if (!hasRetrievedMainManager)
            {
                Helper.FetchUtilityManager(out UtilityManagers mainManager);
                utilityManager = mainManager.transform;
                hasRetrievedMainManager = true;
            }

            // Retrieve necessary managers
            if (!hasRetrievedManagers && utilityManager.childCount >= 2)
            {
                if (utilityManager.childCount < 3)
                {
                    Helper.FetchMouseManager(out mouseManager);
                    Helper.FetchControlsManager(out keysManager);
                }

                Helper.FetchPauseManager(out pauseManager);
                Helper.FetchMouseManager(out mouseManager);
                Helper.FetchControlsManager(out keysManager);

                hasRetrievedManagers = true;
                justRetrievedManagers = true;
            }

            yield return null;
        }
    }
    #endregion
    #endregion
}
