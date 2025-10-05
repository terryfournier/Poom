using UnityEngine;
using UnityEngine.UI;

public class InvertVerticalViewToggle : MonoBehaviour
{
    private Toggle toggle = null;

    private static bool isOn = false;

    public static bool IsOn
    {
        get => isOn;
    }

    // Awake is called once before all other methods, when the GameObject is created
    private void Awake()
    {
        toggle = GetComponent<Toggle>();
    }


    public void OnValueChanged()
    {
        Helper.ToggleBool(ref isOn);
    }
}
