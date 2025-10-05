using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CorruptionFill : MonoBehaviour
{
    [SerializeField] Image fillBar;
    [SerializeField] Sprite[] animFillBar;
    float maxCorruption = 100;
    float corruptionToReach;


    bool reduceCorruption;

    [SerializeField]
    private float animationChangeImageTime;
    int indexImage;
    float cooldownAnimation;

    private PlayerHealthManager playerHealthManager = null;

    public float MaxCorruption
    {
        get { return maxCorruption; }
    }

    private void Start()
    {
        playerHealthManager = GetComponentInParent<PlayerHealthManager>();
        fillBar.transform.localScale = new Vector3(1, playerHealthManager.corruption / maxCorruption, 1);
        cooldownAnimation = 0f;
        corruptionToReach = 0f;
    }

    private void Update()
    {
        if (playerHealthManager.corruption < 0f) { playerHealthManager.corruption = 0f; }
        else if (playerHealthManager.corruption > maxCorruption) { playerHealthManager.corruption = maxCorruption; }

        if(reduceCorruption)
        {
            playerHealthManager.corruption = Mathf.Lerp(playerHealthManager.corruption, corruptionToReach - 5.0f, Time.deltaTime);

            if(playerHealthManager.corruption <= corruptionToReach)
            {
                reduceCorruption = false;
            }
        }

        fillBar.transform.localScale = new Vector3(1, playerHealthManager.corruption / maxCorruption, 1);
        fillBar.transform.position = new Vector3(fillBar.transform.position.x, (100 - playerHealthManager.corruption)/3, fillBar.transform.position.z);
        AnimationRepCorruptionBar();
    }

    public void ReduceCorruption(float _reduce)
    {
        corruptionToReach = playerHealthManager.corruption - _reduce;
        if(corruptionToReach < 0)
        {
            corruptionToReach = 0;
        }
        reduceCorruption = true;
    }


    // Hand made animation
    private void AnimationRepCorruptionBar()
    {
        cooldownAnimation += Time.deltaTime;

        if(cooldownAnimation >= animationChangeImageTime)
        {
            indexImage++;
            fillBar.sprite = animFillBar[indexImage % animFillBar.Length];
            cooldownAnimation = 0;
        }
    }
}
