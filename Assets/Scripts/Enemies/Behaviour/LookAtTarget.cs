using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    private EnemyBehaviour m_enemy;
    private LinksHandle m_links;

    private void Start()
    {
        m_enemy = GetComponent<EnemyBehaviour>();
        m_links = GetComponent<LinksHandle>();
        if (m_links != null)
            m_links.m_linkDetect += OnLinks;
    }

    private void OnDestroy()
    {
        if (m_links != null)
            m_links.m_linkDetect -= OnLinks;
    }

    private void Update()
    {
        if (m_enemy.State == StateMachine.Dead)
            return;

        transform.LookAt(m_enemy.Target.transform.position);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    private void OnLinks(bool _active)
    {
        this.enabled = _active;
    }
}
