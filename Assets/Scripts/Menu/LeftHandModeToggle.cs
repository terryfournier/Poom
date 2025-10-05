using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class LeftHandModeToggle : MonoBehaviour
{
    private Toggle toggle = null;

    public static bool IsOn = false;

    // Awake is called once before all other methods, when the GameObject is created
    private void Awake()
    {
        toggle = GetComponent<Toggle>();
    }


    public void OnValueChanged()
    {
        Helper.ToggleBool(ref IsOn);

        RemapForMenu();
        RemapForGameplay();
    }


    private void RemapForMenu()
    {
        // Keyboard remapping
        if (Helper.KeyboardUsable)
        {
            Keyboard keyboard = Helper.CurrentKeyboard;

            ControlsManager.Remap_Keyboard(KeyboardActions.MENU_BACK, (IsOn) ? keyboard.numpadMinusKey : keyboard.escapeKey);
        }

        // Gamepad remapping
        if (Helper.GamepadUsable)
        {
            StickControl stick = (IsOn) ? Helper.CurrentGamepad.rightStick : Helper.CurrentGamepad.leftStick;

            ControlsManager.Remap_Gamepad(GamepadActions.MENU_UP_2, stick.up);
            ControlsManager.Remap_Gamepad(GamepadActions.MENU_DOWN_2, stick.down);
            ControlsManager.Remap_Gamepad(GamepadActions.MENU_LEFT_2, stick.left);
            ControlsManager.Remap_Gamepad(GamepadActions.MENU_RIGHT_2, stick.right);
        }
    }

    private void RemapForGameplay()
    {
        // Keyboard remapping
        if (Helper.KeyboardUsable)
        {
            Keyboard keyboard = Helper.CurrentKeyboard;

            // Movement keys
            ControlsManager.Remap_Keyboard(KeyboardActions.JUMP, (IsOn) ? keyboard.numpad0Key : keyboard.spaceKey);
            ControlsManager.Remap_Keyboard(KeyboardActions.SPRINT, (IsOn) ? keyboard.rightCtrlKey : keyboard.leftShiftKey);
            ControlsManager.Remap_Keyboard(KeyboardActions.SLIDE, (IsOn) ? keyboard.numpad1Key : keyboard.leftAltKey);
            ControlsManager.Remap_Keyboard(KeyboardActions.DASH, (IsOn) ? keyboard.numpad2Key : keyboard.leftCtrlKey);
            ControlsManager.Remap_Keyboard(KeyboardActions.HEAL, (IsOn) ? keyboard.rightShiftKey : keyboard.aKey);

            // Interaction key
            ControlsManager.Remap_Keyboard(KeyboardActions.INTERACT, (IsOn) ? keyboard.numpad4Key : keyboard.eKey);

            // Reload key
            ControlsManager.Remap_Keyboard(KeyboardActions.RELOAD, (IsOn) ? keyboard.enterKey : keyboard.rKey);

            // Center View key
            ControlsManager.Remap_Keyboard(KeyboardActions.CENTER_VIEW, (IsOn) ? keyboard.numpadPeriodKey : keyboard.vKey);

            // Pause/Resume key
            ControlsManager.Remap_Keyboard(KeyboardActions.TOGGLE_PAUSE, (IsOn) ? keyboard.numpadMinusKey : keyboard.escapeKey);
        }

        // Gamepad remapping
        if (Helper.GamepadUsable)
        {
            // Action buttons
            ControlsManager.SwapButtons_Gamepad(GamepadActions.SPRINT, GamepadActions.CENTER_VIEW);
            ControlsManager.SwapButtons_Gamepad(GamepadActions.HEAL, GamepadActions.SHOOT);
            ControlsManager.SwapButtons_Gamepad(GamepadActions.SCROLL_WPN_UP, GamepadActions.SCROLL_WPN_DOWN);
            ControlsManager.SwapButtons_Gamepad(GamepadActions.AIM_SLOWLY, GamepadActions.TOGGLE_PAUSE);
            ControlsManager.SwapButtons_Gamepad(GamepadActions.JUMP, GamepadActions.PICK_WPN_2);
            ControlsManager.SwapButtons_Gamepad(GamepadActions.SLIDE, GamepadActions.PICK_WPN_3);
            ControlsManager.SwapButtons_Gamepad(GamepadActions.RELOAD, GamepadActions.PICK_WPN_1);
            ControlsManager.SwapButtons_Gamepad(GamepadActions.DASH, GamepadActions.TOGGLE_FLASHLIGHT);
        }
    }
}
