using UnityEngine;

public class AnimationHandle : MonoBehaviour
{
    protected Animator m_animator;
    private EnemyBehaviour m_enemy;
    private EnemyHealthManager m_enemyHealth;
    private AnimationClip[] m_clips;

    virtual protected void Start()
    {
        m_animator = GetComponent<Animator>();
        m_enemy = GetComponent<EnemyBehaviour>();
        m_enemyHealth = GetComponent<EnemyHealthManager>();
        
        m_clips = m_animator.runtimeAnimatorController.animationClips;
        
        m_enemy.AttackTarget += AnimAttack;
        m_enemyHealth.OnDeath.AddListener(AnimDeath);
        m_enemyHealth.OnDamageSet += AnimDamage;
    }

    private void OnDestroy()
    {
        if (m_enemy != null)
            m_enemy.AttackTarget -= AnimAttack;

        if (m_enemyHealth != null)
        {
            m_enemyHealth.OnDeath.RemoveListener(AnimDeath);
            m_enemyHealth.OnDamageSet -= AnimDamage;
        }
    }

    private void AnimAttack()
    {
        m_animator.SetTrigger("Attack");
    }

    private void AnimDeath()
    {
        m_animator.SetTrigger("Death");
        m_animator.SetBool("Dead", true);
    }

    private void AnimDamage()
    {
        m_animator.SetTrigger("Defense");
    }

    public float GetAnimLength(string _name)
    {
        foreach (AnimationClip clip in m_clips)
        {
            if (_name == clip.name)
            {
                return clip.length;
            }
        }
        return 0.0f;
    }
}
