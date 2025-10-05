using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;
using FMOD.Studio;
using Unity.VisualScripting;

public enum VolumeType
{
    MASTER,
    MUSIC,
    SOUND,

    TYPE_NB
}

public class AudioManager : MonoBehaviour
{
    #region Members
    #region [SerializeField]
    [SerializeField] private EventReference sliderPressReference = new EventReference();
    #endregion

    #region private
    private PauseManager pauseManager = null;
    private Dictionary<string, EventInstance> musics = new Dictionary<string, EventInstance>();
    private Dictionary<string, EventInstance> sounds = new Dictionary<string, EventInstance>();
    private Dictionary<string, float> musicVolumesDict = new Dictionary<string, float>();
    private Dictionary<string, float> soundVolumesDict = new Dictionary<string, float>();
    private List<string> musicNames = new List<string>();
    private List<string> soundNames = new List<string>();
    private VolumeSlider masterSlider = null;
    private VolumeSlider musicSlider = null;
    private VolumeSlider soundSlider = null;
    private EventInstance sliderPressInstance = new EventInstance();
    private float masterVolume = 0f;
    private float musicVolume = 0f;
    private float soundVolume = 0f;
    private float masterVolumeCopy = 0f;
    private float musicVolumeCopy = 0f;
    private float soundVolumeCopy = 0f;
    private float updateTimer = 0f;
    private bool thereIsMusic = false;
    private bool thereIsSound = false;
    private bool hasRetrievedPauseManager = false;
    private bool hasReadjustedMasterVolume = true;
    private bool hasStarted = false;
    private bool hasMasterBeenToggled = false;
    private bool haveMusicsBeenToggled = false;
    private bool haveSoundsBeenToggled = false;
    private bool justRetrievedSliders = false;
    private bool isInMenu = false;
    private bool hasAdaptedForPause = false;
    #endregion
    #endregion

    #region Methods
    #region Unity
    // Awake is called once before all other methods, when the GameObject is created
    private void Awake()
    {
        StartCoroutine(RetrieveSliders());

        isInMenu = (string.Equals(SceneManager.GetActiveScene().name, "Menu"));

        if (!HasSound("SliderPress"))
        {
            sliderPressInstance = FMODUnity.RuntimeManager.CreateInstance(sliderPressReference);
            sliderPressInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(Camera.main.gameObject));

            AddSound(sliderPressReference, sliderPressInstance);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // Stop slider retrieval coroutine
        if (justRetrievedSliders)
        {
            StopCoroutine(RetrieveSliders());

            justRetrievedSliders = false;
        }

        // Stop every audio
        // Fetch menu music
        if (!hasStarted)
        {
            StopEverything();
            InitMusics();
            hasStarted = true;
        }

        // Make sure to retrieve Pause Manager only once
        if (!hasRetrievedPauseManager)
        {
            pauseManager = FindAnyObjectByType<PauseManager>();
            hasRetrievedPauseManager = true;
        }

        if (thereIsMusic || thereIsSound)
        {
            masterVolume = VolumeSlider.MasterValue;
            musicVolume = VolumeSlider.MusicValue;
            soundVolume = VolumeSlider.SoundValue;

            sliderPressInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(Camera.main.gameObject));

            // Adjust Master volume only when it changed
            if (masterVolumeCopy != masterVolume)
            {
                AdjustMasterVolume();
                masterVolumeCopy = masterVolume;
            }

            // Adjust Music volume only if there is music and when it changed
            if (thereIsMusic &&
                musicVolumeCopy != musicVolume)
            {
                AdjustMusicVolume();
                musicVolumeCopy = musicVolume;
            }

            // Adjust Sound volume only if there is sound and when it changed
            if (thereIsSound &&
                soundVolumeCopy != soundVolume)
            {
                AdjustSoundVolume();
                soundVolumeCopy = soundVolume;
            }
        }

        // When in Menu or in Pause
        if (isInMenu || (pauseManager && pauseManager.isGamePaused))
        {
            // When playing with Gamepad
            if (Helper.GamepadUsable)
            {
                if (ControlsManager.HasPressed_Gamepad(GamepadActions.MUTE_ALL, false))
                {
                    ToggleEverything();
                }
            }
            // When playing with Keyboard
            else if (Helper.KeyboardUsable && ControlsManager.HasPressed_Keyboard(KeyboardActions.MUTE_ALL, false))
            {
                ToggleEverything();
            }
        }

        // When pausing the game
        if (pauseManager && pauseManager.isGamePaused)
        {
            // Make sure to do it only once
            if (!hasAdaptedForPause)
            {
                PauseAllSound();
                AdjustMasterVolume(0.5f);
                hasAdaptedForPause = true;
            }

            hasReadjustedMasterVolume = false;

            return;
        }

        // Allow readjusting for Pause next time we pause
        hasAdaptedForPause = false;

        // When resuming the game
        if (!hasReadjustedMasterVolume)
        {
            ResumeAllSound();
            AdjustMasterVolume();
            hasReadjustedMasterVolume = true;
        }

        DetectAudioPresence();
    }
    #endregion

    #region Custom
    #region private
    private void AdjustMasterVolume(float _volume = -1f)
    {
        AdjustMusicVolume(_volume);
        AdjustSoundVolume(_volume);
    }
    private void AdjustMusicVolume(float _volume = -1f)
    {
        float volume = musicVolume * masterVolume;

        if (_volume != -1f)
        {
            if (_volume < 0f) { _volume = 0f; }
            else if (_volume > 1f) { _volume = 1f; }

            volume *= _volume;
        }

        for (int i = 0; i < musicNames.Count; i++)
        {
            musics[musicNames[i]].setVolume(volume);
        }

        hasAdaptedForPause = false;
    }
    private void AdjustSoundVolume(float _volume = -1f)
    {
        float volume = soundVolume * masterVolume;

        if (_volume != -1f)
        {
            if (_volume < 0f) { _volume = 0f; }
            else if (_volume > 1f) { _volume = 1f; }

            volume *= _volume;
        }

        for (int i = 0; i < soundNames.Count; i++)
        {
            sounds[soundNames[i]].setVolume(volume);
        }

        hasAdaptedForPause = false;
    }


    private void InitMusics()
    {
        StudioEventEmitter camEmitter = Camera.main.GetComponent<StudioEventEmitter>();

        if (camEmitter)
        {
            EventReference camEvtRef = camEmitter.EventReference;
            EventInstance camEvtInstance = camEmitter.EventInstance;

            if (!camEvtRef.IsNull && !camEvtInstance.IsUnityNull())
            {
                AddMusic(camEmitter.EventReference, camEmitter.EventInstance);
            }
        }
    }

    private void DetectAudioPresence()
    {
        DetectMusicPresence();
        DetectSoundPresence();
    }
    private void DetectMusicPresence()
    {
        thereIsMusic = (musics.Count > 0);
    }
    private void DetectSoundPresence()
    {
        thereIsSound = (sounds.Count > 0);
    }
    #endregion

    #region public
    public void StartEverything()
    {
        DetectAudioPresence();

        StartAllMusic();
        StartAllSound();
    }
    public void StartAllMusic()
    {
        if (thereIsMusic)
        {
            for (int i = 0; i < musicNames.Count; i++)
            {
                musics[musicNames[i]].start();
            }
        }
    }
    public void StartAllSound()
    {
        if (thereIsSound)
        {
            for (int i = 0; i < soundNames.Count; i++)
            {
                sounds[soundNames[i]].start();
            }
        }
    }
    public void StartMusic(in string _musicName)
    {
        if (thereIsMusic && musicNames.Contains(_musicName))
        {
            musics[_musicName].start();
        }
    }
    public void StartSound(in string _soundName)
    {
        if (thereIsMusic && soundNames.Contains(_soundName))
        {
            sounds[_soundName].start();
        }
    }
    public void StartRandomMusic()
    {
        if (thereIsMusic)
        {
            musics[musicNames[Random.Range(0, musicNames.Count)]].start();
        }
    }
    public void StartRandomSound()
    {
        if (thereIsSound)
        {
            sounds[soundNames[Random.Range(0, soundNames.Count)]].start();
        }
    }

    public void StopEverything(in bool _allowFadeout = true)
    {
        DetectAudioPresence();

        StopAllMusic(_allowFadeout);
        StopAllSound(_allowFadeout);
    }
    public void StopAllMusic(in bool _allowFadeout = true)
    {
        if (thereIsMusic)
        {
            FMOD.Studio.STOP_MODE stopMode;

            for (int i = 0; i < musicNames.Count; i++)
            {
                stopMode =
                    (_allowFadeout)
                    ? FMOD.Studio.STOP_MODE.ALLOWFADEOUT
                    : FMOD.Studio.STOP_MODE.IMMEDIATE;
                musics[musicNames[i]].stop(stopMode);
            }
        }
    }
    public void StopAllSound(in bool _allowFadeout = true)
    {
        if (thereIsSound)
        {
            FMOD.Studio.STOP_MODE stopMode;

            for (int i = 0; i < soundNames.Count; i++)
            {
                stopMode =
                    (_allowFadeout)
                    ? FMOD.Studio.STOP_MODE.ALLOWFADEOUT
                    : FMOD.Studio.STOP_MODE.IMMEDIATE;
                sounds[soundNames[i]].stop(stopMode);
            }
        }
    }
    public void StopMusic(in string _musicName, in bool _allowFadeout = true)
    {
        if (thereIsMusic && musicNames.Contains(_musicName))
        {
            FMOD.Studio.STOP_MODE stopMode;

            stopMode =
                (_allowFadeout)
                ? FMOD.Studio.STOP_MODE.ALLOWFADEOUT
                : FMOD.Studio.STOP_MODE.IMMEDIATE;
            musics[_musicName].stop(stopMode);
        }
    }
    public void StopSound(in string _soundName, in bool _allowFadeout = true)
    {
        if (thereIsMusic && soundNames.Contains(_soundName))
        {
            FMOD.Studio.STOP_MODE stopMode;

            stopMode =
                (_allowFadeout)
                ? FMOD.Studio.STOP_MODE.ALLOWFADEOUT
                : FMOD.Studio.STOP_MODE.IMMEDIATE;
            sounds[_soundName].stop(stopMode);
        }
    }

    public void RestartEverything(in bool _allowFadeout = true)
    {
        DetectAudioPresence();

        RestartAllMusic(_allowFadeout);
        RestartAllSound(_allowFadeout);
    }
    public void RestartAllMusic(in bool _allowFadeout = true)
    {
        if (thereIsMusic)
        {
            StopAllMusic(_allowFadeout);
            StartAllMusic();
        }
    }
    public void RestartAllSound(in bool _allowFadeout = true)
    {
        if (thereIsSound)
        {
            StopAllSound(_allowFadeout);
            StartAllSound();
        }
    }
    public void RestartMusic(in string _musicName, in bool _allowFadeout = true)
    {
        if (thereIsMusic && musicNames.Contains(_musicName))
        {
            StopMusic(_musicName, _allowFadeout);
            StartMusic(_musicName);
        }
    }
    public void RestartSound(in string _soundName, in bool _allowFadeout = true)
    {
        if (thereIsMusic && soundNames.Contains(_soundName))
        {
            StopSound(_soundName, _allowFadeout);
            StartSound(_soundName);
        }
    }
    public void RestartRandomMusic()
    {
        if (thereIsMusic)
        {
            RestartMusic(musicNames[Random.Range(0, musicNames.Count)]);
        }
    }
    public void RestartRandomSound()
    {
        if (thereIsSound)
        {
            RestartSound(soundNames[Random.Range(0, soundNames.Count)]);
        }
    }

    public void PauseEverything()
    {
        DetectAudioPresence();

        PauseAllMusic();
        PauseAllSound();
    }
    public void PauseAllMusic()
    {
        if (thereIsMusic)
        {
            for (int i = 0; i < musicNames.Count; i++)
            {
                musics[musicNames[i]].setPaused(true);
            }
        }
    }
    public void PauseAllSound()
    {
        if (thereIsSound)
        {
            for (int i = 0; i < soundNames.Count; i++)
            {
                sounds[soundNames[i]].setPaused(true);
            }
        }
    }
    public void PauseAllActiveMusic()
    {
        if (thereIsMusic)
        {
            string musicName = "";

            for (int i = 0; i < musicNames.Count; i++)
            {
                musicName = musicNames[i];

                musics[musicName].getPlaybackState(out PLAYBACK_STATE state);

                if (state == PLAYBACK_STATE.STARTING || state == PLAYBACK_STATE.PLAYING)
                {
                    musics[musicName].setPaused(true);
                }
            }
        }
    }
    public void PauseAllActiveSound()
    {
        if (thereIsSound)
        {
            string soundName = "";

            for (int i = 0; i < soundNames.Count; i++)
            {
                soundName = soundNames[i];

                sounds[soundName].getPlaybackState(out PLAYBACK_STATE state);

                if (state == PLAYBACK_STATE.STARTING || state == PLAYBACK_STATE.PLAYING)
                {
                    sounds[soundName].setPaused(true);
                }
            }
        }
    }
    public void PauseMusic(in string _musicName)
    {
        if (thereIsMusic && musicNames.Contains(_musicName))
        {
            musics[_musicName].setPaused(true);
        }
    }
    public void PauseSound(in string _soundName)
    {
        if (thereIsMusic && soundNames.Contains(_soundName))
        {
            sounds[_soundName].setPaused(true);
        }
    }

    public void ResumeEverything()
    {
        DetectAudioPresence();

        ResumeAllMusic();
        ResumeAllSound();
    }
    public void ResumeAllMusic()
    {
        if (thereIsMusic)
        {
            for (int i = 0; i < musicNames.Count; i++)
            {
                musics[musicNames[i]].setPaused(false);
            }
        }
    }
    public void ResumeAllSound()
    {
        if (thereIsSound)
        {
            for (int i = 0; i < soundNames.Count; i++)
            {
                sounds[soundNames[i]].setPaused(false);
            }
        }
    }
    public void ResumeAllActiveMusic()
    {
        if (thereIsMusic)
        {
            string musicName = "";

            for (int i = 0; i < musicNames.Count; i++)
            {
                musicName = musicNames[i];

                musics[musicName].getPaused(out bool isPaused);

                if (isPaused)
                {
                    musics[musicName].setPaused(false);
                }
            }
        }
    }
    public void ResumeAllActiveSound()
    {
        if (thereIsMusic)
        {
            string soundName = "";

            for (int i = 0; i < soundNames.Count; i++)
            {
                soundName = soundNames[i];

                sounds[soundName].getPaused(out bool isPaused);

                if (isPaused)
                {
                    sounds[soundName].setPaused(false);
                }
            }
        }
    }
    public void ResumeMusic(in string _musicName)
    {
        if (thereIsMusic && musicNames.Contains(_musicName))
        {
            musics[_musicName].setPaused(false);
        }
    }
    public void ResumeSound(in string _soundName)
    {
        if (thereIsMusic && soundNames.Contains(_soundName))
        {
            sounds[_soundName].setPaused(false);
        }
    }

    public void AddAudio(in EventReference _audioReference, in EventInstance _audioInstance, in bool _isMusic)
    {
        if (_isMusic)
        {
            AddMusic(_audioReference, _audioInstance);
            return;
        }

        AddSound(_audioReference, _audioInstance);
    }
    public void AddMusic(in EventReference _musicReference, in EventInstance _musicInstance)
    {
        string musicName = Helper.GetFmodEventInstanceGuid(_musicInstance);

        foreach (KeyValuePair<string, EventInstance> music in musics)
        {
            if (string.Equals(musicName, Helper.GetFmodEventInstanceGuid(music.Value)))
            {
                _musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                RemoveMusic(_musicInstance);
                return;
            }
        }

        StreamReader sr = new StreamReader(Application.streamingAssetsPath + "/GUIDs.txt");

        string line = "music";

        while (!line.Contains(musicName))
        {
            line = sr.ReadLine();
        }

        musicName = line.Remove(0, (musicName + " event:/").Length);

        float musicVolume = 0f;

        musicNames.Add(musicName);
        musics.Add(musicName, _musicInstance);

        musics[musicName].getVolume(out musicVolume);
        musicVolumesDict[musicName] = musicVolume;
    }
    public void AddSound(in EventReference _soundReference, in EventInstance _soundInstance)
    {
        string soundName = Helper.GetFmodEventInstanceGuid(_soundInstance);

        foreach (KeyValuePair<string, EventInstance> sound in sounds)
        {
            if (string.Equals(soundName, Helper.GetFmodEventInstanceGuid(sound.Value)))
            {
                _soundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                RemoveSound(_soundInstance);
                return;
            }
        }

        StreamReader sr = new StreamReader(Application.streamingAssetsPath + "/GUIDs.txt");

        string line = "sound";

        while (!line.Contains(soundName))
        {
            line = sr.ReadLine();
        }

        soundName = line.Remove(0, (soundName + " event:/").Length);

        float soundVolume = 0f;

        soundNames.Add(soundName);
        sounds.Add(soundName, _soundInstance);

        sounds[soundName].getVolume(out soundVolume);
        soundVolumesDict[soundName] = soundVolume;
    }

    public void RemoveAudio(in EventInstance _audioInstance, in bool _isMusic)
    {
        if (_isMusic)
        {
            RemoveMusic(_audioInstance);
            return;
        }

        RemoveSound(_audioInstance);
    }
    public void RemoveMusic(in EventInstance _musicInstance)
    {
        string musicName = Helper.GetFmodEventInstanceGuid(_musicInstance);

        musicNames.Remove(musicName);
        musics.Remove(musicName);

        musicVolumesDict[musicName] = 0f;
    }
    public void RemoveSound(in EventInstance _soundInstance)
    {
        string soundName = Helper.GetFmodEventInstanceGuid(_soundInstance);

        soundNames.Remove(soundName);
        sounds.Remove(soundName);

        soundVolumesDict[soundName] = 0f;
    }

    public void SetMusicVolume(float _volume)
    {
        _volume = Mathf.Clamp01(_volume);

        for (int i = 0; i < musics.Count; i++)
        {
            musics[musicNames[i]].setVolume(_volume);
        }
    }
    public void SetSoundVolume(float _volume)
    {
        _volume = Mathf.Clamp01(_volume);

        for (int i = 0; i < sounds.Count; i++)
        {
            sounds[soundNames[i]].setVolume(_volume);
        }
    }

    public void SetVolumeOfMusic(in string _musicName, float _volume)
    {
        if (!musicNames.Contains(_musicName))
        {
            Helper.ErrorColour(Colour.WHITE, "SetVolumeOfMusic: Music \"" + _musicName + "\" doesn't exist.");
            return;
        }

        _volume = Mathf.Clamp01(_volume);

        musics[_musicName].setVolume(_volume);
    }
    public void SetVolumeOfSound(in string _soundName, float _volume)
    {
        if (!soundNames.Contains(_soundName))
        {
            Helper.ErrorColour(Colour.WHITE, "SetVolumeOfSound: Sound \"" + _soundName + "\" doesn't exist.");
            return;
        }

        _volume = Mathf.Clamp01(_volume);

        sounds[_soundName].setVolume(_volume);
    }

    public void ToggleMaster()
    {
        if (!masterSlider)
        {
            Helper.ErrorColour(Colour.WHITE, "(ToggleMaster) Cannot toggle volume : masterSlider is null.");
            return;
        }

        Helper.ToggleBool(ref hasMasterBeenToggled);

        masterSlider.SetValue((hasMasterBeenToggled) ? 0f : VolumeSlider.MasterValueCopy);
        masterSlider.AdjustVolume();
    }
    public void ToggleMusic()
    {
        if (!musicSlider)
        {
            Helper.ErrorColour(Colour.WHITE, "(ToggleMusics) Cannot toggle volume : musicSlider is null.");
            return;
        }

        Helper.ToggleBool(ref haveMusicsBeenToggled);

        musicSlider.SetValue((haveMusicsBeenToggled) ? 0f : VolumeSlider.MusicValueCopy);
        musicSlider.AdjustVolume();
    }
    public void ToggleSound()
    {
        if (!soundSlider)
        {
            Helper.ErrorColour(Colour.WHITE, "(ToggleSounds) Cannot toggle volume : soundSlider is null.");
            return;
        }

        Helper.ToggleBool(ref haveSoundsBeenToggled);

        soundSlider.SetValue((haveSoundsBeenToggled) ? 0f : VolumeSlider.SoundValueCopy);
        soundSlider.AdjustVolume();
    }
    public void ToggleEverything()
    {
        ToggleMaster();
        ToggleMusic();
        ToggleSound();
    }

    public void GetMusic(in string _musicName, out EventInstance _music)
    {
        if (!musicNames.Contains(_musicName) || musics[_musicName].IsUnityNull())
        {
            Helper.ErrorColour(Colour.WHITE, "(GetMusic) : Music " + _musicName + " doesn't exist.");

            _music = new EventInstance();

            return;
        }

        _music = musics[_musicName];
    }
    public void GetSound(in string _soundName, out EventInstance _sound)
    {
        if (!soundNames.Contains(_soundName) || sounds[_soundName].IsUnityNull())
        {
            Helper.ErrorColour(Colour.WHITE, "(GetSound) : Sound " + _soundName + " doesn't exist.");

            _sound = new EventInstance();

            return;
        }

        _sound = sounds[_soundName];
    }

    public bool HasMusic(in string _musicName)
    {
        return (musicNames.Contains(_musicName) && !musics[_musicName].IsUnityNull());
    }
    public bool HasSound(in string _soundName)
    {
        return (soundNames.Contains(_soundName) && !sounds[_soundName].IsUnityNull());
    }
    #endregion
    #endregion

    #region Coroutines
    private IEnumerator RetrieveSliders()
    {
        GameObject volumeSliderObj = null;

        while (!justRetrievedSliders)
        {
            if (!masterSlider)
            {
                volumeSliderObj = GameObject.Find("Master Volume Slider");

                if (volumeSliderObj) { masterSlider = volumeSliderObj.GetComponent<VolumeSlider>(); }
            }

            if (!musicSlider)
            {
                volumeSliderObj = GameObject.Find("Music Volume Slider");

                if (volumeSliderObj) { musicSlider = volumeSliderObj.GetComponent<VolumeSlider>(); }
            }

            if (!soundSlider)
            {
                volumeSliderObj = GameObject.Find("Sound Volume Slider");

                if (volumeSliderObj) { soundSlider = volumeSliderObj.GetComponent<VolumeSlider>(); }
            }

            if (masterSlider && musicSlider && soundSlider) { justRetrievedSliders = true; }

            yield return null;
        }
    }
    #endregion
    #endregion
}
