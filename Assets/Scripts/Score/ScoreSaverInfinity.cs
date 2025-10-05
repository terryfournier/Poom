using UnityEngine;

public class ScoreSaverInfinity : MonoBehaviour
{
    PlayerHealthManager healthManager;
    Score score;

    private void Start()
    {
        score = FindFirstObjectByType<Score>();
        healthManager = FindFirstObjectByType<PlayerHealthManager>();
        healthManager.playerDeath += SaveScore;
    }
    // Update is called once per frame
   void SaveScore()
    {
        Debug.Log("Save score");
        if (GameManager.instance.highScore["oil platform"] <= score.GetScore())
        {
            GameManager.instance.highScore["oil platform"] = score.GetScore();
        }
    }
}
