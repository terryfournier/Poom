using UnityEngine;

public class ButtonCredit : MonoBehaviour
{
    [SerializeField] GameObject parentMenu;
    [SerializeField] GameObject parentCredit;
    public void GoToCredit()
    {
        parentMenu.SetActive(false);
        parentCredit.SetActive(true);

        GameObject tentacule = GameObject.Find("Tentacule");

        if (tentacule)
        {
            tentacule.SetActive(false);
        }
    }
}
