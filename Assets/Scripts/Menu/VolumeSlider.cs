using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class VolumeSlider : MonoBehaviour, IPointerDownHandler
{
    private static float masterValue = 0.8f;
    private static float musicValue = 0.4f;
    private static float soundValue = 0.45f;

    public static float MasterValueCopy = 0f;
    public static float MusicValueCopy = 0f;
    public static float SoundValueCopy = 0f;

    public static float MasterValue
    {
        get { return masterValue; }
    }
    public static float MusicValue
    {
        get { return musicValue; }
    }
    public static float SoundValue
    {
        get { return soundValue; }
    }

    [SerializeField] private TextMeshProUGUI volumeValue = null;
    [SerializeField] private VolumeType volumeType = 0;

    private Slider slider = null;
    private float value = 0f;

    [HideInInspector] public bool IsSelected = false;

    public VolumeType Type
    {
        get => volumeType;
    }

    // Awake is called once before all other methods, when the GameObject is created
    private void Awake()
    {
        slider = GetComponent<Slider>();

        InitVolume();
        UpdateVolumeText();
        UpdateCopyValues();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        transform.parent = transform.parent.parent;
    }

    // Update is called once per frame
    private void Update()
    {
        switch (volumeType)
        {
            case VolumeType.MASTER: { value = MasterValue; } break;
            case VolumeType.MUSIC: { value = MusicValue; } break;
            case VolumeType.SOUND: { value = SoundValue; } break;
        }

        if (value != slider.value)
        {
            UpdateVolumeText();
            UpdateCopyValues();
        }

        AdjustVolume();
    }


    public void OnPointerDown(PointerEventData _eventData)
    {
        if (Helper.MouseUsable && !Helper.GamepadUsable)
        {
            if (ControlsManager.HasPressed_Mouse(MouseActions.MENU_PRESS, true))
            {
                IsSelected = true;
            }
        }
    }


    private void InitVolume()
    {
        switch (volumeType)
        {
            case VolumeType.MASTER: { slider.value = masterValue; } break;
            case VolumeType.MUSIC: { slider.value = musicValue; } break;
            case VolumeType.SOUND: { slider.value = soundValue; } break;
        }
    }

    private void UpdateVolumeText()
    {
        volumeValue.text = ((int)(slider.value * 100f)).ToString();
    }


    public void AdjustVolume()
    {
        switch (volumeType)
        {
            case VolumeType.MASTER: { masterValue = slider.value; } break;
            case VolumeType.MUSIC: { musicValue = slider.value; } break;
            case VolumeType.SOUND: { soundValue = slider.value; } break;
        }
    }

    public void SetValue(float _value)
    {
        _value = Mathf.Clamp(_value, slider.minValue, slider.maxValue);

        slider.value = _value;

        UpdateVolumeText();
    }

    public void UpdateCopyValues()
    {
        switch (volumeType)
        {
            case VolumeType.MASTER: { MasterValueCopy = slider.value; } break;
            case VolumeType.MUSIC: { MusicValueCopy = slider.value; } break;
            case VolumeType.SOUND: { SoundValueCopy = slider.value; } break;
        }
    }
}
