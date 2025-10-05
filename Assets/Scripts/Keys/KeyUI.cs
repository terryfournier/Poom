using UnityEngine;
using UnityEngine.UI;

public class KeyUI : MonoBehaviour
{
    [SerializeField] private KeyBehaviour key;
    private Image m_image;

    private void Start()
    {
        m_image = GetComponent<Image>();

        if (key != null)
            key.OnKeyTaken += ActivKey;

    }

    private void OnDestroy()
    {
        if (key != null)
            key.OnKeyTaken -= ActivKey;
    }

    private void ActivKey()
    {
        m_image.enabled = false;
    }
}
