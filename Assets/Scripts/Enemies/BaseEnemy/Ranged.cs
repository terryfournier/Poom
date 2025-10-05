using UnityEngine.VFX;
using UnityEngine;
using System.Collections;

public class Ranged : EnemyBehaviour
{
    [SerializeField] private GameObject m_ballPrefabs;
    [SerializeField] private Transform m_invocSpawn;
    private float m_ballSpeed = 5.0f;

    protected override void Start()
    {
        base.Start();

        m_distanceFactor = 1.0f;
    }

    protected override void Attack()
    {
        StartCoroutine(Invocation());
    }

    private IEnumerator Invocation()
    {
        if(OnHit != null)
            OnHit.Invoke();

        yield return new WaitForSeconds(1.0f);
        Vector3 spawnPoint = m_invocSpawn.position + transform.forward * 1.5f;

        if (Vector3.Distance(transform.position, Target.transform.position) < DistanceValueToTest())
        {
            GameObject go = Instantiate(m_ballPrefabs, spawnPoint, Quaternion.identity);
            Ball fireBall = go.GetComponent<Ball>();
            fireBall.m_damage = m_damage + 0.1f * m_multiplier;
            Vector3 direction = Target.transform.position - spawnPoint;
            go.GetComponent<Rigidbody>().linearVelocity = direction * m_ballSpeed;
        }
    }
}

