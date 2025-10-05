using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealthManager : MonoBehaviour, IDamageable, IPoisoneable
{
    // The damage fullScreen shader
    [SerializeField] private FullScreenPassRendererFeature damageFullScreen;

    private const float FALL_TIMER_MAX = 1.5f;
    private const float FALL_SPEED = 30f;
    private const float RELOAD_TIMER_MAX = 3.5f;

    [SerializeField] private CorruptionFill corruptionBar = null;
    [SerializeField] private PlayerView playerViewScript = null;
    [SerializeField] private PlayerGroundDetector groundDetectorScript = null;
    [SerializeField] private GameObject wallDetectorObj = null;
    [SerializeField] private GameObject poisonIndicator;

    private float fallTimer = 0f;
    private float reloadTimer = 0f;
    private float timerDamageShaderDisplay = 0f;
    private float maxDurationDamageShader = 1.5f;
    private bool hasInvokedMethod = false;
    private bool canFall = false;
    private bool canTakeDamage = true;

    public Action OnCorruptedPlayer;
    public Action playerDeath;
    public float corruption = 0f;

    private float poisonScale;
    private Coroutine PoisonningCorou;

    public float Corruption
    {
        get { return corruption; }
    }

    // Awake is called once before all other methods, when the GameObject is created
    private void Awake()
    {
        OnCorruptedPlayer += GetCorrupted;
        canFall = true;
    }

    // Update is called once per frame
    private void Update()
    {
        // Debug action
        // Die instantly
        if (Helper.CurrentKeyboard.digit0Key.wasPressedThisFrame)
        {
            corruption = corruptionBar.MaxCorruption;
        }

        // Debug action
        // Toggle dieability
        if (Helper.CurrentKeyboard.digit9Key.wasPressedThisFrame)
        {
            canTakeDamage = !canTakeDamage;
        }

        if (canTakeDamage)
        {
            if (corruption < 0f) { corruption = 0f; }
            else if (corruption > corruptionBar.MaxCorruption) { corruption = corruptionBar.MaxCorruption; }

            if (corruption == corruptionBar.MaxCorruption)
            {
                if (!hasInvokedMethod)
                {
                    OnCorruptedPlayer?.Invoke();
                    hasInvokedMethod = true;
                }

                if (canFall)
                {
                    if (groundDetectorScript.IsColliding)
                    {
                        Rigidbody rb = GetComponent<Rigidbody>();
                        rb.useGravity = false;
                        rb.linearVelocity = Vector3.zero;

                        groundDetectorScript.gameObject.SetActive(false);

                        canFall = false;
                    }
                }

                if (reloadTimer < RELOAD_TIMER_MAX) { reloadTimer += Time.deltaTime; }
                else { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }
            }


            // Shader display time
            timerDamageShaderDisplay += Time.deltaTime;

            if (timerDamageShaderDisplay >= maxDurationDamageShader)
            {
                damageFullScreen.SetActive(false);
            }
        }

        //if(poisonScale > 0)
        //{
        //    poisonIndicator.SetActive(true);
        //    cooldownDamagePoison += Time.deltaTime;
        //    if(cooldownDamagePoison >= 3.0f)
        //    {
        //        corruption += poisonScale;
        //        poisonScale -= 1;
        //        cooldownDamagePoison = 0;
        //    }
        //}
        //else
        //{
        //    poisonIndicator.SetActive(false);
        //}
    }

    public void TakeDamage(float _damage)
    {
        if (canTakeDamage)
        {
            GetComponent<PlayerSoundManager>().StartSound(PlayerSound.HURT);
            corruption += _damage;
            damageFullScreen.SetActive(true);
            timerDamageShaderDisplay = 0;
        }
    }

    private void GetCorrupted()
    {
        GetComponent<PlayerController>().enabled = false;
        GetComponent<PlayerPickup>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;

        playerViewScript.enabled = false;
        wallDetectorObj.SetActive(false);

        StartCoroutine(FallCoroutine());
    }

    private IEnumerator FallCoroutine()
    {
        playerDeath?.Invoke();
        while (fallTimer < FALL_TIMER_MAX)
        {
            transform.Rotate(Vector3.right * FALL_SPEED * Time.deltaTime);
            transform.Rotate(Vector3.back * FALL_SPEED * Time.deltaTime);
            transform.Translate(Vector3.down * 0.75f / FALL_TIMER_MAX * Time.deltaTime);

            fallTimer += Time.deltaTime;

            yield return new WaitForSeconds(float.MinValue);
        }
        SceneManager.LoadScene("GameOver");
        yield return null;
    }

    public void Poison(int _poisonPower)
    {
        poisonScale = _poisonPower;

        if (PoisonningCorou == null)
            PoisonningCorou = StartCoroutine(Poisoned());
    }

    private IEnumerator Poisoned()
    {
        float coolDown = 3.0f;
        poisonIndicator.SetActive(true);

        while (poisonScale > 0)
        {
            TakeDamage(poisonScale);
            poisonScale--;
            yield return new WaitForSeconds(coolDown);
        }

        poisonIndicator.SetActive(false);
        PoisonningCorou = null;
    }
}
