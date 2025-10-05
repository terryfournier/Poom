using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DynamicFovToggle : MonoBehaviour, IPointerDownHandler
{
    public static bool IsOn = false;

    [HideInInspector] public bool IsSelected = false;

    private Toggle toggle = null;

    // Awake is called once before all other methods, when the GameObject is created
    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        toggle.isOn = IsOn;
    }

    // Update is called once per frame
    private void Update()
    {
        IsOn = toggle.isOn;

        if (IsSelected)
        {
            // When playing with Keyboard
            if (Helper.KeyboardUsable && ControlsManager.HasPressed_Keyboard(KeyboardActions.MENU_PRESS_1, true) ||
                Helper.KeyboardUsable && ControlsManager.HasPressed_Keyboard(KeyboardActions.MENU_PRESS_2, true))
            {
                ToggleDynamicFov();

                return;
            }

            // When playing with Gamepad
            if (Helper.GamepadUsable && ControlsManager.HasPressed_Gamepad(GamepadActions.MENU_PRESS, true))
            {
                ToggleDynamicFov();
            }
        }
    }


    public void OnPointerDown(PointerEventData _eventData)
    {
        if (Mouse.current.IsPressed(0))
        {
            IsSelected = true;
        }
    }

    public void ToggleDynamicFov()
    {
        toggle.isOn = !toggle.isOn;
        IsOn = toggle.isOn;
    }
}
