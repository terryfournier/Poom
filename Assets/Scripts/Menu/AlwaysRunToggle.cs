using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AlwaysRunToggle : MonoBehaviour
{
    public static bool IsOn = false;

    private PlayerController playerController = null;
    private bool justFetchedPlayerController = false;

    // Awake is called once before all other methods, when the GameObject is created
    private void Awake()
    {
        Toggle toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(ToggleSprintMode);

        IsOn = toggle.isOn;

        StartCoroutine(FetchPlayerController());
    }

    // Update is called once per frame
    private void Update()
    {
        if (justFetchedPlayerController)
        {
            StopCoroutine(FetchPlayerController());
            justFetchedPlayerController = false;
        }
    }


    private void ToggleSprintMode(bool _arg0)
    {
        IsOn = !IsOn;
        playerController.ToggleIsSprinting();
    }


    private IEnumerator FetchPlayerController()
    {
        while (!playerController)
        {
            playerController = FindAnyObjectByType<PlayerController>();
            yield return null;
        }

        justFetchedPlayerController = true;
    }
}
