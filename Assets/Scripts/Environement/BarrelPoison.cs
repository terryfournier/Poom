using FMOD.Studio;
using UnityEngine.AI;
using UnityEngine.VFX;
using UnityEngine;
using FMODUnity;

public class BarelPoison : MonoBehaviour, IDamageable
{
    float HealthPoint = 1;
    [SerializeField] GameObject fog;
    [SerializeField] float explosionRadius = 10.0f;
    [SerializeField] float damageExplosion = 20.0f;
    [SerializeField]
    private EventReference eventExplosion;
    private EventInstance instanceEventExplosion;
    private float timerExplode;
    private VisualEffect visualEffect;
    bool explode;

    private PlayerHealthManager healthManager;
    private void Start()
    {
        healthManager = FindFirstObjectByType<PlayerHealthManager>();
        HealthPoint = 1;
        instanceEventExplosion = FMODUnity.RuntimeManager.CreateInstance(eventExplosion);
        //instanceEventExplosion = AudioManager.Sounds["medium-explosion-40472"];
        timerExplode = 0;
        explode = false;
        visualEffect = GetComponent<VisualEffect>();
        visualEffect.enabled = false;
    }

    void IDamageable.TakeDamage(float _damage)
    {
        HealthPoint -= _damage;
        if (HealthPoint <= 0 && !explode)
        {

            explode = true;
            visualEffect.enabled = true;
            visualEffect.Play();
            MonoBehaviour[] monos = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
            GameObject go = Instantiate(fog);
            go.transform.position = transform.position;

            foreach (var mono in monos)
            {
                if (mono is IDamageable)
                {
                    if (Vector3.Distance(transform.position, mono.transform.position) <= explosionRadius)
                    {
                        (mono as IDamageable).TakeDamage(damageExplosion);
                    }
                }
            }

            instanceEventExplosion.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));

            explode = true;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            gameObject.GetComponent<NavMeshObstacle>().enabled = false;
            instanceEventExplosion.start();
        }
    }

    private void Update()
    {
        if (HealthPoint <= 0)
        {
            timerExplode += Time.deltaTime;
            if (timerExplode >= 2.2f)
            {
                Destroy(gameObject);
            }
        }
    }
}
