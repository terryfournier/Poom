using UnityEngine;
using UnityEngine.UI;

public class BloodSpot : MonoBehaviour
{
    private Vector3 originalPosition;

    private float maxDurationOnScreen;
    public float currentDurationOnScreen;

    private Image image;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalPosition = transform.position;
        maxDurationOnScreen = 3.0f;
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        currentDurationOnScreen += Time.deltaTime;

        transform.position = new Vector3(transform.position.x, transform.position.y - 20 * Time.deltaTime, transform.position.z);
        image.color = new Color(image.color.r, image.color.b, image.color.b, 1 - (currentDurationOnScreen / maxDurationOnScreen));

        if(currentDurationOnScreen >= maxDurationOnScreen)
        {
            gameObject.SetActive(false);
            transform.position = originalPosition;
        }
    }
}
