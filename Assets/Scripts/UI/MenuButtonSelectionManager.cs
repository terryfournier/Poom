using UnityEngine;

public class MenuButtonSelectionManager : MonoBehaviour
{
    private const float SELECTION_TIMER_MAX = 0.333f;

    [SerializeField] private GameObject[] buttons = null;

    private ControlsManager keysManager = null;
    private float selectionTimer = 0f;
    private int selectionIndex = 0;
    private bool hasRetrievedKeysManager = false;
    private bool canUpdateButton = true;

    public static bool SelectionResets = true;

    // Update is called once per frame
    private void Update()
    {
        // Retrieve Game Keys Manager
        if (!hasRetrievedKeysManager)
        {
            keysManager = GameObject.Find("Game Keys Manager(Clone)").GetComponent<ControlsManager>();
            hasRetrievedKeysManager = true;
        }

        // Make it possible to spam to select buttons faster
        if (SelectionResets)
        {
            ResetSelection();
        }

        // When playing with Gamepad
        if (Helper.GamepadUsable)
        {
            NavigateMenu_Gamepad();
        }
        // When playing with Keyboard/Mouse
        else
        {
            if (Helper.MouseUsable) { NavigateMenu_Mouse(); }
            if (Helper.KeyboardUsable) { NavigateMenu_Keyboard(); }
        }

        // Makes button highlighting loop back
        if (canUpdateButton)
        {
            // Loops back button index
            if (selectionIndex < 0) { selectionIndex = buttons.Length - 1; }
            else if (selectionIndex > buttons.Length - 1) { selectionIndex = 0; }

            // Makes current indexed button highlighted
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].GetComponent<MenuButtonSelection>().IsHighlighted = (selectionIndex == i);
            }

            // Avoids making this when unnecessary
            canUpdateButton = false;
        }

        // Allow button selection whenever a key/button
        // of a connected Keyboard/Mouse/Gamepad is pressed
        if (Helper.GameDevicePress) { canUpdateButton = true; }
    }


    private void NavigateButtons(in bool _upwards)
    {
        if (selectionTimer < SELECTION_TIMER_MAX) { selectionTimer += Time.unscaledDeltaTime; }
        else
        {
            selectionIndex = (_upwards) ? selectionIndex - 1 : selectionIndex + 1;
            selectionTimer = 0f;
        }
    }

    private void NavigateMenu_Keyboard()
    {
        if (ControlsManager.HasPressed_Keyboard(KeyboardActions.MENU_UP_1, true) ||
            ControlsManager.HasPressed_Keyboard(KeyboardActions.MENU_UP_2, true))
        {
            NavigateButtons(true);
        }
        else if (ControlsManager.HasPressed_Keyboard(KeyboardActions.MENU_DOWN_1, true) ||
                 ControlsManager.HasPressed_Keyboard(KeyboardActions.MENU_DOWN_2, true))
        {
            NavigateButtons(false);
        }
        else { selectionTimer = SELECTION_TIMER_MAX; }
    }
    private void NavigateMenu_Mouse()
    {
        // Enable selection with Mouse
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].GetComponent<MenuButtonSelection>().IsHighlighted)
            {
                selectionIndex = i;
                canUpdateButton = true;
            }
        }
    }
    private void NavigateMenu_Gamepad()
    {
        if (ControlsManager.HasPressed_Gamepad(GamepadActions.MENU_UP_1, true) ||
            ControlsManager.HasPressed_Gamepad(GamepadActions.MENU_UP_2, true))
        {
            NavigateButtons(true);
        }
        else if (ControlsManager.HasPressed_Gamepad(GamepadActions.MENU_DOWN_1, true) ||
                 ControlsManager.HasPressed_Gamepad(GamepadActions.MENU_DOWN_2, true))
        {
            NavigateButtons(false);
        }
        else { selectionTimer = SELECTION_TIMER_MAX; }
    }


    public void ResetSelection()
    {
        selectionIndex = 0;
        selectionTimer = SELECTION_TIMER_MAX;

        SelectionResets = false;
    }
}
