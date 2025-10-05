using System.Collections;
using UnityEngine;

public class RecoilCameraController : MonoBehaviour
{
    [Header("Mouse Look")]
    public float sensitivity = 100f;
    public float pitchClamp = 80f;

    private float pitch;
    private float yaw;

    [Header("Recoil Settings")]
    public Vector3 recoilKick = new Vector3(-5f, 2f, 0f); // Pitch, Yaw, Roll
    public float recoilReturnSpeed = 8f; // How fast it returns
    public float recoilSnapiness = 12f;  // How fast it applies

    private Vector3 recoilCurrent;
    private Vector3 recoilTarget;

    private MouseManager mouseManager;
    private bool justFetchedMouseManager = false;

    void Start()
    {
        StartCoroutine(FetchMouseManager());
    }

    void Update()
    {
        if (justFetchedMouseManager)
        {
            StopCoroutine(FetchMouseManager());
            justFetchedMouseManager = false;
        }

        HandleMouseLook();
        UpdateRecoil();
        ApplyFinalRotation();
    }

    void HandleMouseLook()
    {
        float aimFactor = (mouseManager) ? mouseManager.AimFactor : 0.75f;
        float mouseX = Helper.GetAxis_Mouse(true) * mouseManager.Sensitivity.x * aimFactor * Time.deltaTime;
        float mouseY = Helper.GetAxis_Mouse(false) * mouseManager.Sensitivity.y * aimFactor * Time.deltaTime;

        yaw += mouseX;
        pitch -= mouseY;
    }

    void UpdateRecoil()
    {
        // Spring back toward zero
        recoilTarget = Vector3.Lerp(recoilTarget, Vector3.zero, recoilReturnSpeed * Time.deltaTime);
        recoilCurrent = Vector3.Slerp(recoilCurrent, recoilTarget, recoilSnapiness * Time.deltaTime);
    }

    void ApplyFinalRotation()
    {
        Quaternion lookRotation = Quaternion.Euler(pitch, yaw, 0f);
        Quaternion recoilRotation = Quaternion.Euler(recoilCurrent);
        transform.localRotation = lookRotation * recoilRotation;
    }

    // Call this to apply recoil from other scripts
    public void ApplyRecoil()
    {
        Vector3 randomRecoil = new Vector3(
            recoilKick.x,
            Random.Range(-recoilKick.y, recoilKick.y),
            Random.Range(-recoilKick.z, recoilKick.z)
        );

        recoilTarget += randomRecoil;
    }


    private IEnumerator FetchMouseManager()
    {
        while (!justFetchedMouseManager)
        {
            mouseManager = FindAnyObjectByType<MouseManager>();

            if (mouseManager) { justFetchedMouseManager = true; }

            yield return null;
        }
    }
}
