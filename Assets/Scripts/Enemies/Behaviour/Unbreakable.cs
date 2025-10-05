using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class Unbreakable : MonoBehaviour
{
    [SerializeField] private GameObject m_vfxRebound;

    public void VFXActivation(Vector3 _hitPoint)
    {
        Instantiate(m_vfxRebound, _hitPoint, Quaternion.identity);
    }
}
