using UnityEngine;

public class MoveCameraCinématique : MonoBehaviour
{
    [SerializeField] Transform startPos;
    [SerializeField] Transform endPos;

    private bool isMoveTrigger;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isMoveTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            isMoveTrigger = true;
        }

        if(isMoveTrigger)
        {
            transform.position = Vector3.Lerp(transform.position, endPos.position, Time.deltaTime);
        }
    }
}
