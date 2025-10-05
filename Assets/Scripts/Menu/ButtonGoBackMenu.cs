using UnityEngine;

public class ButtonGoBackMenu : MonoBehaviour
{
    [SerializeField] GameObject parentMenu;
    [SerializeField] GameObject parentCurrentNoneMenuState;
    [SerializeField] GameObject tentacule;

    public void GoBackToMenu()
    {
        if (parentCurrentNoneMenuState.name == "Credits")
        {
            if (!tentacule) { tentacule = GameObject.Find("Tentacule"); }
            if (tentacule) { tentacule.SetActive(true); }
        }

        if (parentMenu) { parentMenu.SetActive(true); }
        if (parentCurrentNoneMenuState) { parentCurrentNoneMenuState.SetActive(false); }
    }

    // Update is called once per frame
    private void Update()
    {
        // When a Gamepad is connected
        // Use it
        if (Helper.GamepadUsable && ControlsManager.HasPressed_Gamepad(GamepadActions.MENU_BACK, false))
        {
            GoBackToMenu();

            return;
        }

        // When no Gamepad is connected
        // Use Keyboard
        if (Helper.KeyboardUsable && !Helper.GamepadUsable &&
            ControlsManager.HasPressed_Keyboard(KeyboardActions.MENU_BACK, false))
        {
            GoBackToMenu();
        }
    }
}
