using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class WeaponAnimationHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject player;
    private float playerSpeed;
    private Rigidbody playerRigidbody;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerRigidbody = player.GetComponent<Rigidbody>();

    }

    //All events and values used in animation
    public void TriggerReload()
    {
        animator.SetTrigger("Reload");

    }

    public void TriggerShoot()
    {
        animator.SetTrigger("Shot");
    }

    public void TriggerMelee()
    {
        animator.SetTrigger("Melee");
    }

    private void Update()
    {
        if (playerRigidbody != null)
        {
            playerSpeed = playerRigidbody.linearVelocity.sqrMagnitude;
        }
        else
        {
            playerSpeed = 0f;
        }
        if (animator == null)
        {
            return;
        }
        animator.SetFloat("speedPlayer", playerSpeed);
    }

}
