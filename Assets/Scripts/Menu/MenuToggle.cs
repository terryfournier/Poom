using System;
using UnityEngine;

public enum ToggleType
{
    DYNAMIC_FOV,
    MOTION_BLUR,
    SPEED_LINES,
    VIEW_BOBBING,
    ALWAYS_RUN,
    LEFT_HAND_MODE,

    TOGGLE_TYPE_NB
}

public class MenuToggle : MonoBehaviour
{
    [SerializeField] private ToggleType type = 0;

    private Action[] toggleBehaviour = null;

    public ToggleType Type
    {
        get => type;
    }

    // Awake is called once before all other methods, when the GameObject is created
    private void Awake()
    {
        toggleBehaviour = new Action[(int)(ToggleType.TOGGLE_TYPE_NB)];

        toggleBehaviour[0] = DynamicFoVBehaviour;
        toggleBehaviour[1] = MotionBlurBehaviour;
        toggleBehaviour[2] = SpeedLinesBehaviour;
        toggleBehaviour[3] = ViewBobbingBehaviour;
        toggleBehaviour[4] = AlwaysRunBehaviour;
        toggleBehaviour[5] = LeftHandModeBehaviour;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }


    public void OnValueChanged()
    {
        toggleBehaviour[(int)(type)]();
    }


    private void DynamicFoVBehaviour()
    {

    }
    private void MotionBlurBehaviour()
    {

    }
    private void SpeedLinesBehaviour()
    {

    }
    private void ViewBobbingBehaviour()
    {

    }
    private void AlwaysRunBehaviour()
    {

    }
    private void LeftHandModeBehaviour()
    {

    }
}
