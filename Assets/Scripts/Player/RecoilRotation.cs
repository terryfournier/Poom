using UnityEngine;

public class RecoilRotator : MonoBehaviour
{
    [Header("Recoil Settings")]
    public float recoilSharpness = 15f;     // How quickly the camera matches recoilRotation
    public float returnSpeed = 10f;         // How quickly recoilRotation returns to identity

    private Quaternion recoilRotation = Quaternion.identity;   // Target recoil direction
    private Quaternion kickbackRotation = Quaternion.identity; // Actual offset applied to camera
    private Quaternion baseRotation = Quaternion.identity;     // Optional base rotation

    void Start()
    {
        baseRotation = transform.localRotation; // Optional: store if needed
    }

    void Update()
    {
        // Gradually return recoilRotation to neutral
        recoilRotation = Quaternion.Slerp(recoilRotation, Quaternion.identity, returnSpeed * Time.deltaTime);

        // Smoothly follow the recoil target
        kickbackRotation = Quaternion.Slerp(kickbackRotation, recoilRotation, recoilSharpness * Time.deltaTime);

        // Apply final rotation (additive)
        transform.localRotation = baseRotation * kickbackRotation;
    }

    public void AddRecoil(Vector3 rotationEuler)
    {
        Quaternion recoil = Quaternion.Euler(rotationEuler);
        recoilRotation *= recoil; // Accumulate the recoil
    }
}
