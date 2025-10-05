using Unity.AI.Navigation;
using UnityEngine;

public class LinkBehaviour : MonoBehaviour
{
    private NavMeshLink m_link;
    public LinkType m_type = LinkType.Double;

    private void Start()
    {
        m_link = GetComponent<NavMeshLink>();
    }

    public void ToogleLinkAcivation(bool _enabled)
    {
        m_link.enabled = _enabled;
    }
}
