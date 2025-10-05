using UnityEngine;

public class ButtonResume : MonoBehaviour
{
    [SerializeField] private PauseManager pauseManager = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if (!pauseManager)
        {
            pauseManager = FindAnyObjectByType<PauseManager>();
        }
    }


    public void ResumeGame()
    {
        pauseManager.Unpause();
    }
}
