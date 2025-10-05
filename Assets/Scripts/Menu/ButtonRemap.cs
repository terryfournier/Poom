using UnityEngine;
using UnityEngine.UI;

public class ButtonRemap : MonoBehaviour
{
    [SerializeField] private GameObject inputField = null;

    // Awake is called once before all other methods, when the GameObject is created
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(MakeFieldAppear);
    }


    private void MakeFieldAppear()
    {
        inputField.SetActive(true);
    }
}
