using UnityEngine;

public class LightLinkedDoor : MonoBehaviour
{
    [SerializeField] private KeyBehaviour key;
    [SerializeField] private Material appliedMat;
    private Light lightComponent;

    private void Start()
    {
        lightComponent = GetComponentInChildren<Light>();
        RedLight();

        if (key != null)
            key.OnKeyTaken += GreenLight;
    }


    private void Update()
    {
        lightComponent.intensity = Mathf.PingPong(Time.time, 1.0f) / 500 + 0.001f;
    }

    private void OnDestroy()
    {
        if (key != null)
            key.OnKeyTaken -= GreenLight;
    }

    private void GreenLight()
    {
        lightComponent.color = new Color(25, 200, 25, 255);
        appliedMat.SetColor("_EmissionColor", new Color(25, 200, 25, 255));
    }

    private void RedLight()
    {
        lightComponent.color = new Color(200, 25, 25, 255);
        appliedMat.SetColor("_EmissionColor", new Color(200, 25, 25, 255));
    }
}
