using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SensitivitySlider : MonoBehaviour
{
    public static float Sensitivity = 1f;

    [SerializeField] TextMeshProUGUI valueText = null;

    private Slider slider = null;

    // Awake is called once before all other methods, when the GameObject is created
    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.value = Sensitivity;
    }

    public void OnValueChanged()
    {
        if (slider)
        {
            if (valueText)
            {
                valueText.text = ((int)(slider.value * 100f)).ToString();
            }

            Sensitivity = slider.value;
        }
    }
}
