using UnityEngine;

public class Blocker : Tank
{
    [SerializeField] private GameObject m_ballPrefabs;
    [SerializeField] private Transform m_invocSpawn;
    private float m_ballSpeed = 1.0f;

    protected override void Attack()
    {
        Invocation();
    }

    private void Invocation()
    {
        GameObject go = Instantiate(m_ballPrefabs, m_invocSpawn.position, Quaternion.identity);
        Ball fireBall = go.GetComponent<Ball>();
        fireBall.m_damage = m_damage + 0.1f * m_multiplier;
        Vector3 direction = Target.transform.position - m_invocSpawn.position;
        go.GetComponent<Rigidbody>().linearVelocity = transform.forward * m_ballSpeed;
    }
}
