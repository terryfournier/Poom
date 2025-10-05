using System;
using UnityEngine;
using UnityEngine.AI;

public class LinksHandle : MonoBehaviour
{
    protected LinkBehaviour m_link;
    protected NavMeshAgent m_agent;
    protected Action m_linkHere;
    protected OffMeshLinkData m_linkData;
    protected LinkType m_linkType;
    protected Vector3 m_StartLink { get; private set; } = Vector3.zero;
    protected Vector3 m_EndLink { get; private set; } = Vector3.zero;

    public Action<bool> m_linkDetect;

    protected void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();

        m_linkHere = LinkHere;
    }

    private void Update()
    {
        if (m_agent.isOnOffMeshLink && m_link == null)
        {
            LinksDetected();
        }
    }

    private void LinksDetected()
    {
        m_linkDetect?.Invoke(false);
        m_link = (m_agent.currentOffMeshLinkData.owner as MonoBehaviour).gameObject.GetComponent<LinkBehaviour>();
        m_linkType = m_link.m_type;

        m_linkData = m_agent.currentOffMeshLinkData;
        m_StartLink = m_agent.transform.position;
        m_EndLink = m_linkData.endPos + Vector3.up * m_agent.baseOffset;

        transform.LookAt(m_link.transform);

        m_link.ToogleLinkAcivation(false);
        m_linkHere?.Invoke();
    }

    protected void LinkHere()
    {
        m_link.ToogleLinkAcivation(true);
        m_agent.CompleteOffMeshLink(); // mark the link as "done"
        m_link = null;
        m_linkDetect?.Invoke(true);
    }
}
