using UnityEngine;
using UnityEngine.Rendering;

public class SaveEndLevel : MonoBehaviour
{
    [SerializeField] string zoneToUnlock;
    [SerializeField] string currentLevelName;
    [SerializeField] Score score;

    public void Save()
    {
        GameManager.instance.isLevelOpen[zoneToUnlock] = true;
        if(score.GetScore() >= GameManager.instance.highScore[currentLevelName])
        {
            GameManager.instance.highScore[currentLevelName] = score.GetScore();
        }
        
    }
}
