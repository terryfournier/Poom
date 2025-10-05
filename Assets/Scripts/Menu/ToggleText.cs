using UnityEngine;
using UnityEngine.UI;

public class ToggleText : MonoBehaviour
{
    [SerializeField] private Toggle correspondingToggle = null;

    private bool isOn = false;

    public void OnValueChanged()
    {
        isOn = !isOn;
        correspondingToggle.SetIsOnWithoutNotify(isOn);
    }
}
