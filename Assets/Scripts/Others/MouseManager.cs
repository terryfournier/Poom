using UnityEngine;
using UnityEngine.SceneManagement;

public class MouseManager : MonoBehaviour
{
    private PauseManager pauseManager = null;
    private Vector2 screenCenter = new Vector2(Screen.width, Screen.height) * 0.5f;
    private Vector2 sensitivity = new Vector2(0.0625f, 0.0375f);
    private string activeSceneName = "";
    private float aimFactor = 0f;
    private bool isSnapped = false;
    private bool hasAdaptedToPlayMode = false;

    public Vector2 Sensitivity
    {
        get { return sensitivity * SensitivitySlider.Sensitivity; }
    }
    public float AimFactor
    {
        get => aimFactor;
    }

    // Awake is called once before all other methods, when the GameObject is created
    private void Awake()
    {
        SceneManager.activeSceneChanged += GetActiveSceneName;

        SnapCursor();
    }

    // Update is called once per frame
    private void Update()
    {
        aimFactor =
                ((Helper.GamepadUsable && ControlsManager.HasPressed_Gamepad(GamepadActions.AIM_SLOWLY, true)) ||
                (!Helper.GamepadUsable && Helper.MouseUsable && ControlsManager.HasPressed_Mouse(MouseActions.AIM_SLOWLY, true)))
                ? 0.5f
                : 1f;

        if (Helper.GamepadUsable)
        {
            if (!hasAdaptedToPlayMode)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Confined;

                hasAdaptedToPlayMode = true;
            }

            return;
        }

        hasAdaptedToPlayMode = false;

        if (Helper.MouseUsable)
        {
            // When in menu or in game over
            // Make cursor visible and enable free movement
            if (!IsInGame())
            {
                if (!isSnapped && activeSceneName == "GameOver")
                {
                    SnapCursor();
                    isSnapped = true;
                }

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            // When in game
            else
            {
                // When game is paused
                if (pauseManager.isGamePaused)
                {
                    // Snap cursor to center, make it visible and make it free
                    if (Cursor.lockState != CursorLockMode.None)
                    {
                        SnapCursor();

                        Cursor.visible = true;
                        Cursor.lockState = CursorLockMode.None;
                    }
                }
                // When game is running
                else
                {
                    // Hide cursor and lock it inside
                    if (Cursor.lockState != CursorLockMode.Confined)
                    {
                        Cursor.visible = false;
                        Cursor.lockState = CursorLockMode.Confined;
                    }
                }
            }
        }
    }


    /// <summary>
    /// Snaps mouse cursor to center of window
    /// </summary>
    public static void SnapCursor()
    {
        if (!Helper.MouseConnected) { return; }

        Helper.CurrentMouse.WarpCursorPosition(new Vector2(Screen.width, Screen.height) * 0.5f);
    }
    /// <summary>
    /// Snaps mouse cursor to _position (in pixels)
    /// </summary>
    /// <param name="_position">The position to snap the mouse curso to</param>
    public static void SnapCursor(in Vector2 _position)
    {
        if (!Helper.MouseConnected) { return; }

        Helper.CurrentMouse.WarpCursorPosition(_position);
    }

    private bool IsInGame()
    {
        if (activeSceneName != "Menu" && activeSceneName != "GameOver")
        {
            pauseManager = GameObject.Find("PauseManager(Clone)").GetComponent<PauseManager>();
        }

        return
            activeSceneName != "Menu" &&
            activeSceneName != "Pause" &&
            activeSceneName != "GameOver";
    }

    private void GetActiveSceneName(Scene _current, Scene _next)
    {
        activeSceneName =
            (_current.name != _next.name)
            ? _next.name
            : _current.name;
    }
}
