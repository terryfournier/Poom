using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public enum KeyboardActions
{
    MENU_PRESS_1,
    MENU_PRESS_2,
    MENU_UP_1,
    MENU_DOWN_1,
    MENU_LEFT_1,
    MENU_RIGHT_1,
    MENU_UP_2,
    MENU_DOWN_2,
    MENU_LEFT_2,
    MENU_RIGHT_2,
    MENU_BACK,
    MENU_TAB_LEFT_1,
    MENU_TAB_RIGHT_1,
    MENU_TAB_LEFT_2,
    MENU_TAB_RIGHT_2,

    MUTE_ALL,
    MUTE,

    MOVE_FWD,
    MOVE_BACK,
    MOVE_LEFT,
    MOVE_RIGHT,

    SPRINT,
    JUMP,
    SLIDE,
    DASH,
    HEAL,
    CENTER_VIEW,

    INTERACT,
    PICK_WPN_1,
    PICK_WPN_2,
    PICK_WPN_3,
    RELOAD,

    TOGGLE_PAUSE,

    ACTION_NB
}

public enum MouseActions
{
    MENU_PRESS,

    TOGGLE_FLASHLIGHT,

    SHOOT,

    AIM_SLOWLY,

    ACTION_NB
}

public enum GamepadActions
{
    MENU_PRESS,
    MENU_UP_1,
    MENU_DOWN_1,
    MENU_LEFT_1,
    MENU_RIGHT_1,
    MENU_UP_2,
    MENU_DOWN_2,
    MENU_LEFT_2,
    MENU_RIGHT_2,
    MENU_BACK,
    MENU_TAB_LEFT,
    MENU_TAB_RIGHT,

    MUTE_ALL,
    MUTE,

    TOGGLE_FLASHLIGHT,

    SPRINT,
    JUMP,
    SLIDE,
    DASH,
    HEAL,
    CENTER_VIEW,

    INTERACT,
    PICK_WPN_1,
    PICK_WPN_2,
    PICK_WPN_3,
    SCROLL_WPN_UP,
    SCROLL_WPN_DOWN,
    RELOAD,
    SHOOT,

    TOGGLE_PAUSE,

    AIM_SLOWLY,

    ACTION_NB
}

public class ControlsManager : MonoBehaviour
{
    private static readonly ButtonControl[] gamepadButtons = new ButtonControl[(int)(GamepadActions.ACTION_NB)];
    private static readonly KeyControl[] keyboardKeys = new KeyControl[(int)(KeyboardActions.ACTION_NB)];
    private static readonly ButtonControl[] mouseButtons = new ButtonControl[(int)(MouseActions.ACTION_NB)];

    public static bool KeyboardReady = false;
    public static bool MouseReady = false;
    public static bool GamepadReady = false;

    // Awake is called once before all other methods, when the GameObject is created
    private void Awake()
    {
        InitKeyboardActions();
        InitMouseActions();
        InitGamepadActions();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!Helper.KeyboardConnected) { KeyboardReady = false; }
        if (!Helper.MouseConnected) { MouseReady = false; }
        if (!Helper.GamepadConnected) { GamepadReady = false; }

        if (Helper.KeyboardConnected && !KeyboardReady)
        {
            InitKeyboardActions();
            KeyboardReady = true;
        }

        if (Helper.MouseConnected && !MouseReady)
        {
            InitMouseActions();
            MouseReady = true;
        }

        if (Helper.GamepadConnected && !GamepadReady)
        {
            InitGamepadActions();
            GamepadReady = true;
        }
    }


    private static void InitKeyboardActions()
    {
        Keyboard keyboard = Helper.CurrentKeyboard;

        if (keyboard != null)
        {
            keyboardKeys[(int)(KeyboardActions.MENU_PRESS_1)] = keyboard.enterKey;
            keyboardKeys[(int)(KeyboardActions.MENU_PRESS_2)] = keyboard.spaceKey;

            keyboardKeys[(int)(KeyboardActions.MENU_UP_1)] = keyboard.upArrowKey;
            keyboardKeys[(int)(KeyboardActions.MENU_DOWN_1)] = keyboard.downArrowKey;
            keyboardKeys[(int)(KeyboardActions.MENU_LEFT_1)] = keyboard.leftArrowKey;
            keyboardKeys[(int)(KeyboardActions.MENU_RIGHT_1)] = keyboard.rightArrowKey;
            keyboardKeys[(int)(KeyboardActions.MENU_UP_2)] = keyboard.wKey;
            keyboardKeys[(int)(KeyboardActions.MENU_DOWN_2)] = keyboard.sKey;
            keyboardKeys[(int)(KeyboardActions.MENU_LEFT_2)] = keyboard.aKey;
            keyboardKeys[(int)(KeyboardActions.MENU_RIGHT_2)] = keyboard.dKey;

            keyboardKeys[(int)(KeyboardActions.MENU_BACK)] = keyboard.escapeKey;
            keyboardKeys[(int)(KeyboardActions.MENU_TAB_LEFT_1)] = keyboard.qKey;
            keyboardKeys[(int)(KeyboardActions.MENU_TAB_RIGHT_1)] = keyboard.eKey;
            keyboardKeys[(int)(KeyboardActions.MENU_TAB_LEFT_2)] = keyboard.digit1Key;
            keyboardKeys[(int)(KeyboardActions.MENU_TAB_RIGHT_2)] = keyboard.digit3Key;

            keyboardKeys[(int)(KeyboardActions.MUTE_ALL)] = keyboard.semicolonKey;
            keyboardKeys[(int)(KeyboardActions.MUTE)] = keyboard.tKey;

            keyboardKeys[(int)(KeyboardActions.MOVE_FWD)] = keyboard.wKey;
            keyboardKeys[(int)(KeyboardActions.MOVE_BACK)] = keyboard.sKey;
            keyboardKeys[(int)(KeyboardActions.MOVE_LEFT)] = keyboard.aKey;
            keyboardKeys[(int)(KeyboardActions.MOVE_RIGHT)] = keyboard.dKey;

            keyboardKeys[(int)(KeyboardActions.SPRINT)] = keyboard.leftShiftKey;
            keyboardKeys[(int)(KeyboardActions.JUMP)] = keyboard.spaceKey;
            keyboardKeys[(int)(KeyboardActions.SLIDE)] = keyboard.leftAltKey;
            keyboardKeys[(int)(KeyboardActions.DASH)] = keyboard.leftCtrlKey;
            keyboardKeys[(int)(KeyboardActions.HEAL)] = keyboard.qKey;
            keyboardKeys[(int)(KeyboardActions.CENTER_VIEW)] = keyboard.vKey;

            keyboardKeys[(int)(KeyboardActions.INTERACT)] = keyboard.eKey;
            keyboardKeys[(int)(KeyboardActions.PICK_WPN_1)] = keyboard.digit1Key;
            keyboardKeys[(int)(KeyboardActions.PICK_WPN_2)] = keyboard.digit2Key;
            keyboardKeys[(int)(KeyboardActions.PICK_WPN_3)] = keyboard.digit3Key;
            keyboardKeys[(int)(KeyboardActions.RELOAD)] = keyboard.rKey;

            keyboardKeys[(int)(KeyboardActions.TOGGLE_PAUSE)] = keyboard.escapeKey;
        }
    }
    private static void InitMouseActions()
    {
        Mouse mouse = Helper.CurrentMouse;

        if (mouse != null)
        {
            mouseButtons[(int)(MouseActions.MENU_PRESS)] = mouse.leftButton;
            mouseButtons[(int)(MouseActions.TOGGLE_FLASHLIGHT)] = mouse.middleButton;
            mouseButtons[(int)(MouseActions.SHOOT)] = mouse.leftButton;
            mouseButtons[(int)(MouseActions.AIM_SLOWLY)] = mouse.rightButton;
        }
    }
    private static void InitGamepadActions()
    {
        Gamepad gamepad = Helper.CurrentGamepad;

        if (gamepad != null)
        {
            gamepadButtons[(int)(GamepadActions.MENU_PRESS)] = gamepad.buttonSouth;
            gamepadButtons[(int)(GamepadActions.MENU_UP_1)] = gamepad.dpad.up;
            gamepadButtons[(int)(GamepadActions.MENU_DOWN_1)] = gamepad.dpad.down;
            gamepadButtons[(int)(GamepadActions.MENU_LEFT_1)] = gamepad.dpad.left;
            gamepadButtons[(int)(GamepadActions.MENU_RIGHT_1)] = gamepad.dpad.right;
            gamepadButtons[(int)(GamepadActions.MENU_UP_2)] = gamepad.leftStick.up;
            gamepadButtons[(int)(GamepadActions.MENU_DOWN_2)] = gamepad.leftStick.down;
            gamepadButtons[(int)(GamepadActions.MENU_LEFT_2)] = gamepad.leftStick.left;
            gamepadButtons[(int)(GamepadActions.MENU_RIGHT_2)] = gamepad.leftStick.right;
            gamepadButtons[(int)(GamepadActions.MENU_BACK)] = gamepad.buttonEast;
            gamepadButtons[(int)(GamepadActions.MENU_TAB_LEFT)] = gamepad.leftShoulder;
            gamepadButtons[(int)(GamepadActions.MENU_TAB_RIGHT)] = gamepad.rightShoulder;

            gamepadButtons[(int)(GamepadActions.MUTE_ALL)] = gamepad.buttonNorth;
            gamepadButtons[(int)(GamepadActions.MUTE)] = gamepad.buttonWest;

            gamepadButtons[(int)(GamepadActions.TOGGLE_FLASHLIGHT)] = gamepad.dpad.up;

            gamepadButtons[(int)(GamepadActions.SPRINT)] = gamepad.leftStickButton;
            gamepadButtons[(int)(GamepadActions.JUMP)] = gamepad.buttonSouth;
            gamepadButtons[(int)(GamepadActions.SLIDE)] = gamepad.buttonEast;
            gamepadButtons[(int)(GamepadActions.DASH)] = gamepad.buttonNorth;
            gamepadButtons[(int)(GamepadActions.HEAL)] = gamepad.leftTrigger;
            gamepadButtons[(int)(GamepadActions.CENTER_VIEW)] = gamepad.rightStickButton;
            gamepadButtons[(int)(GamepadActions.AIM_SLOWLY)] = gamepad.selectButton;

            gamepadButtons[(int)(GamepadActions.INTERACT)] = gamepad.buttonWest;
            gamepadButtons[(int)(GamepadActions.PICK_WPN_1)] = gamepad.dpad.left;
            gamepadButtons[(int)(GamepadActions.PICK_WPN_2)] = gamepad.dpad.down;
            gamepadButtons[(int)(GamepadActions.PICK_WPN_3)] = gamepad.dpad.right;
            gamepadButtons[(int)(GamepadActions.SCROLL_WPN_UP)] = gamepad.rightShoulder;
            gamepadButtons[(int)(GamepadActions.SCROLL_WPN_DOWN)] = gamepad.leftShoulder;
            gamepadButtons[(int)(GamepadActions.RELOAD)] = gamepad.buttonWest;
            gamepadButtons[(int)(GamepadActions.SHOOT)] = gamepad.rightTrigger;

            gamepadButtons[(int)(GamepadActions.TOGGLE_PAUSE)] = gamepad.startButton;
        }
    }


    public static bool HasPressed_Keyboard(in KeyboardActions _action, in bool _isPressConstant)
    {
        if (_action >= KeyboardActions.ACTION_NB)
        {
            return false;
        }

        if (Helper.KeyboardUsable)
        {
            return (_isPressConstant) ? keyboardKeys[(int)(_action)].IsPressed() : keyboardKeys[(int)(_action)].wasPressedThisFrame;
        }

        return false;
    }
    public static bool HasPressed_Mouse(in MouseActions _action, in bool _isPressConstant)
    {
        if (_action >= MouseActions.ACTION_NB)
        {
            return false;
        }

        if (Helper.MouseUsable)
        {
            return (_isPressConstant) ? mouseButtons[(int)(_action)].IsPressed() : mouseButtons[(int)(_action)].wasPressedThisFrame;
        }

        return false;
    }
    public static bool HasPressed_Gamepad(in GamepadActions _action, in bool _isPressConstant)
    {
        if (_action >= GamepadActions.ACTION_NB)
        {
            return false;
        }

        if (Helper.GamepadUsable)
        {
            return (_isPressConstant) ? gamepadButtons[(int)(_action)].IsPressed() : gamepadButtons[(int)(_action)].wasPressedThisFrame;
        }

        return false;
    }

    public static bool HasReleased_Keyboard(in KeyboardActions _action)
    {
        if (_action >= KeyboardActions.ACTION_NB)
        {
            return false;
        }

        if (Helper.KeyboardUsable)
        {
            return keyboardKeys[(int)(_action)].wasReleasedThisFrame;
        }

        return false;
    }
    public static bool HasReleased_Mouse(in MouseActions _action)
    {
        if (_action >= MouseActions.ACTION_NB)
        {
            return false;
        }

        if (Helper.MouseUsable)
        {
            return mouseButtons[(int)(_action)].wasReleasedThisFrame;
        }

        return false;
    }
    public static bool HasReleased_Gamepad(in GamepadActions _action)
    {
        if (_action >= GamepadActions.ACTION_NB)
        {
            return false;
        }

        if (Helper.GamepadUsable)
        {
            return gamepadButtons[(int)(_action)].wasReleasedThisFrame;
        }

        return false;
    }


    public static void Remap_Keyboard(in KeyboardActions _action, in KeyControl _key)
    {
        if (_action >= KeyboardActions.ACTION_NB)
        {
            Helper.ErrorColour(Colour.WHITE, "Remap_Keyboard: Invalid _action.");
            return;
        }

        keyboardKeys[(int)(_action)] = _key;
    }
    public static void Remap_Mouse(in MouseActions _action, in ButtonControl _button)
    {
        if (_action >= MouseActions.ACTION_NB)
        {
            Helper.ErrorColour(Colour.WHITE, "Remap_Mouse: Invalid _action.");
            return;
        }

        mouseButtons[(int)(_action)] = _button;
    }
    public static void Remap_Gamepad(in GamepadActions _action, in ButtonControl _button)
    {
        if (_action >= GamepadActions.ACTION_NB)
        {
            Helper.ErrorColour(Colour.WHITE, "Remap_Gamepad: Invalid _action.");
            return;
        }

        gamepadButtons[(int)(_action)] = _button;
    }


    public static KeyControl GetKey(in KeyboardActions _action)
    {
        return keyboardKeys[(int)(_action)];
    }
    public static ButtonControl GetButton_Mouse(in MouseActions _action)
    {
        return mouseButtons[(int)(_action)];
    }
    public static ButtonControl GetButton_Gamepad(in GamepadActions _action)
    {
        return gamepadButtons[(int)(_action)];
    }


    public static void SwapKeys(in KeyboardActions _action1, in KeyboardActions _action2)
    {
        int iAction1 = (int)(_action1);
        int iAction2 = (int)(_action2);

        KeyControl action1Cpy = keyboardKeys[iAction1];
        keyboardKeys[iAction1] = keyboardKeys[iAction2];
        keyboardKeys[iAction2] = action1Cpy;
    }
    public static void SwapButtons_Mouse(in MouseActions _action1, in MouseActions _action2)
    {
        int iAction1 = (int)(_action1);
        int iAction2 = (int)(_action2);

        ButtonControl action1Cpy = mouseButtons[iAction1];
        mouseButtons[iAction1] = mouseButtons[iAction2];
        mouseButtons[iAction2] = action1Cpy;
    }
    public static void SwapButtons_Gamepad(in GamepadActions _action1, in GamepadActions _action2)
    {
        int iAction1 = (int)(_action1);
        int iAction2 = (int)(_action2);

        ButtonControl action1Cpy = gamepadButtons[iAction1];
        gamepadButtons[iAction1] = gamepadButtons[iAction2];
        gamepadButtons[iAction2] = action1Cpy;
    }
}
