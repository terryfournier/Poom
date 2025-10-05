using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealthManager : MonoBehaviour, IDamageable, IPoisoneable
{
    [SerializeField] private string m_deathAnimName;
    private EnemyBehaviour m_enemy;
    [SerializeField] private float m_lifePoint;
    [SerializeField] public UnityEvent OnDeath;
    //public Action OnDeath;
    public Action OnDamageSet;
    private Score m_scoreHandle;

    private float poisonScale;
    private Coroutine PoisonningCorou;

    private void Start()
    {
        string key = GetComponent<KeyHolder>().Key;
        EnemyData data = FindAnyObjectByType<EnemyLoader>().GetData(key);
        m_lifePoint = data.LifePoints;

        m_enemy = GetComponent<EnemyBehaviour>();

        m_scoreHandle = FindAnyObjectByType<Score>();
    }

    private void Update()
    {
        if (m_enemy.State == StateMachine.Dead)
            return;

    }

    public void TakeDamage(float _damage)
    {
        if (m_enemy.State == StateMachine.Dead)
            return;

        m_lifePoint -= _damage;
        m_scoreHandle.AddScore(_damage);

        if (m_lifePoint <= 0)
        {
            if (OnDeath != null)
                OnDeath.Invoke();
            Destroy(transform.gameObject, GetComponent<AnimationHandle>().GetAnimLength(m_deathAnimName));
            m_enemy.Kill();
        }
        else
        {
            OnDamageSet?.Invoke();
        }

    }

    public void Poison(int _poisonPower)
    {
        poisonScale = _poisonPower;

        if (PoisonningCorou == null)
            PoisonningCorou = StartCoroutine(Poisoned());
    }

    private IEnumerator Poisoned()
    {
        float coolDown = 3.0f;

        while (poisonScale > 0)
        {
            TakeDamage(poisonScale);
            poisonScale--;
            yield return new WaitForSeconds(coolDown);
        }

        PoisonningCorou = null;
    }
}
