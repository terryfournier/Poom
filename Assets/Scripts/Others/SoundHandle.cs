using FMODUnity;
using UnityEngine;

public class SoundHandle
{
    public SoundHandle(EventReference _referenceSound, AudioManager _audioManager)
    {
        m_sound = _referenceSound;

        m_instance = RuntimeManager.CreateInstance(m_sound);
        _audioManager.AddSound(m_sound, m_instance);
    }

    public void Spatialization(GameObject _go)
    {
        m_instance.set3DAttributes(RuntimeUtils.To3DAttributes(_go));
    }

    public void StartSound()
    {
        m_instance.start();
    }

    public void StopSound()
    {
        m_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public bool IsRunnig()
    {
        FMOD.Studio.PLAYBACK_STATE state;
        m_instance.getPlaybackState(out state);
        return state == FMOD.Studio.PLAYBACK_STATE.PLAYING;
    }

    public float GetSoundLength()
    {
        m_instance.getDescription(out FMOD.Studio.EventDescription eventDescription);
        eventDescription.getPath(out string eventPath);
        if (eventDescription.getLength(out int lengthMS) == FMOD.RESULT.OK)
        {
            return lengthMS / 1000f;
        }
        return 0.0f;
    }

    private EventReference m_sound;
    private FMOD.Studio.EventInstance m_instance;
}
