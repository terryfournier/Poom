using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField] TMP_Text m_Text;

    [SerializeField] TimerInfinityMode m_TimerInfinityMode;

    // Update is called once per frame
    void Update()
    {
        int minutes = Mathf.FloorToInt(m_TimerInfinityMode.timer / 60f);
        int seconds = Mathf.FloorToInt(m_TimerInfinityMode.timer %60f);

        m_Text.text = string.Format("{00:00}:{01:00}", minutes, seconds);
    }
}
