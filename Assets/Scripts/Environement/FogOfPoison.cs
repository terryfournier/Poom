using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfPoison : MonoBehaviour
{
    [SerializeField] private float m_lifeTime;
    private List<IPoisoneable> m_poisoneable = new List<IPoisoneable>();

    private void Start()
    {
        StartCoroutine(LifeTime());
    }

    private void Update()
    {
        foreach (IPoisoneable poisoneable in m_poisoneable)
        {
            poisoneable.Poison(5);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IPoisoneable tempPoisoned = other.transform.GetComponentInParent<IPoisoneable>();

        if (tempPoisoned != null)
            m_poisoneable.Add(tempPoisoned);

    }

    private void OnTriggerExit(Collider other)
    {
        IPoisoneable tempPoisoned = other.gameObject.GetComponentInParent<IPoisoneable>();

        if (m_poisoneable.Contains(tempPoisoned))
            m_poisoneable.Remove(tempPoisoned);
    }

    private IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(m_lifeTime);
        Destroy(gameObject);
    }
}
