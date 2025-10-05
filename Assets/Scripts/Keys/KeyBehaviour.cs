using System;
using UnityEngine;

public class KeyBehaviour : MonoBehaviour
{
    public Action OnKeyTaken;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnKeyTaken?.Invoke();

            Destroy(gameObject);
        }
    }
}
