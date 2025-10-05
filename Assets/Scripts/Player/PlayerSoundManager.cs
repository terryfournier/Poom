using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public enum PlayerSound
{
    WALK,
    SPRINT,
    HURT,
    JUMP,
    OUT_OF_BREATH,
    TORCH,
    SLIDE,
    DASH
}

public class PlayerSoundManager : MonoBehaviour
{
    private enum ActiveMovementSound
    {
        NONE,
        WALK,
        SPRINT
    }

    [SerializeField] private EventReference walkEvent = new EventReference();
    [SerializeField] private EventReference sprintEvent = new EventReference();
    [SerializeField] private EventReference hurtEvent = new EventReference();
    [SerializeField] private EventReference jumpEvent = new EventReference();
    [SerializeField] private EventReference outOfBreathEvent = new EventReference();
    [SerializeField] private EventReference torchEvent = new EventReference();
    [SerializeField] private EventReference slideEvent = new EventReference();
    [SerializeField] private EventReference dashEvent = new EventReference();

    private List<EventInstance> allInstances = new List<EventInstance>();
    private PlayerController playerController = null;
    private AudioManager audioManager = null;
    private PauseManager pauseManager = null;
    private ActiveMovementSound previousSound = ActiveMovementSound.NONE;
    private bool walkSprintSoundsAreStopped = true;
    private bool canSwitchSound = true;
    private bool justFetchedManagers = false;

    public bool CanSwitchSound
    {
        set => canSwitchSound = value;
    }

    // Awake is called once before all other methods, when the GameObject is created
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();

        StartCoroutine(FetchManagers());
    }

    // Update is called once per frame
    private void Update()
    {
        if (justFetchedManagers)
        {
            StopCoroutine(FetchManagers());

            // Create and add all Player sounds
            CreateAndAddSound(walkEvent);
            CreateAndAddSound(sprintEvent);
            CreateAndAddSound(hurtEvent);
            CreateAndAddSound(jumpEvent);
            CreateAndAddSound(outOfBreathEvent);
            CreateAndAddSound(torchEvent);
            CreateAndAddSound(slideEvent);
            CreateAndAddSound(dashEvent);

            justFetchedManagers = false;
        }

        // When game is Not paused
        if (pauseManager && !pauseManager.isGamePaused)
        {
            Set3DAttributes();

            // When Player is moving and on ground
            if (playerController.IsMoving && playerController.IsGrounded)
            {
                PlayAppropriateSound();
                AdjustPitch();
                LoopSound();

                return;
            }

            // When not moving or in the air
            if (walkSprintSoundsAreStopped == false)
            {
                StopSounds();
                walkSprintSoundsAreStopped = true;
            }
        }
    }

    private void OnDestroy()
    {
        for (int i = allInstances.Count - 1; i >= 0; i--)
        {
            StopSound((PlayerSound)(i));
        }
    }



    private void CreateAndAddSound(in EventReference _reference)
    {
        EventInstance instance = FMODUnity.RuntimeManager.CreateInstance(_reference);

        allInstances.Add(instance);
        audioManager.AddSound(_reference, instance);
    }

    private void SwitchMovementSound()
    {
        // When Sprinting
        if (playerController.IsSprinting)
        {
            StopSound(PlayerSound.WALK);
            StartSound(PlayerSound.SPRINT);
            previousSound = ActiveMovementSound.SPRINT;

            return;
        }

        // When Walking
        StopSound(PlayerSound.SPRINT);
        StartSound(PlayerSound.WALK);
        previousSound = ActiveMovementSound.WALK;
    }


    /// <summary>
    /// Sets FMOD 3D attributes to _instance only
    /// </summary>
    /// <param name="_instance">The event instance corresponding to the desired sound</param>
    private void Set3DAttributes(in EventInstance _instance)
    {
        _instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
    }
    /// <summary>
    /// Sets FMOD 3D attributes to all event instances
    /// </summary>
    private void Set3DAttributes()
    {
        for (int i = 0; i < allInstances.Count; i++)
        {
            allInstances[i].set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        }
    }

    private void PlayAppropriateSound()
    {
        if (canSwitchSound || previousSound == ActiveMovementSound.NONE)
        {
            SwitchMovementSound();
            walkSprintSoundsAreStopped = false;
            canSwitchSound = false;
        }
    }

    private void AdjustPitch()
    {
        // When playing with Gamepad
        if (Helper.GamepadUsable)
        {
            // Adapt pitch to speed
            // based on Left Stick push value
            // Normalise it for a value between 0 and 1
            float pitch = 0.5f + 0.5f * playerController.AbsTotalSpeedValue;

            allInstances[(int)(PlayerSound.SPRINT)].setPitch(pitch);
            allInstances[(int)(PlayerSound.WALK)].setPitch(pitch);
        }
        // When playing with Keyboard and Mouse
        else
        {
            // Always have a pitch of 1
            // because there is no in-between value with a Keyboard like with a Gamepad
            allInstances[(int)(PlayerSound.SPRINT)].setPitch(1f);
            allInstances[(int)(PlayerSound.WALK)].setPitch(1f);
        }
    }

    private void LoopSound()
    {
        if (previousSound != ActiveMovementSound.NONE)
        {
            // Loop Walk sound
            if (previousSound == ActiveMovementSound.WALK && SoundEnded(PlayerSound.WALK))
            {
                RestartSound(PlayerSound.WALK);
                return;
            }

            // Loop Sprint sound
            if (previousSound == ActiveMovementSound.SPRINT && SoundEnded(PlayerSound.SPRINT))
            {
                RestartSound(PlayerSound.SPRINT);
            }
        }
    }

    private void StopSounds(in bool _allowFadeout = true)
    {
        // Stop sounds
        StopSound(PlayerSound.WALK, _allowFadeout);
        StopSound(PlayerSound.SPRINT, _allowFadeout);

        // Flag sounds as stopped
        previousSound = ActiveMovementSound.NONE;
        canSwitchSound = false;
    }


    public void StartSound(in PlayerSound _playerSound)
    {
        allInstances[(int)(_playerSound)].start();
    }
    public void StopSound(in PlayerSound _playerSound, bool _allowFadeout = true)
    {
        allInstances[(int)(_playerSound)].stop((_allowFadeout) ? FMOD.Studio.STOP_MODE.ALLOWFADEOUT : FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
    public void RestartSound(in PlayerSound _playerSound, bool _allowFadeout = true)
    {
        StopSound(_playerSound, _allowFadeout);
        StartSound(_playerSound);
    }
    public void PauseSound(in PlayerSound _playerSound)
    {
        allInstances[(int)(_playerSound)].setPaused(true);
    }
    public void ResumeSound(in PlayerSound _playerSound)
    {
        allInstances[(int)(_playerSound)].setPaused(false);
    }

    public bool SoundEnded(in PlayerSound _playerSound)
    {
        allInstances[(int)(_playerSound)].getPlaybackState(out PLAYBACK_STATE state);

        return (state == PLAYBACK_STATE.STOPPED);
    }

    public void ResetChangeState()
    {
        previousSound = ActiveMovementSound.NONE;
    }


    private IEnumerator FetchManagers()
    {
        while (!pauseManager && !audioManager)
        {
            // Fetch PauseManager
            pauseManager = FindAnyObjectByType<PauseManager>();
            // Fetch AudioManager
            audioManager = FindAnyObjectByType<AudioManager>();
            yield return null;
        }

        justFetchedManagers = true;
    }
}
