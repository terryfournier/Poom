using UnityEngine;
using UnityEngine.UI;

public class InvertViewToggle : MonoBehaviour
{
    [SerializeField] private bool isForVerticalView = true;

    private Toggle toggle = null;

    private static bool isOn_Vertical = false;
    private static bool isOn_Horizontal = false;

    public static bool IsOn_Vertical
    {
        get => isOn_Vertical;
    }
    public static bool IsOn_Horizontal
    {
        get => isOn_Horizontal;
    }

    // Awake is called once before all other methods, when the GameObject is created
    private void Awake()
    {
        toggle = GetComponent<Toggle>();
    }


    public void OnValueChanged()
    {
        Helper.ToggleBool(ref ((isForVerticalView) ? ref isOn_Vertical : ref isOn_Horizontal));
    }
}
