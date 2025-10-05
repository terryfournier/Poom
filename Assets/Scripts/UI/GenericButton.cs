using UnityEngine;
using UnityEngine.EventSystems;

public class GenericButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject[] objToToggle = null;
    [SerializeField] private GameObject[] objToActivate = null;
    [SerializeField] private GameObject[] objToDeactivate = null;
    [SerializeField] private bool isActivatedByDefault = false;

    private GameObject obj = null;
    private bool wasClicked = false;
    private bool hasResetClickState = false;

    public bool WasClicked
    {
        get => wasClicked;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if (isActivatedByDefault)
        {
            GetComponentInChildren<MenuButtonSelection>().IsHighlighted = true;
            OnPointerClick(null);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (!hasResetClickState && wasClicked)
        {
            wasClicked = false;
            hasResetClickState = true;
        }
    }

    public void OnPointerClick(PointerEventData _eventData)
    {
        if (objToToggle.Length > 0)
        {
            for (int i = 0; i < objToToggle.Length; i++)
            {
                obj = objToToggle[i];
                obj.SetActive(!obj.activeSelf);
            }
        }

        if (objToActivate.Length > 0)
        {
            for (int i = 0; i < objToActivate.Length; i++)
            {
                obj = objToActivate[i];
                obj.SetActive(true);
            }
        }

        if (objToDeactivate.Length > 0)
        {
            for (int i = 0; i < objToDeactivate.Length; i++)
            {
                obj = objToDeactivate[i];
                obj.SetActive(false);
            }
        }

        wasClicked = true;
        hasResetClickState = false;
    }
}
