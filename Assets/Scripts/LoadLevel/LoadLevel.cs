using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadLevel : MonoBehaviour
{
    [SerializeField] string LevelToLoadName;
    [SerializeField] Sprite[] tentaculeImages;
    [SerializeField] Image imageInScene;

    private int spriteCounter = 0;
    private bool justLoadedLevel = false;

    private void Start()
    {
        ButtonPlay playButton = FindAnyObjectByType<ButtonPlay>();

        if (playButton)
        {
            playButton.Onclick += LaunchCoroutine;
        }
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
        FindAnyObjectByType<ButtonPlay>().Onclick -= LaunchCoroutine;
    }
    private IEnumerator LoadLevelCorroutine()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(LevelToLoadName);
        async.allowSceneActivation = false;

        float timer = 0.0f;
        while (timer < 0.5f)
        {
            imageInScene.sprite = tentaculeImages[spriteCounter % 6];
            imageInScene.color = Color.white;
            spriteCounter++;
            yield return new WaitForSeconds(0.2f);
            timer += Time.deltaTime * 3.0f;
        }
        yield return new WaitWhile(() => async.progress < 0.9f);

        justLoadedLevel = true;
        async.allowSceneActivation = true;
    }
}
