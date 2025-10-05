using UnityEngine;
using UnityEngine.UI;

public class SyringueUi : MonoBehaviour
{
    [SerializeField]
    private Image imageWater;
    [SerializeField]
    private Sprite[] waterFill;
    [SerializeField]
    private PlayerInventory inventory;

    private void Update()
    {
        if(inventory.syringeNb == 1)
        {
            imageWater.sprite = waterFill[0];
            imageWater.color = Color.white;
        }
        else if(inventory.syringeNb == 2)
        {
            imageWater.sprite = waterFill[1];
            imageWater.color = Color.white;
        }
        else if(inventory.syringeNb == 3)
        {
            imageWater.sprite = waterFill[2];
            imageWater.color = Color.white;
        }
        else
        {
            imageWater.sprite = null;
            imageWater.color = Color.clear;
        }
    }
}
