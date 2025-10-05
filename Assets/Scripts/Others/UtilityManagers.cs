using UnityEngine;
using UnityEngine.SceneManagement;

public class UtilityManagers : MonoBehaviour
{
    [SerializeField] private GameObject pauseManagerPrefab = null;
    [SerializeField] private GameObject mouseManagerPrefab = null;
    [SerializeField] private GameObject gameKeysManagerPrefab = null;
    [SerializeField] private GameObject audioManagerPrefab = null;
    [SerializeField] private GameObject EnemyLoaderPrefab = null;

    // Awake is called once before all other methods, when the GameObject is created
    private void Awake()
    {
        string activeSceneName = SceneManager.GetActiveScene().name;
        bool inPlayableScene =
            (!string.Equals(activeSceneName, "Menu") &&
             !string.Equals(activeSceneName, "Load") &&
             !string.Equals(activeSceneName, "GameOver"));

        // If not in Menu, Load or Game Over
        if (inPlayableScene)
        {
            PlaceOrReplace("PauseManager");
        }

        PlaceOrReplace("Mouse Manager");
        PlaceOrReplace("Game Keys Manager");
        PlaceOrReplace("Audio Manager");
        PlaceOrReplace("EnemyLoader");
    }

#if UNITY_EDITOR
    // Update is called once per frame
    private void Update()
    {
        // Reload scene (for debugging only)
        if (Helper.CurrentKeyboard.backspaceKey.wasReleasedThisFrame)
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }

        // Toggle Left-hand mode (for debugging only)
        if (Helper.CurrentKeyboard.insertKey.wasReleasedThisFrame)
        {
            Helper.ToggleBool(ref LeftHandModeToggle.IsOn);
        }
    }
#endif


    private void PlaceOrReplace(in string _prefabName)
    {
        if (Helper.PrefabExists(_prefabName))
        {
            GameObject instance = Helper.GetInstance(_prefabName);
            Destroy(instance);
        }

        if (_prefabName == "PauseManager") { Instantiate(pauseManagerPrefab, transform); return; }
        if (_prefabName == "Mouse Manager") { Instantiate(mouseManagerPrefab, transform); return; }
        if (_prefabName == "Game Keys Manager") { Instantiate(gameKeysManagerPrefab, transform); return; }
        if (_prefabName == "Audio Manager") { Instantiate(audioManagerPrefab, transform); return; }
        if (_prefabName == "EnemyLoader") { Instantiate(EnemyLoaderPrefab, transform); return; }
    }
}
