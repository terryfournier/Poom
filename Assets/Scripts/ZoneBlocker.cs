using UnityEngine;

public class ZoneBlocker : MonoBehaviour
{
    [SerializeField] GameObject parentEnnemi;
    MeshRenderer meshRenderer;
    BoxCollider boxCollider;
    bool forcedExist;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider>();
        forcedExist = false;
    }

    private void Update()
    {
        if (!forcedExist)
        {
            if (parentEnnemi.transform.childCount <= 0)
            {
                meshRenderer.enabled = false;
                boxCollider.enabled = false;
            }
            else
            {
                meshRenderer.enabled = true;
                boxCollider.enabled = true;
            }
        }
        if (Helper.CurrentKeyboard.mKey.wasPressedThisFrame)
        {
            forcedExist = true;
            if (meshRenderer.enabled == false)
            {
                meshRenderer.enabled = true;
                boxCollider.enabled = true;
            }
            else
            {
                meshRenderer.enabled = false;
                boxCollider.enabled = false;
            }
        }


    }
}
