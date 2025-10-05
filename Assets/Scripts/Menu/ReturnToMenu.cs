using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReturnToMenu : MonoBehaviour
{
    [SerializeField] private GameObject titleScreenObj = null;
    [SerializeField] private GameObject settingsMenuObj = null;
    [SerializeField] private PauseManager pauseManager = null;

    private MenuButtonSelection menuButtonSelection = null;

    // Awake is called once before all other methods, when the GameObject is created
    private void Awake()
    {
        menuButtonSelection = GetComponent<MenuButtonSelection>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if (!pauseManager)
        {
            pauseManager = FindAnyObjectByType<PauseManager>();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (Helper.GamepadUsable)
        {
            if (ControlsManager.HasPressed_Gamepad(GamepadActions.MENU_BACK, false))
            {
                if (Helper.IsAtScene("Menu"))
                {
                    GetComponent<Button>().onClick?.Invoke();
                }
                else
                {
                    pauseManager.Unpause();
                }
            }
        }
        else if (ControlsManager.HasPressed_Keyboard(KeyboardActions.MENU_BACK, false))
        {
            if (Helper.IsAtScene("Menu"))
            {
                GetComponent<Button>().onClick?.Invoke();
            }
            else
            {
                pauseManager.Unpause();
            }
        }

        if (menuButtonSelection.IsHighlighted)
        {
            // When playing with Gamepad
            if (Helper.GamepadUsable)
            {
                if (ControlsManager.HasPressed_Gamepad(GamepadActions.MENU_PRESS, false))
                {
                    GetComponent<Button>().onClick?.Invoke();
                }

                return;
            }

            // When playing with Keyboard
            if (Helper.KeyboardUsable)
            {
                if (ControlsManager.HasPressed_Keyboard(KeyboardActions.MENU_PRESS_1, false) ||
                    ControlsManager.HasPressed_Keyboard(KeyboardActions.MENU_PRESS_2, false))
                {
                    GetComponent<Button>().onClick?.Invoke();
                }
            }
        }
    }


    public void ReturnToMenuFunc()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ReturnToMenuDontReload()
    {
        settingsMenuObj.SetActive(false);
        titleScreenObj.SetActive(true);
    }
}
