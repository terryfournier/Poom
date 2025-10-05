using UnityEngine;
using UnityEngine.VFX;

public class Bloodsplatter : MonoBehaviour
{
    VisualEffect effect;
    float lifetime = 0.4f;
    public float size = 0.1f;

    private void Start()
    {
        effect = GetComponent<VisualEffect>();
        effect.Play();
    }

    void Update()
    {
        if (lifetime > 0)
        {
            lifetime -= Time.deltaTime;
        }
        else
        {
            effect.Stop();
            Destroy(gameObject);
        }
    }
}
