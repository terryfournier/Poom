using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;
using UnityEngine.VFX;

public class Barel : MonoBehaviour, IDamageable
{
    float HealthPoint = 1;
    [SerializeField] float explosionRadius = 10.0f;
    [SerializeField] float damageExplosion = 50.0f;
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
            //Check all rigidbodies in range and apply damage and knockback
            //Rigidbody[] rigidbodies = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);
            MonoBehaviour[] monos = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

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


            //foreach (Rigidbody rb in rigidbodies)
            //{
            //    if (rb != null)
            //    {
            //        rb.AddExplosionForce(damageExplosion, transform.position, explosionRadius);
            //    }

            //}
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
