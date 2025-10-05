using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AgentBehaviour : MonoBehaviour
{
    [SerializeField] private Transform m_pelvis;
    private EnemyBehaviour m_enemy;
    private NavMeshAgent m_agent;
    private Coroutine m_pathSearcherCorou;
    private float m_detectionAngle = 30.0f;
    private float playerPurchase = 5.0f;
    private Vector3 m_target;
    private float m_timeWaiting = 0.5f;
    private bool m_pathFound = false;

    private void Start()
    {

        m_enemy = GetComponent<EnemyBehaviour>();

        m_agent = GetComponent<NavMeshAgent>();

        string key = GetComponent<KeyHolder>().Key;
        EnemyData data = FindAnyObjectByType<EnemyLoader>().GetData(key);

        m_agent.speed = data.Speed;

        m_enemy.UpdateMultiplier += UpdateSpeed;

        
    }

    private void Update()
    {
        if(m_enemy.State == StateMachine.Dead)
        {
            m_agent.enabled = false;
            return;
        }

        if (!m_pathFound)
        {
            NavMeshPath path = new NavMeshPath();
            m_agent.CalculatePath(m_enemy.Target.transform.position, path);
            if (path.status != NavMeshPathStatus.PathComplete)
            {
                m_enemy.m_pathIncorrect = true;
            }
            else
            {
                m_enemy.m_pathIncorrect = false;
                m_pathFound = true;
            }
        }

        if (m_enemy.State == StateMachine.Purchase)
        {
            if (m_pathSearcherCorou == null)
            {
                m_pathSearcherCorou = StartCoroutine(pathSearcher());
            }
        }
    }

    private void OnDestroy()
    {
        if (m_enemy != null)
            m_enemy.UpdateMultiplier -= UpdateSpeed;
    }

    private IEnumerator pathSearcher()
    {
        while (m_enemy.State == StateMachine.Purchase)
        {
            if (m_agent.enabled)
            {
                if (!m_agent.pathPending)
                {
                    GetPath();
                }
            }
            yield return new WaitForSeconds(m_timeWaiting);
        }
        m_pathSearcherCorou = null;
    }

    private void GetPath()
    {
        float heightPlayerEnemy = Mathf.Abs(m_pelvis.position.y - m_enemy.Target.transform.position.y);
        if (heightPlayerEnemy > 1.0f
            && NavMesh.SamplePosition(m_enemy.Target.transform.position,
            out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
        {
            m_agent.SetDestination(m_enemy.Target.transform.position);
            m_target = m_enemy.Target.transform.position;
        }
        else
        {
            if (Vector3.Distance(m_enemy.Target.transform.position, transform.position) < playerPurchase)
            {
                m_agent.SetDestination(m_enemy.Target.transform.position);
                m_target = m_enemy.Target.transform.position;
            }
            else
            {
                RandomPath();
            }
        }
    }

    private void RandomPath()
    {
        int count = 0;
        do
        {
            Vector2 offset = Random.insideUnitCircle * 10.0f;
            m_target = transform.position;
            m_target.x += offset.x;
            m_target.z += offset.y;
        }
        while (count++ < 10
        && !IsTargetForward(m_target)
        && !NavMesh.SamplePosition(m_target, out NavMeshHit hit, 1.0f, NavMesh.AllAreas));

        if (count >= 10 || !IsTargetForward(m_target))
        {
            m_target = m_enemy.Target.transform.position;
        }
        m_agent.SetDestination(m_target);
    }

    private bool IsTargetForward(Vector3 _target)
    {
        Vector3 directionToTarget = _target - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToTarget);
        return angle < m_detectionAngle;
    }

    private void UpdateSpeed()
    {
        m_agent.speed += 0.25f * m_enemy.m_Multipler;
    }
}
