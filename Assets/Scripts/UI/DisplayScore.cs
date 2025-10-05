using TMPro;
using UnityEngine;

public class DisplayScore : MonoBehaviour
{
    [SerializeField] string levelToDisplaying;
    [SerializeField] TMP_Text textToChange;
    void Start()
    {
        textToChange.text = "HighScore = " + GameManager.instance.highScore[levelToDisplaying].ToString();
    }
}
