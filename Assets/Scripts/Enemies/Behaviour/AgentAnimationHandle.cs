using UnityEngine.AI;

public class AgentAnimationHandle : AnimationHandle
{
    private NavMeshAgent m_agent;

    protected override void Start()
    {
        base.Start();
        m_agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        m_animator.SetFloat("Velocity", m_agent.velocity.magnitude);
    }
}
