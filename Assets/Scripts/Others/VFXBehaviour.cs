using UnityEngine;
using UnityEngine.VFX;

public class VFXBehaviour : MonoBehaviour
{
    [SerializeField] private VisualEffect m_vfxPrefabs;
    [SerializeField] private Transform m_spawnPoint;
    [SerializeField] private string m_lifeTimeName;
    [SerializeField] private Vector3 rotation;

    public void CreateVFX(Vector3 _position)
    {
        VisualEffect m_currentVFX = Instantiate(m_vfxPrefabs, _position, Quaternion.identity);

        m_currentVFX.Play();

        Destroy(m_currentVFX.gameObject, m_currentVFX.GetFloat(m_lifeTimeName));
    }

    public void CreateVFX(Vector3 _position, Quaternion _rotation)
    {
        VisualEffect m_currentVFX = Instantiate(m_vfxPrefabs, _position, _rotation);

        m_currentVFX.Play();

        Destroy(m_currentVFX.gameObject, m_currentVFX.GetFloat(m_lifeTimeName));
    }

    public void CreateVFX()
    {
        CreateVFX(m_spawnPoint.position, m_spawnPoint.rotation);
    }

    public void CreateGroudedVFX()
    {
        Ray ray = new Ray(m_spawnPoint.position, Vector3.down);

        Physics.Raycast(ray, out RaycastHit hit, 100.0f, ~LayerMask.GetMask("Enemy"));

        if (hit.transform.CompareTag("Ground"))
        {
            Vector3 pos = m_spawnPoint.position;
            pos.y = hit.point.y + 0.01f;
            CreateVFX(pos, transform.rotation);
        }
    }


    public void CreateVFXWithTimer(float _lifetime)
    {
        VisualEffect m_currentVFX = Instantiate(m_vfxPrefabs, m_spawnPoint.position, m_spawnPoint.rotation, m_spawnPoint.transform);

        m_currentVFX.Play();

        Destroy(m_currentVFX.gameObject, _lifetime);
    }
}
