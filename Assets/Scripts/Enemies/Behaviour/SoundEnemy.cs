using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

public class SoundEnemy : MonoBehaviour
{
    [Header("Put as much as eventref than string")]

    [SerializeField] private List<string> m_soundName;
    [SerializeField] private List<EventReference> m_soundRef;

    private Dictionary<string, SoundHandle> m_sounds = new Dictionary<string, SoundHandle>();

    private EnemyBehaviour m_enemy;
    private EnemyHealthManager m_enemyHealth;

    private PauseManager m_pauseManager;
    private AudioManager m_audioManager;

    private void Start()
    {
        m_enemy = GetComponent<EnemyBehaviour>();
        m_enemyHealth = GetComponent<EnemyHealthManager>();

        m_pauseManager = FindAnyObjectByType<PauseManager>();
        m_audioManager = FindAnyObjectByType<AudioManager>();

        for (int i = 0; i < m_soundRef.Count; i++)
        {
            m_sounds.Add(m_soundName[i], new SoundHandle(m_soundRef[i], m_audioManager));
        }

        m_enemy.AttackTarget += AttackSound;

        if (m_enemyHealth != null)
        {
            m_enemyHealth.OnDamageSet += DefenseSound;
            m_enemyHealth.OnDeath.AddListener(DeathSound);
        }
    }

    private void Update()
    {
        if (m_enemy.State == StateMachine.Dead)
        {
            m_sounds["Breathe"].StopSound();
            return;
        }

        foreach (SoundHandle sound in m_sounds.Values)
        {
            sound.Spatialization(gameObject);
        }

        if (!m_sounds["Breathe"].IsRunnig())
        {
            m_sounds["Breathe"].StartSound();
        }
    }

    private void OnDestroy()
    {
        if (m_enemy != null)
            m_enemy.AttackTarget -= AttackSound;

        if (m_enemyHealth != null)
        {
            m_enemyHealth.OnDamageSet -= DefenseSound;
            m_enemyHealth.OnDeath.RemoveListener(DeathSound);
        }
    }

    private void AttackSound()
    {
        m_sounds["Attack"].StartSound();
    }

    private void DefenseSound()
    {
        m_sounds["Defense"].StartSound();
    }

    private void DeathSound()
    {
        m_sounds["Death"].StartSound();
    }
}