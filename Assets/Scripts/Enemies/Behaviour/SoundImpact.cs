using FMODUnity;
using UnityEngine;

public class SoundImpact : MonoBehaviour
{
    [SerializeField] private EventReference _impactSound;
    private SoundHandle m_sound;

    private void Start()
    {
        m_sound = new SoundHandle(_impactSound, FindAnyObjectByType<AudioManager>());
    }

    private void Update()
    {
        m_sound.Spatialization(gameObject);
    }

    public void PlaySound()
    {
        m_sound.StartSound();
    }
}
