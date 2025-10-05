using System.Collections;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    private PlayerView m_playerView;
    private float m_lerpTime = 0.5f;
    private float m_speed = 3.0f;

    private void Start()
    {
        m_playerView = GetComponent<PlayerView>();
    }
    public void gripCamera(Transform _trs, float _lerpTime)
    {
        m_playerView.enabled = false;
        transform.position = _trs.position;
        transform.rotation = _trs.rotation;
        m_lerpTime = _lerpTime;

        Weapon weapon = GetComponentInChildren<Weapon>();
        if(weapon != null)
            weapon.gameObject.SetActive(false);
    }

    public void Lerper(Transform _trs)
    {
        StartCoroutine(LaunchLerp(_trs));
    }
    
    private IEnumerator LaunchLerp(Transform _trs)
    {
        while (m_lerpTime > 0)
        {
            m_lerpTime -= Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, _trs.position, Time.deltaTime * m_speed);
            yield return null;
        }
    }
}

//while (m_rotationDone < 20.0f)
//{
//    float currentRotation = Mathf.Lerp(0, 10, Time.deltaTime * m_speed);

//    m_rotationDone += currentRotation;
//    transform.eulerAngles -= new Vector3(0, currentRotation, 0);
//    yield return null;
//}

//m_rotationDone = 0.0f;
//while (m_rotationDone < 40.0f)
//{
//    float currentRotation = Mathf.Lerp(0, 10, Time.deltaTime * m_speed);

//    m_rotationDone += currentRotation;
//    transform.eulerAngles += new Vector3(0, currentRotation, 0);
//    yield return null;
//}

//m_rotationDone = 0.0f;
//while (m_rotationDone < 20.0f)
//{
//    float currentRotation = Mathf.Lerp(0, 10, Time.deltaTime * m_speed);

//    m_rotationDone += currentRotation;
//    transform.eulerAngles -= new Vector3(0, currentRotation, 0);
//    yield return null;
//}

//while (m_lerpTime > 0.0f)
//{
//    m_lerpTime -= Time.deltaTime;
//    transform.position = Vector3.Lerp(transform.position, _trs.position, Time.deltaTime * m_speed);
//    yield return null;
//}

//delete Event in arm anim
//Add launch Lerp in ArmBenitar