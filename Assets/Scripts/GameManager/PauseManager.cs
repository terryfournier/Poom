using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] GameObject canvaPause;
    [SerializeField] GameObject buttonsObj = null;

    public bool isGamePaused;

    private AudioManager audioManager = null;
    private int deviceCountBuffer = 0;
    private bool hasPausedBecauseDeconnexion = false;

    private void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
        deviceCountBuffer = Helper.GameDeviceCount;
        isGamePaused = false;
    }

    private void Update()
    {
        // Pause game when a device connected/deconnected
        if (!isGamePaused && !hasPausedBecauseDeconnexion &&
            deviceCountBuffer != Helper.GameDeviceCount)
        {
            PauseUnpause();

            deviceCountBuffer = Helper.GameDeviceCount;
            hasPausedBecauseDeconnexion = true;

            return;
        }

        hasPausedBecauseDeconnexion = false;

        // If a Gamepad is connected
        // Use it
        if (Helper.GamepadUsable)
        {
            // Pause/Unpause game by pressing the <Start> button
            if (ControlsManager.HasPressed_Gamepad(GamepadActions.TOGGLE_PAUSE, false))
            {
                PauseUnpause();
            }

            // If game is Paused
            // Unpause game by pressing the <East> button
            if (isGamePaused && Gamepad.current.buttonEast.wasPressedThisFrame)
            {
                if (!SettingsMenu.IsOn) { PauseUnpause(); }
                else
                {
                    SettingsMenu.Deactivate();
                    buttonsObj.SetActive(true);
                }
            }
        }
        // If no Gamepad is connected
        // Use Keyboard
        // Pause/Unpause by pressing the <Escape> key
        else if (Helper.KeyboardUsable && ControlsManager.HasPressed_Keyboard(KeyboardActions.TOGGLE_PAUSE, false))
        {
            if (!SettingsMenu.IsOn) { PauseUnpause(); }
            else
            {
                SettingsMenu.Deactivate();
                buttonsObj.SetActive(true);
            }
        }

        // When game is paused
        // Stop time in all the game
        if (isGamePaused)
        {
            Time.timeScale = 0f;

            return;
        }

        // When game is running
        // Enable time to flow at normal speed in all the game
        Time.timeScale = 1f;
    }


    private void PauseUnpause()
    {
        isGamePaused = !isGamePaused;
        canvaPause.SetActive(isGamePaused);
    }
    public void Pause()
    {
        isGamePaused = true;
        canvaPause.SetActive(true);
    }
    public void Unpause()
    {
        isGamePaused = false;
        canvaPause.SetActive(false);
    }


    public void OnClickResume()
    {
        isGamePaused = false;
        canvaPause.SetActive(false);
    }
    public void OnClickReturnMenu()
    {
        isGamePaused = false;
        canvaPause.SetActive(false);
        audioManager.StopEverything();
        SceneManager.LoadScene("Menu");
    }
    public void OnClickQuit()
    {
        isGamePaused = false;
        canvaPause.SetActive(false);
        Application.Quit();
    }

}
