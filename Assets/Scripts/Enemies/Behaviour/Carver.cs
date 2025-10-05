using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Carver : MonoBehaviour
{
    [SerializeField] private string m_defenseAnimName;
    private NavMeshAgent m_agent;
    private NavMeshObstacle m_obstacle;
    private EnemyBehaviour m_enemy;
    private Coroutine m_unCarvCorou;
    private StateMachine m_prevState;

    private EnemyHealthManager m_enemyHealth;
    private AnimationHandle m_animHandle;
    private void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_obstacle = GetComponent<NavMeshObstacle>();
        m_enemy = GetComponent<EnemyBehaviour>();
        m_enemyHealth = GetComponent<EnemyHealthManager>();
        m_animHandle = GetComponent<AnimationHandle>();

        m_enemyHealth.OnDamageSet += DamageInflict;
        m_prevState = m_enemy.State;
    }

    private void Update()
    {
        if(m_prevState != m_enemy.State)
        {
            m_prevState = m_enemy.State;
            switch (m_prevState)
            {
                case StateMachine.Purchase:
                    UnCarv();
                    break;
                case StateMachine.Attack:
                    Carv();
                    break;
                case StateMachine.Idle:
                    Carv();
                    break;
            }
        }
    }

    private void OnDestroy()
    {
        if (m_enemyHealth != null)
            m_enemyHealth.OnDamageSet -= DamageInflict;
    }

    private void Carv()
    {
        m_agent.enabled = false;
        m_obstacle.enabled = true;
    }

    private void UnCarv()
    {
        if(m_unCarvCorou == null)
        {
            m_unCarvCorou = StartCoroutine(UnCravCoroutine());
        }
    }

    private IEnumerator UnCravCoroutine()
    {
        m_obstacle.enabled = false;
        yield return null;
        m_agent.enabled = true;
        m_unCarvCorou = null;
    }

    private IEnumerator DamageStop()
    {
        if (m_agent.enabled)
        {
            Carv();
            yield return new WaitForSeconds(m_animHandle.GetAnimLength(m_defenseAnimName));
            UnCarv();
        }
    }

    public void DamageInflict()
    {
        StartCoroutine(DamageStop());
    }
}
