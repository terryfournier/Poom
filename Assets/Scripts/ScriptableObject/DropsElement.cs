using UnityEngine;

[CreateAssetMenu()]
public class DropsElement : ScriptableObject
{
    [SerializeField] public GameObject m_PrefabsDrops;
    [SerializeField] public float m_DropRate;
}
