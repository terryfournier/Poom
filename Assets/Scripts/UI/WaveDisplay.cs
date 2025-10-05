using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class WaveDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private WaveSystem wave;


    private void Start()
    {
        text.text = "1";
        wave.newWave += UpdateWave;
    }

    private void UpdateWave()
    {
        text.text = wave.GetWave().ToString();
        StartCoroutine(nextWave());
    }

    private IEnumerator nextWave()
    {
        text.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        while(text.color.g >= 0.05f && text.transform.position != new Vector3(Screen.width - 200, 0, 0))
        {
            text.color = Color.Lerp(text.color, Color.red, Time.deltaTime);
            text.transform.position = Vector3.Lerp(text.transform.position, new Vector3(Screen.width - 50, Screen.height - 50, 0), Time.deltaTime);
            yield return null;
        }
        while (text.color.g <= 0.99f)
        {
            text.color = Color.Lerp(text.color, Color.white, Time.deltaTime);
            yield return null;
        }
        yield return null;
    }
}
