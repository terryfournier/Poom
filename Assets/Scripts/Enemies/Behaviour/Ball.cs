using System;
using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float m_lifeTime;
    [NonSerialized] public float m_damage;

    private void Start()
    {
        StartCoroutine(LifeTimeCorou());
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable dm = other.gameObject.GetComponent<IDamageable>();
        if (dm != null)
        {
            dm.TakeDamage(m_damage);
        }
        Destroy(gameObject);
    }

    private IEnumerator LifeTimeCorou()
    {
        yield return new WaitForSeconds(m_lifeTime);
        Destroy(gameObject);
    }
}
