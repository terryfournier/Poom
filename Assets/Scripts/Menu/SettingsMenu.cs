using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public static bool IsOn = false;

    private static bool deactivate = false;
    private static bool canDeactivate = false;

    [SerializeField] private MenuButtonSelection[] tabs = null;

    private int tabNb = 0;
    private int index = 0;
    private int indexCpy = 0;

    // Update is called once per frame
    private void Update()
    {
        IsOn = gameObject.activeSelf;

        if (!canDeactivate && deactivate)
        {
            gameObject.SetActive(false);
            IsOn = false;
            canDeactivate = true;
        }

        tabNb = tabs.Length;

        if (tabNb > 0)
        {
            NavigateTabs();
        }
    }


    private void NavigateTabs()
    {
        ScrollTabs();

        if (indexCpy != index)
        {
            AdaptHighlightingAndSelectTab();
            indexCpy = index;
        }
    }

    private void ScrollTabs()
    {
        bool pressedTabLeft = false;
        bool pressedTabRight = false;

        if (Helper.GamepadUsable)
        {
            pressedTabLeft = ControlsManager.HasPressed_Gamepad(GamepadActions.MENU_TAB_LEFT, false);
            pressedTabRight = ControlsManager.HasPressed_Gamepad(GamepadActions.MENU_TAB_RIGHT, false);
        }
        else if (Helper.KeyboardUsable)
        {
            pressedTabLeft =
                (ControlsManager.HasPressed_Keyboard(KeyboardActions.MENU_TAB_LEFT_1, false) ||
                ControlsManager.HasPressed_Keyboard(KeyboardActions.MENU_TAB_LEFT_2, false));
            pressedTabRight =
                (ControlsManager.HasPressed_Keyboard(KeyboardActions.MENU_TAB_RIGHT_1, false) ||
                ControlsManager.HasPressed_Keyboard(KeyboardActions.MENU_TAB_RIGHT_2, false));
        }

        if (pressedTabLeft)
        {
            index--;
        }
        else if (pressedTabRight)
        {
            index++;
        }

        GenericButton genericButton = null;

        for (int i = 0; i < tabNb; i++)
        {
            genericButton = tabs[i].GetComponent<GenericButton>();

            if (genericButton && genericButton.WasClicked)
            {
                index = i;
                break;
            }
        }

        WrapIndexAroundLimits();

        if (pressedTabLeft || pressedTabRight)
        {
            tabs[index].GetComponentInParent<MenuButtonSelectionManager>().ResetSelection();
        }
    }

    private void WrapIndexAroundLimits()
    {
        if (index < 0) { index = tabNb - 1; }
        else if (index > tabNb - 1) { index = 0; }
    }

    private void AdaptHighlightingAndSelectTab()
    {
        bool isSelected = false;

        for (int i = 0; i < tabNb; i++)
        {
            isSelected = (i == index);

            tabs[i].IsHighlighted = isSelected;
            tabs[i].transform.GetComponentInParent<MenuButtonSelectionManager>().enabled = isSelected;
        }

        tabs[index].GetComponent<GenericButton>().OnPointerClick(null);
    }


    public static void Deactivate()
    {
        deactivate = true;
        canDeactivate = false;
    }
}
