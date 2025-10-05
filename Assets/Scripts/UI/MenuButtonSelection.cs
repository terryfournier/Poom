using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MenuButtonSelection : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
{
    [SerializeField] private float sliderSpeed = 0f;

    private Button button = null;
    private Slider slider = null;
    private Toggle toggle = null;
    private TextMeshProUGUI text = null;
    private ControlsManager controlsManager = null;
    private AudioManager audioManager = null;
    private VolumeSlider volumeSlider = null;
    private Color lightGray = new Color(0.75f, 0.75f, 0.75f, 1f);
    private string sliderSoundName = "";

    [HideInInspector] public bool IsHighlighted = false;

    // Awake is called once before all other methods, when the GameObject is created
    private void Awake()
    {
        FetchComponents();

        // SliderPress sound
        sliderSoundName = "SliderPress";
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        FetchManagers();
    }

    // Update is called once per frame
    private void Update()
    {
        // When button is highlighted
        if (IsHighlighted)
        {
            ChangeColour(Color.white);

            // When playing with Gamepad
            if (Helper.GamepadUsable)
            {
                if (button)
                {
                    // Press highlighted Button
                    if (ControlsManager.HasPressed_Gamepad(GamepadActions.MENU_PRESS, false))
                    {
                        PressButton();

                        if (button.name.Contains("Settings"))
                        {
                            Helper.ToggleBool(ref SettingsMenu.IsOn);
                        }
                    }
                }
                else if (slider)
                {
                    HandleSlider_Gamepad();

                    // Mute/Unmute desired value
                    VolumeSlider volumeSlider = slider.GetComponent<VolumeSlider>();

                    if (volumeSlider && ControlsManager.HasPressed_Gamepad(GamepadActions.MUTE, false))
                    {
                        MuteUnmute(volumeSlider.Type);
                    }

                    // Play a sound to test volume
                    if (ControlsManager.HasPressed_Gamepad(GamepadActions.MENU_PRESS, false))
                    {
                        PlaySliderSound();
                    }
                }
                else if (toggle)
                {
                    // Toggle Toggle
                    if (ControlsManager.HasPressed_Gamepad(GamepadActions.MENU_PRESS, false))
                    {
                        toggle.isOn = !toggle.isOn;
                    }
                }
            }
            // When playing with Keyboard and Mouse
            else
            {
                if (Helper.KeyboardUsable)
                {
                    if (button)
                    {
                        // Press Button
                        if (ControlsManager.HasPressed_Keyboard(KeyboardActions.MENU_PRESS_1, false) ||
                            ControlsManager.HasPressed_Keyboard(KeyboardActions.MENU_PRESS_2, false))
                        {
                            PressButton();

                            if (button.name.Contains("Settings"))
                            {
                                Helper.ToggleBool(ref SettingsMenu.IsOn);
                            }

                        }
                    }
                    else if (slider)
                    {
                        HandleSlider_Keyboard();

                        // Mute/Unmute desired value
                        VolumeSlider volumeSlider = slider.GetComponent<VolumeSlider>();

                        if (volumeSlider && ControlsManager.HasPressed_Keyboard(KeyboardActions.MUTE, false))
                        {
                            MuteUnmute(volumeSlider.Type);
                        }

                        // Play a sound to test volume
                        if (ControlsManager.HasPressed_Keyboard(KeyboardActions.MENU_PRESS_1, false) ||
                            ControlsManager.HasPressed_Keyboard(KeyboardActions.MENU_PRESS_2, false))
                        {
                            PlaySliderSound();
                        }
                    }
                    else if (toggle)
                    {
                        // Toggle Toggle
                        if (ControlsManager.HasPressed_Keyboard(KeyboardActions.MENU_PRESS_1, false) ||
                            ControlsManager.HasPressed_Keyboard(KeyboardActions.MENU_PRESS_2, false))
                        {
                            toggle.isOn = !toggle.isOn;
                        }
                    }
                }

                // Update Slider value with Mouse scroll
                if (slider && Helper.MouseUsable)
                {
                    HandleSlider_Mouse();
                }
            }

            return;
        }

        // When button is not highlighted
        ChangeColour(lightGray);
    }


    public void OnPointerEnter(PointerEventData _eventData)
    {
        if (Helper.MouseUsable && !Helper.GamepadUsable)
        {
            IsHighlighted = true;
        }
    }

    public void OnPointerDown(PointerEventData _eventData)
    {
        if (Helper.MouseUsable && !Helper.GamepadUsable &&
            ControlsManager.HasPressed_Mouse(MouseActions.MENU_PRESS, false))
        {
            if (button)
            {
                PressButton();
            }
            else if (slider)
            {
                PlaySliderSound();
            }
        }
    }

    public void OnPointerExit(PointerEventData _eventData)
    {
        if (Helper.MouseUsable && !Helper.GamepadUsable)
        {
            IsHighlighted = false;
        }
    }


    private void FetchComponents()
    {
        button = GetComponent<Button>();
        slider = GetComponent<Slider>();
        toggle = GetComponent<Toggle>();

        for (int i = 0; i < transform.childCount; i++)
        {
            if (text) { break; }

            text = transform.GetChild(i).GetComponent<TextMeshProUGUI>();
        }

        if (slider)
        {
            volumeSlider = GetComponent<VolumeSlider>();
        }
    }

    private void FetchManagers()
    {
        controlsManager = GameObject.Find("Game Keys Manager(Clone)").GetComponent<ControlsManager>();

        if (slider)
        {
            audioManager = GameObject.Find("Audio Manager(Clone)").GetComponent<AudioManager>();
        }
    }

    private void ChangeColour(in Color _colour)
    {
        if (text) { text.color = _colour; }
        else if (slider) { Helper.ReplaceUIColour(slider, _colour); }
        else if (toggle) { Helper.ReplaceUIColour(toggle, _colour); }
    }


    private void MuteUnmute(in VolumeType _volumeType)
    {
        switch (_volumeType)
        {
            case VolumeType.MASTER: { audioManager.ToggleMaster(); } break;
            case VolumeType.MUSIC: { audioManager.ToggleMusic(); } break;
            case VolumeType.SOUND: { audioManager.ToggleSound(); } break;
        }
    }

    private void HandleSlider_Gamepad()
    {
        float leftStickXAxis = Helper.GetAxis_Gamepad(true, true);
        float sliderIncrement = Mathf.Abs(leftStickXAxis) * sliderSpeed;

        // When using D-Pad
        // Make Slider move at normal speed
        if (ControlsManager.HasPressed_Gamepad(GamepadActions.MENU_LEFT_1, true) || ControlsManager.HasPressed_Gamepad(GamepadActions.MENU_RIGHT_1, true))
        {
            sliderIncrement = 1f;
        }

        // Decrease Slider value
        if (ControlsManager.HasPressed_Gamepad(GamepadActions.MENU_LEFT_1, true) ||
            ControlsManager.HasPressed_Gamepad(GamepadActions.MENU_LEFT_2, true))
        {
            slider.value -= sliderIncrement * Time.unscaledDeltaTime;
        }
        // Increase Slider value
        else if (ControlsManager.HasPressed_Gamepad(GamepadActions.MENU_RIGHT_1, true) ||
                 ControlsManager.HasPressed_Gamepad(GamepadActions.MENU_RIGHT_2, true))
        {
            slider.value += sliderIncrement * Time.unscaledDeltaTime;
        }
    }
    private void HandleSlider_Keyboard()
    {
        // Decrease Slider value
        if (ControlsManager.HasPressed_Keyboard(KeyboardActions.MENU_LEFT_1, true) ||
            ControlsManager.HasPressed_Keyboard(KeyboardActions.MENU_LEFT_2, true))
        {
            slider.value -= sliderSpeed * Time.unscaledDeltaTime;
        }
        // Increase Slider value
        else if (ControlsManager.HasPressed_Keyboard(KeyboardActions.MENU_RIGHT_1, true) ||
                 ControlsManager.HasPressed_Keyboard(KeyboardActions.MENU_RIGHT_2, true))
        {
            slider.value += sliderSpeed * Time.unscaledDeltaTime;
        }
    }
    private void HandleSlider_Mouse()
    {
        float scrollY = Helper.CurrentMouse.scroll.value.y;

        // Decrease Slider value
        if (scrollY > 0f)
        {
            slider.value += sliderSpeed * Time.deltaTime;
        }
        // Increase Slider value
        else if (scrollY < 0f)
        {
            slider.value -= sliderSpeed * Time.deltaTime;
        }
    }

    private void PlaySliderSound()
    {
        if (audioManager.HasSound(sliderSoundName))
        {
            audioManager.ResumeSound(sliderSoundName);
            audioManager.RestartSound(sliderSoundName);
        }
    }

    private void PressButton()
    {
        button.onClick?.Invoke();

        if (button.GetComponent<ButtonResume>())
        {
            PlayerController playerController = FindAnyObjectByType<PlayerController>();

            if (playerController && playerController.IsGrounded)
            {
                PlayerController.StopJump = true;
            }
        }
    }
}
