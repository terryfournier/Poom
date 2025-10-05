using NUnit.Framework.Internal;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class NozleShot : MonoBehaviour
{
    [SerializeField] Vector3 posMuzle;
    Light flameLight;

    private void Awake()
    {
        flameLight = GetComponent<Light>();
        flameLight.enabled = false;
    }

}
