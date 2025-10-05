using System;
using System.Collections;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadLevelBenitar : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [HideInInspector]
    public string LevelToLoadName;
    [SerializeField] Sprite[] tentaculeImages;
    [SerializeField] Image imageInScene;
    [SerializeField] GameObject canvaLoad;
    [SerializeField] private EventReference eventLoad;
    private EventInstance instanceEventLoad;

    private int spriteCounter = 0;
    private bool justLoadedLevel = false;

    StudioEventEmitter musicEmiter;

    public Action desactivateSounds;

    private void Start()
    {
        instanceEventLoad = FMODUnity.RuntimeManager.CreateInstance(eventLoad);
        //instanceEventLoad = AudioManager.Sounds["menuLoad"];
    }

    // Update is called once per frame
    private void Update()
    {
        if (justLoadedLevel)
        {
            StopCoroutine(LoadLevelCorroutine());
            justLoadedLevel = false;
        }
    }

    public void LaunchCoroutine()
    {
        StartCoroutine(LoadLevelCorroutine());
    }

    private IEnumerator LoadLevelCorroutine()
    {
        desactivateSounds?.Invoke();

        AsyncOperation async = SceneManager.LoadSceneAsync(LevelToLoadName);

        if (musicEmiter != null)
            musicEmiter.Stop();

        canvaLoad.SetActive(true);
        async.allowSceneActivation = false;

        instanceEventLoad.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));

        instanceEventLoad.start();


        float timer = 0.0f;
        while (timer < 1.0f)
        {
            imageInScene.sprite = tentaculeImages[spriteCounter % 6];
            imageInScene.color = Color.white;
            spriteCounter++;
            yield return new WaitForSeconds(0.2f);
            timer += Time.deltaTime * 3.0f;
        }
        yield return new WaitWhile(() => async.progress < 0.9f);
        instanceEventLoad.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        justLoadedLevel = true;
        async.allowSceneActivation = true;
    }
}
