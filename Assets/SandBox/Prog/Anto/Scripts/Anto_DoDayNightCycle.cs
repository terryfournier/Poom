using UnityEngine;

public class Anto_DoDayNightCycle : MonoBehaviour
{
    [SerializeField] private float cycleSpeed = 0f;

    // Update is called once per frame
    private void Update()
    {
        transform.Rotate(cycleSpeed * Vector3.right * Time.deltaTime);
    }
}
