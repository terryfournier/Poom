using UnityEngine;

[CreateAssetMenu()]
public class WeaponDatabase : ScriptableObject
{
    [SerializeField] public DropsElement[] m_weapons;
    [SerializeField] public float m_dropRate;
}
