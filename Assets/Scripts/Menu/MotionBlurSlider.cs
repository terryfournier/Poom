using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System;

public class MotionBlurSlider : MonoBehaviour, IPointerDownHandler
{
    private const float INITIAL_VALUE = 0f;
    private const float INCREMENT = 25f;

    public static float Value
    {
        get { return value; }
    }

    private static float value = INITIAL_VALUE;

    [SerializeField] private TextMeshProUGUI motionBlurValue = null;

    private Slider slider = null;

    [HideInInspector] public bool IsSelected = false;

    // Awake is called once before all other methods, when the GameObject is created
    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.value = INITIAL_VALUE;

        value = slider.value;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        ChangeText();
    }

    // Update is called once per frame
    private void Update()
    {
        if (value != slider.value)
        {
            value = slider.value;
            ChangeText();
        }
    }

    public void OnPointerDown(PointerEventData _eventData)
    {
        if (Mouse.current.IsPressed(0))
        {
            IsSelected = true;
        }
    }


    private void ChangeText()
    {
        motionBlurValue.text = Math.Round(value, 3).ToString();
    }


    public void ChangeVolume(in bool _up)
    {
        value +=
            (_up)
            ? INCREMENT * Time.deltaTime
            : -INCREMENT * Time.deltaTime;
    }
}
