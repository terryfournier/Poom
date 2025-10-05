using UnityEngine;
using UnityEngine.VFX;

public class VFXHeadShot : MonoBehaviour
{
    VisualEffect m_visualEffect;
    [SerializeField] Transform m_pivot;
    private void Start()
    {
        m_visualEffect = GetComponentInChildren<VisualEffect>();
        Destroy(gameObject, m_visualEffect.GetFloat("HSLifetime"));
    }

    private void Update()
    {
        Quaternion rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        m_pivot.eulerAngles = rotation.eulerAngles;
    }
}
