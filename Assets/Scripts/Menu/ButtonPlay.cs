using System;
using UnityEngine;

public class ButtonPlay : MonoBehaviour
{
    [SerializeField] GameObject menuParent;
    public Action Onclick;

    private AudioManager audioManager = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    public void LaunchGame()
    {
        if (audioManager)
        {
            audioManager.StopEverything();
        }

        Onclick?.Invoke();
        menuParent.SetActive(false);
    }
}
