using UnityEngine;

public class ArmBenitar : MonoBehaviour
{
    [SerializeField] private Transform m_head;
    [SerializeField] private Transform m_lerpAnim;
    private CameraHandler m_camera;

    private AnimationClip[] m_clips;

    private void Awake()
    {
        m_clips = GetComponent<Animator>().runtimeAnimatorController.animationClips;

        Camera.main.transform.parent = null;
        m_camera = Camera.main.transform.GetComponent<CameraHandler>();

        m_camera.gripCamera(m_head, GetAnimLength());

    }

    public void LaunchLerp()
    {
        m_camera.Lerper(m_lerpAnim);
    }

    public float GetAnimLength()
    {
        if(m_clips == null) 
            return 0.0f;

        foreach (AnimationClip clip in m_clips)
        {
            if ("ARMS_BENITAR" == clip.name)
            {
                return clip.length;
            }
        }
        return 0.0f;
    }
}
