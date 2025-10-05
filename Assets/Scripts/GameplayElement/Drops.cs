using Unity.VisualScripting;
using UnityEngine;

public class Drops : MonoBehaviour
{
    [SerializeField] private Transform m_pelvis;
    [SerializeField] private DropsDatabase m_dropsDatabase;
    private float m_totalProba;
    private float m_dropsProba;

    void Start()
    {
        foreach (DropsElement drops in m_dropsDatabase.m_drops)
        {
            m_totalProba += drops.m_DropRate;
            m_dropsProba += drops.m_DropRate;
        }
        m_totalProba += m_dropsDatabase.m_weapons.m_dropRate;
    }

    public void ActivDrops()
    {
        if (m_dropsDatabase.m_drops.Count == 0)
            return;

        float randomNum = Random.Range(0.0f, m_totalProba);

        GameObject toDrop = null;
        float currentDrop = m_dropsDatabase.m_drops[0].m_DropRate;
        for (int i = 1; i < m_dropsDatabase.m_drops.Count; i++)
        {
            if (randomNum < currentDrop)
            {
                toDrop = m_dropsDatabase.m_drops[i - 1].m_PrefabsDrops;
            }
            else
            {
                currentDrop += m_dropsDatabase.m_drops[i].m_DropRate;
            }
        }

        if (toDrop == null)
        {
            if (randomNum > m_dropsProba)
            {
                toDrop = WeaponSeletor();
            }
            else
            {
                toDrop = m_dropsDatabase.m_drops[m_dropsDatabase.m_drops.Count - 1].m_PrefabsDrops;
            }
        }

        Ray ray = new Ray(m_pelvis.position, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, LayerMask.GetMask("Ground")))
        {
            Vector3 spawnPoint = hit.point;
            Instantiate(toDrop, spawnPoint, Quaternion.identity);
        }
    }

    private GameObject WeaponSeletor()
    {
        DropsElement[] weapon = m_dropsDatabase.m_weapons.m_weapons;
        float totalProba = 0; 

        for (int i = 0; i < weapon.Length; i++)
        {
            totalProba += weapon[i].m_DropRate;
        }
        float randomNum = Random.Range(0.0f, totalProba);
        float currentDrop = weapon[0].m_DropRate;
        for (int i = 1; i < weapon.Length; i++)
        {
            if (randomNum < currentDrop)
            {
                return weapon[i - 1].m_PrefabsDrops;
            }
            else
            {
                currentDrop += weapon[i].m_DropRate;
            }
        }

        return weapon[weapon.Length - 1].m_PrefabsDrops;
    }
}
