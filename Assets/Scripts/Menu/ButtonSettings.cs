using UnityEngine;
using UnityEngine.UI;

public class ButtonSettings : MonoBehaviour
{
    [SerializeField] private GameObject settings = null;

    // Awake is called once before all other methods, when the GameObject is created
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(ToggleSettings);
    }


    private void ToggleSettings()
    {
        settings.SetActive(true);
        transform.parent.gameObject.SetActive(false);
    }
}
