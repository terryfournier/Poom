using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VolumeToggle : MonoBehaviour
{
    [SerializeField] private VolumeType volumeType = 0;

    private Toggle toggle = null;
    private AudioManager audioManager = null;
    private int masterCounter = 0;
    private int musicCounter = 0;
    private int soundCounter = 0;
    private bool justRetrievedAudioManager = false;

    // Awake is called once before all other methods, when the GameObject is created
    private void Awake()
    {
        toggle = GetComponent<Toggle>();

        StartCoroutine(RetrieveAudioManager());
    }

    // Update is called once per frame
    private void Update()
    {
        if (justRetrievedAudioManager)
        {
            StopCoroutine(RetrieveAudioManager());
            justRetrievedAudioManager = false;
        }
    }


    public void OnClick()
    {
        if (!audioManager) { return; }

        switch (volumeType)
        {
            case VolumeType.MASTER: { audioManager.ToggleMaster(); } break;
            case VolumeType.MUSIC: { audioManager.ToggleMusic(); } break;
            case VolumeType.SOUND: { audioManager.ToggleSound(); } break;
        }
    }


    private IEnumerator RetrieveAudioManager()
    {
        while (!audioManager)
        {
            audioManager = FindAnyObjectByType<AudioManager>();

            if (audioManager) { justRetrievedAudioManager = true; }
            yield return null;
        }
    }
}
