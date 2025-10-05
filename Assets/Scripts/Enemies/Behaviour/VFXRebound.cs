using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class VFXRebound : MonoBehaviour
{
    VisualEffect m_visualEffect;
    private void Start()
    {
        m_visualEffect = GetComponent<VisualEffect>();
        Destroy(gameObject, m_visualEffect.GetFloat("RIcochetLifeTime"));
    }
}
