using UnityEngine;
using UnityEngine.Rendering;

public class PlayerView : MonoBehaviour
{
    #region Constants
    private const float TILT_MAX = 5f;
    private const float TILT_SPEED = 9f;
    private const float TILT_TIMER_MAX = 1f;
    #endregion

    #region Members
    #region [SerializedField]
    [SerializeField] private FullScreenPassRendererFeature speedLinesFeature = null;
    [SerializeField] private VolumeProfile volumeProfile = null;
    [SerializeField] private PlayerWallDetector wallDetector = null;
    [SerializeField] private float motionBlurValue = 0f;
    [SerializeField] private float walkFOV = 0f;
    [SerializeField] private float sprintFOV = 0f;
    [SerializeField] private float dashFOV = 0f;
    [SerializeField] private float shakeStrength = 0f;
    [SerializeField] private float shakeSpeed = 0f;
    [SerializeField] private float shakeDuration = 0f;
    #endregion

    #region private
    private Transform utilityManager = null;
    private PlayerController playerController = null;
    private PauseManager pauseManager = null;
    private MouseManager mouseManager = null;
    private ControlsManager keysManager = null;
    private FloatParameter motionBlurFloatParam = null;
    private Vector3 rotation = Vector3.zero;
    private Vector3 posBeforeShake = Vector3.zero;
    private Vector3 posAfterShake = Vector3.zero;
    private Vector3 shakeOffset = Vector3.zero;
    private float verticalViewAxis = 0f;
    private float rotationDone = 0.0f;
    private float baseFOV = 0f;
    private float alpha = 0f;
    private float bobOffsetX = 0f;
    private float bobSpeed = 0f;
    private float bobLimit = 0f;
    private float tiltTimer = 0f;
    private float shakeTimer = 0f;
    private bool hasRetrievedManagers = false;
    private bool bobUp = false;
    private bool isDynamicFovOn = false;
    private bool isSpeedLinesEffectOn = false;
    private bool isViewBobbingOn = false;
    private bool gamepadConnected = false;
    private bool isRepositioned = true;
    #endregion
    #endregion

    #region Properties
    public Material SpeedLinesMat
    {
        get => speedLinesFeature.passMaterial;
    }
    public Quaternion Rotation
    {
        get => transform.rotation;
    }
    public Vector3 EulerRotation
    {
        get => transform.rotation.eulerAngles;
    }
    #endregion

    #region Methods
    #region Unity
    // Awake is called once before all other methods, when the GameObject is created
    private void Awake()
    {
        posBeforeShake = transform.position;

        baseFOV = Camera.main.fieldOfView;
        bobUp = (Random.Range(0, 2) == 0);

        SpeedLinesMat.SetFloat("_CenterOffsetX", 0.5f);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        playerController = transform.GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    private void Update()
    {
        gamepadConnected = Helper.GamepadUsable;

        if (!utilityManager && playerController.UtilityManager)
        {
            utilityManager = playerController.UtilityManager.transform;
        }

        if (!hasRetrievedManagers &&
            utilityManager && utilityManager.childCount >= 2)
        {
            RetrieveManagers();

            hasRetrievedManagers = true;
        }

        if (hasRetrievedManagers)
        {
            motionBlurFloatParam = new FloatParameter((MotionBlurToggle.IsOn) ? motionBlurValue : 0f);

            isDynamicFovOn = DynamicFovToggle.IsOn;
            isSpeedLinesEffectOn = SpeedLinesToggle.IsOn;
            isViewBobbingOn = ViewBobbingToggle.IsOn;

            // Toggle motion blur
            volumeProfile.components[0].parameters[2].SetValue(motionBlurFloatParam);

            // When game is not paused
            if (!pauseManager.isGamePaused)
            {
                if (Helper.GamepadUsable) { verticalViewAxis = Helper.GetAxis_Gamepad(false, LeftHandModeToggle.IsOn) * 1.25f; }
                else if (Helper.MouseUsable) { verticalViewAxis = Helper.GetAxis_Mouse(false) * mouseManager.Sensitivity.y; }

                verticalViewAxis *= mouseManager.AimFactor;

                if (InvertViewToggle.IsOn_Vertical)
                {
                    verticalViewAxis *= -1f;
                }

                if (playerController.HasToAdaptAimSpeed)
                {
                    verticalViewAxis *= playerController.AimFactorWhenLookingAtEnemy;
                }

                Rotate();
                Clamp();

                if (!gamepadConnected)
                {
                    if (Helper.KeyboardUsable &&
                        ControlsManager.HasPressed_Keyboard(KeyboardActions.CENTER_VIEW, false))
                    {
                        CenterView();
                    }
                }
                else if (ControlsManager.HasPressed_Gamepad(GamepadActions.CENTER_VIEW, false))
                {
                    CenterView();
                }

                if (isSpeedLinesEffectOn)
                {
                    ManageSpeedLines();
                }

                if (isDynamicFovOn &&
                    !playerController.IsSliding && playerController.CanChangeView)
                {
                    AdjustFOV();
                    AdjustNearClipPlane();

                    playerController.CanChangeView = false;
                }

                // KEEP THIS
                // I am working on it
                //if (Helper.KeyboardConnected && Helper.CurrentKeyboard.tabKey.wasPressedThisFrame)
                //{
                //    shakeTimer = shakeDuration;
                //    posBeforeShake = transform.position;
                //    isRepositioned = false;
                //}

                //if (shakeTimer > 0f)
                //{
                //    Shake();

                //    shakeTimer -= Time.deltaTime;
                //}
                //else if (!isRepositioned)
                //{
                //    shakeOffset = posBeforeShake - transform.position;
                //    transform.position = posBeforeShake + playerController.LastTraveledDistance;
                //    isRepositioned = true;
                //}
            }
        }
    }
    #endregion

    #region Custom
    #region private
    private void Rotate()
    {
        const float ROTATION_SPEED = 90f;

        // Manage rotation
        rotation = ROTATION_SPEED * Time.deltaTime * verticalViewAxis * Vector3.left;

        //Tilt();

        // Manage view bobbing
        if (isViewBobbingOn &&
            playerController.IsMoving == true && playerController.IsGrounded == true &&
            playerController.IsSliding == false && wallDetector.IsColliding == false)
        {
            Bob();
        }

        // Apply total rotation
        rotationDone += rotation.x;
    }

    private void Clamp()
    {
        if (Helper.EpsilonTestIn(rotationDone, 45f, false))
        {
            transform.Rotate(rotation); // Rotate Up/Down

            return;
        }

        rotationDone -= rotation.x;
        posBeforeShake = transform.position;
    }

    private void AdjustFOV()
    {
        const float UP_SPEED = 8f;
        const float DOWN_SPEED = 4f;

        float fov = Camera.main.fieldOfView;

        if (playerController.IsMoving || playerController.IsInDash)
        {
            // When moving
            if (playerController.IsMoving)
            {
                if (!playerController.IsSprinting)
                {
                    if (fov < walkFOV)
                    {
                        fov += (walkFOV - fov) * UP_SPEED * Time.deltaTime;
                    }
                    else
                    {
                        fov -= (fov - walkFOV) * DOWN_SPEED * Time.deltaTime;
                    }
                }
                else
                {
                    if (fov < sprintFOV)
                    {
                        fov += (sprintFOV - fov) * UP_SPEED * Time.deltaTime;
                    }
                }
            }

            // When dashing
            if (playerController.IsInDash)
            {
                if (fov < dashFOV)
                {
                    fov += (dashFOV - fov) * UP_SPEED * Time.deltaTime;
                }
                else if (fov > baseFOV)
                {
                    fov -= (fov - baseFOV) * DOWN_SPEED * 1.5f * Time.deltaTime;
                }
            }
        }
        else
        {
            if (fov > baseFOV)
            {
                fov -= (fov - baseFOV) * DOWN_SPEED * Time.deltaTime;
            }
        }

        Camera.main.fieldOfView = fov;
    }

    private void AdjustNearClipPlane()
    {
        float nearClip = 0f;
        float fov = Camera.main.fieldOfView;

        if (fov == baseFOV) { nearClip = 0.0335f; } // <- When Idling
        else if (fov == walkFOV) { nearClip = 0.03f; } // When Walking
        else { nearClip = 0.0305f; } // <---------------- When Sprinting/Sliding

        Camera.main.nearClipPlane = nearClip;
    }

    private void CenterView()
    {
        Vector3 centeredRotation = Camera.main.transform.rotation.eulerAngles;
        centeredRotation.x = 0f;

        Camera.main.transform.rotation = Quaternion.Euler(centeredRotation);
        rotationDone = 0f;
    }

    private void Bob()
    {
        bool isSprinting = playerController.IsSprinting;

        bobSpeed = ((isSprinting) ? 2.25f : 0.75f);
        bobLimit = ((isSprinting) ? 0.175f : 0.075f);

        if (gamepadConnected)
        {
            float totalSpeedValue = playerController.AbsTotalSpeedValue;

            bobSpeed *= totalSpeedValue;
            bobLimit *= totalSpeedValue;
        }

        if (bobUp)
        {
            bobOffsetX -= bobSpeed * Time.deltaTime;

            if (bobOffsetX < -bobLimit) { bobUp = false; }
        }
        else
        {
            bobOffsetX += bobSpeed * Time.deltaTime;

            if (bobOffsetX > bobLimit) { bobUp = true; }
        }

        rotation.x += bobOffsetX;
    }

    private float rotaZ_ = 0f;

    private void Tilt()
    {
        if (playerController.IsMoving)
        {
            if (gamepadConnected)
            {
                if (Helper.GetAxis_Gamepad(true, true) < 0f)
                {
                    if (rotaZ_ < TILT_MAX) { rotaZ_ += (TILT_MAX - rotaZ_) * TILT_SPEED * Time.deltaTime; }
                }
                else if (Helper.GetAxis_Gamepad(true, true) > 0f)
                {
                    if (rotaZ_ > -TILT_MAX) { rotaZ_ -= (rotaZ_ - TILT_MAX) * TILT_SPEED * Time.deltaTime; }
                }

                rotation.z += rotaZ_;
            }
        }
        else
        {
            rotaZ_ = 0f;
            rotation.z -= rotation.z * TILT_SPEED * Time.deltaTime;
        }
    }

    private void ManageSpeedLines()
    {
        if (playerController.IsGrounded)
        {
            if (playerController.IsMoving &&
            (playerController.IsSprinting || playerController.IsSliding || playerController.IsInDash))
            {
                if (!speedLinesFeature.isActive)
                {
                    speedLinesFeature.SetActive(true);
                }

                if (alpha < 0.5f) { alpha += 5f * Time.deltaTime; }
            }
            else
            {
                if (alpha > 0f) { alpha -= 5f * Time.deltaTime; }
                else { speedLinesFeature.SetActive(false); }
            }

            if (speedLinesFeature.isActive)
            {
                if (alpha < 0f) { alpha = 0f; }
                else if (alpha > 0.5f) { alpha = 0.5f; }

                if (gamepadConnected)
                {
                    alpha *= playerController.AbsTotalSpeedValue;
                }

                speedLinesFeature.passMaterial.SetFloat("_Alpha", alpha);
            }
        }
    }

    private void RetrieveManagers()
    {
        if (utilityManager.childCount == 2)
        {
            mouseManager = utilityManager.GetChild(0).GetComponent<MouseManager>();
            keysManager = utilityManager.GetChild(1).GetComponent<ControlsManager>();
        }

        pauseManager = utilityManager.GetChild(0).GetComponent<PauseManager>();
        mouseManager = utilityManager.GetChild(1).GetComponent<MouseManager>();
        keysManager = utilityManager.GetChild(2).GetComponent<ControlsManager>();
    }
    #endregion

    #region public
    public void Shake()
    {
        int directionNb = Random.Range(0, 4);
        Vector3 direction = new Vector3();

        switch (directionNb)
        {
            case 0: { direction = Vector3.up; } break;
            case 1: { direction = Vector3.down; } break;
            case 2: { direction = Vector3.left; } break;
            case 3: { direction = Vector3.right; } break;
        }

        direction *= shakeStrength;

        transform.Translate(shakeSpeed * direction * Time.deltaTime);
        posAfterShake = transform.position;
    }
    #endregion
    #endregion
    #endregion
}
