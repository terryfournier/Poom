using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class DropsDatabase : ScriptableObject
{
    [SerializeField] public List<DropsElement> m_drops = new List<DropsElement>();
    [SerializeField] public WeaponDatabase m_weapons;
}
