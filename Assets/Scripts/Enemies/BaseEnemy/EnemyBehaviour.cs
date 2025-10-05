using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemyBehaviour : MonoBehaviour
{

    // Base Data
    [SerializeField] private Transform m_head;
    [SerializeField] private SkinnedMeshRenderer m_mesh;
    private float m_coolDown;
    private float m_distanceAttack;
    protected float m_damage;
    protected float m_distanceFactor;
    public EnemyType Type { get; private set; } = EnemyType.CLASSIC;
    private float m_maxSizeMesh;
    protected int m_multiplier = 0;
    public Action UpdateMultiplier;
    public int m_Multipler
    {
        get => m_multiplier;
        set
        {
            m_multiplier = value;
            UpdateMultiplier?.Invoke();
        }
    }

    // Player Data
    public PlayerController Target { get; protected set; } = null;
    public StateMachine State { get; private set; } = StateMachine.Idle;

    // Detection data 
    [SerializeField] private float m_detectionRange = 50.0f;
    private float m_detectionAngle = 60.0f;

    // Attack Data 
    [SerializeField] protected UnityEvent OnHit;
    private Coroutine m_coolDownCorou;
    public Action AttackTarget;

    // Idle Data
    private bool m_attackLocker = false;
    [NonSerialized] public bool m_pathIncorrect = false;

    virtual protected void Start()
    {
        m_distanceFactor = 2.0f;

        string key = GetComponent<KeyHolder>().Key;
        EnemyData data = FindAnyObjectByType<EnemyLoader>().GetData(key);

        m_coolDown = data.Cooldown;
        m_damage = data.Damage;
        m_distanceAttack = data.DistanceAttack;
        Type = data.TypeEn;

        Target = FindFirstObjectByType<PlayerController>();

        if (m_mesh != null)
        {
            m_maxSizeMesh = Mathf.Max(m_mesh.bounds.extents.x, m_mesh.bounds.extents.z);
        }
    }

    virtual protected void Update()
    {
        if (m_pathIncorrect)
            return;

        switch (State)
        {
            case StateMachine.Purchase:
                PurchaseBehaviour();
                break;
            case StateMachine.Attack:
                AttackBehaviour();
                break;
            case StateMachine.Idle:
                IdleBehaviour();
                break;
            case StateMachine.Dead:
                break;
        }
    }

    private void PurchaseBehaviour()
    {
        if (Vector3.Distance(transform.position, Target.transform.position) < m_detectionRange)
        {
            if (Vector3.Distance(transform.position, Target.transform.position) < DistanceValueToTest())
            {
                Vector3 directionToPlayer = Target.transform.position - transform.position;
                float angle = Vector3.Angle(transform.forward, directionToPlayer);

                if (angle < m_detectionAngle)
                {
                    if (Physics.Raycast(transform.position, directionToPlayer.normalized,
                        out RaycastHit hit, DistanceValueToTest(), ~LayerMask.GetMask("Enemy", "Detector")))
                    {
                        if (hit.transform.CompareTag("Player") || hit.transform.CompareTag("MainCamera"))
                        {
                            State = StateMachine.Attack;
                        }
                    }
                }
            }
        }
        else
        {
            State = StateMachine.Idle;
        }
    }

    private void AttackBehaviour()
    {
        if (Vector3.Distance(transform.position, Target.transform.position) < DistanceValueToTest())
        {
            if (m_coolDownCorou == null)
            {
                LaunchAttack();
                m_attackLocker = true;
            }
            State = StateMachine.Idle;
        }
        else
        {
            State = StateMachine.Purchase;
        }
    }

    private void IdleBehaviour()
    {
        if (m_coolDownCorou != null)
        {
            Vector3 targetPos = Target.transform.position;
            targetPos.y = transform.position.y;
            if (Vector3.Distance(transform.position, targetPos) > DistanceValueToTest())
            {
                State = StateMachine.Purchase;
            }
        }
        else
        {
            if (m_attackLocker)
            {
                m_attackLocker = false;
                if (Vector3.Distance(transform.position, Target.transform.position) < DistanceValueToTest())
                {
                    State = StateMachine.Attack;
                }
                else
                {
                    State = StateMachine.Purchase;
                }
            }
            else
            {
                if (Vector3.Distance(transform.position, Target.transform.position) < m_detectionRange)
                {
                    State = StateMachine.Purchase;
                }
            }
        }
    }

    virtual protected void Attack()
    {
        PlayerHealthManager targetHealth = Target.gameObject.GetComponent<PlayerHealthManager>();

        if (targetHealth != null)
        {
            if (Vector3.Distance(transform.position, Target.transform.position) < DistanceValueToTest())
                targetHealth.TakeDamage(m_damage + 1f * m_multiplier);
        }
        if (OnHit != null)
            OnHit.Invoke();
    }

    private void LaunchAttack()
    {
        AttackTarget?.Invoke();
        m_coolDownCorou = StartCoroutine(CoolDownAttack());
    }

    private IEnumerator CoolDownAttack()
    {
        yield return new WaitForSeconds(m_coolDown);
        m_coolDownCorou = null;
    }

    protected float DistanceValueToTest()
    {
        return m_distanceAttack + m_maxSizeMesh * m_distanceFactor;
    }

    public void Kill()
    {
        State = StateMachine.Dead;
    }
}
