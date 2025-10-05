using System.Collections.Generic;

using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    //=======//
    // ENUM //
    //=====//

    public enum Multiplier
    {
        NONE = 4,
        BLOODSHED = 5,
        HECATOMB = 6,
        SLAUGHTER = 7,
        EXTERMINATION = 8,
        RAMPAGE = 9
    };

     // Multiplier
     Multiplier mFactor;

    // Text
    TMP_Text m_text;

    // Image
    [SerializeField]
    Image m_multOutline;
    [SerializeField]
    Image m_multBack;


    // Font
    [SerializeField] TMP_FontAsset m_font;


    // float
    float m_score;
    List<float> m_scoreToReach = null;
    float m_lastScoretoReach;
    float timerMultiplier;
    float timerUpScore;
    [SerializeField] float maxTimerUpScore;
    [SerializeField] float multiplierReset;
    int nbAddBeforeReset;

    //Text
    List<GameObject> m_bufferTextToAdd = null;

    // Vector3
    List<Vector3> m_positionBuffer = null;

    //GameObject
    [SerializeField] GameObject m_anchor;

    // bool 
    bool m_scoreIncremented;

    #region Multiplier Sprite
    // All The mult and Out
    // None
    [Header("Multiplier Sprite")]
    [SerializeField]
    Sprite m_NoneOut;
    [SerializeField]
    Sprite m_NoneBack;
    // BloodShed
    [SerializeField]
    Sprite m_BloodShedOut;
    [SerializeField]
    Sprite m_BloodShedBack;
    // Hecatomb
    [SerializeField]
    Sprite m_HecatombOut;
    [SerializeField]
    Sprite m_HecatombBack;
    // Slaughter
    [SerializeField]
    Sprite m_SlaughterOut;
    [SerializeField]
    Sprite m_SlaughterBack;
    // Extermination
    [SerializeField]
    Sprite m_ExterminationOut;
    [SerializeField]
    Sprite m_ExterminationBack;
    // Rampage
    [SerializeField]
    Sprite m_RampageOut;
    [SerializeField]
    Sprite m_RampageBack;
    #endregion

    private void Start()
    {
        m_text = GetComponent<TMP_Text>();
        m_score = 0;
        timerMultiplier = 0;
        m_lastScoretoReach = m_score;
        m_scoreToReach = new List<float>();
        m_bufferTextToAdd = new List<GameObject>();
        m_positionBuffer = new List<Vector3>();
        m_scoreIncremented = false;
        nbAddBeforeReset = 0;
        timerUpScore = 0;
    }

    //the function to call to add something to the score
    public void AddScore(float _scoreToAdd)
    {
        // Reset the multiplier timer
        timerMultiplier = 0;

        float toAddAMultiplier = ManageMultiplier(_scoreToAdd);

        // usefull for the lerp
        m_scoreToReach.Add(toAddAMultiplier + m_lastScoretoReach);

        // usefull to know our new score reach
        m_lastScoretoReach = toAddAMultiplier + m_lastScoretoReach;


        GameObject textTemp = new GameObject("buffer text" + m_bufferTextToAdd.Count);
        textTemp.transform.position = m_anchor.transform.position - new Vector3( 0, 30 * m_bufferTextToAdd.Count, 0);
        TMP_Text toModify = textTemp.AddComponent<TextMeshProUGUI>();
        toModify.text = "+ " + toAddAMultiplier.ToString();
        toModify.font = m_font;
        toModify.fontSize = 36;
        toModify.color = Color.red;
        textTemp.transform.SetParent(m_anchor.transform);


        // adding to the list of floating text to make a cool movement
        m_bufferTextToAdd.Add(textTemp);

        if (m_bufferTextToAdd.Count >= 5)
        {
            toModify.alpha = 0;
        }

        // always have all the positions we need
        if (m_positionBuffer.Count < m_bufferTextToAdd.Count)
        {
            m_positionBuffer.Add(textTemp.transform.position);
        }
    }

    private void Update()
    {
        timerMultiplier += Time.deltaTime;
        timerUpScore += Time.deltaTime;

        if (timerMultiplier >= multiplierReset)
        {
            m_multBack.sprite = m_NoneBack;
            m_multOutline.sprite = m_NoneOut;
            m_multBack.fillAmount = 1;
            nbAddBeforeReset = 0;
        }
        else
        {
            m_multBack.fillAmount = 1 - (timerMultiplier / multiplierReset);
        }

        if (m_scoreIncremented)
        {
            if(ManageToAddFloatingTextBox(timerUpScore))
            {
                Destroy(m_bufferTextToAdd[0]);
                m_bufferTextToAdd.RemoveAt(0);
            }
        }
        if (m_scoreToReach.Count > 0)
        {
            
            m_score = Mathf.Lerp(Mathf.Round(m_score), Mathf.Round(m_scoreToReach[0]),(timerUpScore / maxTimerUpScore) *  m_bufferTextToAdd.Count);


            // if we reach the score we want we need to floor due to score
            if (Mathf.Round(m_score) == Mathf.Round(m_scoreToReach[0]))
            {
                m_scoreIncremented = true;
                m_text.color = Color.white;
                m_score = m_scoreToReach[0];
                m_scoreToReach.RemoveAt(0);
                timerUpScore = 0;
            }
            else
            {
                // fade to red
                m_text.color = Color.Lerp(m_text.color, Color.red, (timerUpScore / maxTimerUpScore) * m_bufferTextToAdd.Count);
            }

            m_text.text = ((int)m_score).ToString();
        }

        if (Helper.CurrentKeyboard.lKey.IsPressed())
        {
            int temp = Random.Range(10, 150);
            AddScore(temp);
        }
    }

    // Manage all the List of floating above the score, when delete them, move them ect ...
    bool ManageToAddFloatingTextBox(float _timerUpScore)
    {
        if(m_bufferTextToAdd.Count > 0)
        {
            for (int i = 0; i < m_bufferTextToAdd.Count; i++)
            {
                if (i == 0)
                {
                    //modify the alpha of the first element to make him disepear
                    m_bufferTextToAdd[i].GetComponent<TMP_Text>().alpha = Mathf.Lerp(m_bufferTextToAdd[i].GetComponent<TMP_Text>().alpha, 0, (_timerUpScore / maxTimerUpScore) * m_bufferTextToAdd.Count);
                }
                else
                {
                    //make all the element go to the top except the first one 
                    m_bufferTextToAdd[i].transform.position = Vector3.Lerp(m_bufferTextToAdd[i].transform.position, m_positionBuffer[i - 1], (_timerUpScore / maxTimerUpScore) * m_bufferTextToAdd.Count);
                    if(i >= 5)
                    {
                        m_bufferTextToAdd[i].GetComponent<TMP_Text>().alpha = 0;
                    }
                    else
                    {
                        m_bufferTextToAdd[i].GetComponent<TMP_Text>().alpha = 1;
                    }
                }
            }
            if (m_bufferTextToAdd[0].GetComponent<TMP_Text>().alpha <= 0.1f)
            {
                m_scoreIncremented = false;
                return true;
            }
        }
        
        return false;
    }

    float ManageMultiplier(float _scoreToAdd)
    {
        nbAddBeforeReset++;

        // Setting the multiplier
        if (nbAddBeforeReset < 6)
        {
            m_multBack.sprite = m_NoneBack;
            m_multOutline.sprite = m_NoneOut;
            mFactor = Multiplier.NONE;
        }
        else if (nbAddBeforeReset < 11)
        {
            m_multBack.sprite = m_BloodShedBack;
            m_multOutline.sprite = m_BloodShedOut;
            mFactor = Multiplier.BLOODSHED;
        }
        else if (nbAddBeforeReset < 16)
        {
            m_multBack.sprite = m_HecatombBack;
            m_multOutline.sprite = m_HecatombOut;
            mFactor = Multiplier.HECATOMB;
        }
        else if (nbAddBeforeReset < 16)
        {
            m_multBack.sprite = m_SlaughterBack;
            m_multOutline.sprite = m_SlaughterOut;
            mFactor = Multiplier.SLAUGHTER;
        }
        else if (nbAddBeforeReset < 21)
        {
            m_multBack.sprite = m_ExterminationBack;
            m_multOutline.sprite = m_ExterminationOut;
            mFactor = Multiplier.EXTERMINATION;
        }
        else
        {
            m_multBack.sprite = m_RampageBack;
            m_multOutline.sprite = m_RampageOut;
            mFactor = Multiplier.RAMPAGE;
        }

        switch (mFactor)
        {
            case Multiplier.NONE:
                return _scoreToAdd;
            case Multiplier.BLOODSHED:
                return _scoreToAdd * 1.50f;
            case Multiplier.HECATOMB:
                return _scoreToAdd * 1.75f;
            case Multiplier.SLAUGHTER:
                return _scoreToAdd * 2.0f;
            case Multiplier.EXTERMINATION:
                return _scoreToAdd * 2.25f;
            case Multiplier.RAMPAGE:
                return _scoreToAdd * 2.5f;
            default:
                return _scoreToAdd;
        }
    }

    public float GetScore()
    {
        return m_score;
    }
}
