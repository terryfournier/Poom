using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UseStim : MonoBehaviour
{
    private Image image;
    //private TMP_Text m_Text;
    private bool alphaManage;

    [SerializeField] PlayerHealthManager playerHealthManager;
    [SerializeField] PlayerInventory inv;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        alphaManage = false;
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerHealthManager.corruption >= 80 && inv.syringeNb > 0)
        {
            image.enabled = true;
        }
        else
        {
            image.enabled = false;
        }

        if(!alphaManage)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b,  Mathf.Lerp(image.color.a, 0, Time.deltaTime * 2)); 
            if (Mathf.Lerp(image.color.a, 0, Time.deltaTime) <= 0.05f)
            {
                alphaManage = true;
            }
        }
        else
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(image.color.a, 1, Time.deltaTime * 2));
            if (Mathf.Lerp(image.color.a, 0, Time.deltaTime) >= 0.95f)
            {
                alphaManage = false;
            }
        }
    }
}
