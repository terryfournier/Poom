using System.Collections;
using UnityEngine.VFX;
using UnityEngine;

public class LinksHandleTP : LinksHandle
{
    [SerializeField] private VisualEffect m_vfxPrefabs;
    [SerializeField] private string m_SpawnMoment;

    private new void Start()
    {
        base.Start();

        m_linkHere = Teleportation;
    }

    private void Teleportation()
    {
        StartCoroutine(TeleportationLink());
    }

    private IEnumerator TeleportationLink()
    {
        VisualEffect m_currentVFX = Instantiate(m_vfxPrefabs, m_EndLink, Quaternion.identity);

        m_currentVFX.Play();
        float lifetime = m_currentVFX.GetFloat(m_SpawnMoment);
        yield return new WaitForSeconds(lifetime);
        Destroy(m_currentVFX.gameObject, lifetime);
        LinkHere();
    }
}
