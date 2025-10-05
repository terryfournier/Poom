using FMODUnity;
using UnityEngine;
using UnityEngine.VFX;

public class SyringueOnFloor : MonoBehaviour, ICollectable
{
    private float existDuration;
    private float m_maxLife = 90.0f;
    private VisualEffect m_vfxHi;

    private void Start()
    {
        m_vfxHi = GetComponent<VisualEffect>();
        m_vfxHi.SetFloat("CircleLifeTime", m_maxLife);
    }

    void ICollectable.Collect(PlayerInventory _playerInventory)
    {
        

        if(_playerInventory.syringeNb < 3)
        {
            _playerInventory.syringeNb++;
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        existDuration += Time.deltaTime;
        if(existDuration >= m_maxLife)
        {
            Destroy(GetComponent<StudioEventEmitter>());
            Destroy(gameObject);
        }
    }
}
