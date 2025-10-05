using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{

    public enum AxisEndConv
    {
        x,
        y,
        z
    }
    public GameObject belt;

    public Transform startPos;
    public Transform endPos;

    public AxisEndConv axisEndConv;

    float distancePlayer = 1.5f;

    private Vector3 dir;

    private float direction;

    private PlayerController playerController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dir = transform.position - endPos.position;

        playerController = FindAnyObjectByType<PlayerController>();

        switch (axisEndConv)
        {
            case AxisEndConv.x:
                direction = startPos.position.x - endPos.position.x;
                Mathf.Clamp(direction, -1, 1);
                if (direction < 0)
                {
                    direction = -1;
                }
                else
                {
                    direction = 1;
                }
                break;
            case AxisEndConv.y:
                direction = startPos.position.y - endPos.position.y;
                Mathf.Clamp(direction, -1, 1);

                if (direction < 0)
                {
                    direction = -1;
                }
                else
                {
                    direction = 1;
                }
                break;
            case AxisEndConv.z:
                direction = startPos.position.z - endPos.position.z;
                Mathf.Clamp(direction, -1, 1);

                if (direction < 0)
                {
                    direction = -1;
                }
                else
                {
                    direction = 1;
                }
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (axisEndConv)
        {
            case AxisEndConv.x:
                transform.position -= new Vector3(dir.normalized.x * 2 * Time.deltaTime, 0, 0);
                if (Vector3.Distance(playerController.transform.position, transform.position) <= distancePlayer)
                {
                    Vector3 dirPlayer = playerController.transform.position - startPos.position;
                    Vector3 temp = Vector3.zero;
                    temp.x += (dirPlayer.normalized.x * 20000 * Time.deltaTime);
                    playerController.VelocityAdd = temp;
                }
                if (direction == -1)
                {
                    if (transform.position.x >= endPos.position.x)
                    {
                        //GameObject go = Instantiate(belt);
                        //go.transform.position = startPos.position;
                        //go.transform.rotation = transform.rotation;
                        //go.transform.localScale = transform.localScale;

                        //ConveyorBelt temp = go.GetComponent<ConveyorBelt>();
                        //temp.axisEndConv = axisEndConv;
                        //temp.startPos = startPos;
                        //temp.endPos = endPos;
                        //temp.belt = belt;

                        //Destroy(gameObject);
                        transform.position = startPos.position;
                    }
                }
                else
                {
                    if (Mathf.Abs(transform.position.x) <= Mathf.Abs(endPos.position.x))
                    {
                        transform.position = startPos.position;
                    }
                }

                break;
            case AxisEndConv.y:
                //transform.position -= new Vector3(0, dir.normalized.y * 2 * Time.deltaTime, 0);
                //if (Vector3.Distance(playerController.transform.position, transform.position) <= distancePlayer)
                //{
                //    Vector3 dirPlayer = playerController.transform.position - startPos.position;
                //    Vector3 temp = Vector3.zero;
                //    temp.x += (dirPlayer.normalized.y * 20000 * Time.deltaTime);
                //    playerController.VelocityAdd = temp;
                //}
                //if (direction == -1)
                //{
                //    if (transform.position.y >= endPos.position.y)
                //    {
                //        transform.position = startPos.position;
                //    }
                //}
                //else
                //{
                //    if (Mathf.Abs(transform.position.y) <= Mathf.Abs(endPos.position.y))
                //    {
                //        transform.position = startPos.position;
                //    }
                //}
                break;
            case AxisEndConv.z:
                transform.position -= new Vector3(0, 0, dir.normalized.z * 2 * Time.deltaTime);
                if (Vector3.Distance(playerController.transform.position, transform.position) <= distancePlayer)
                {
                    Vector3 dirPlayer = playerController.transform.position - startPos.position;
                    Vector3 temp = Vector3.zero;
                    temp.z += (dirPlayer.normalized.z * 20000 * Time.deltaTime);
                    playerController.VelocityAdd = temp;
                }
                if (direction == -1)
                {
                    if (transform.position.z >= endPos.position.z)
                    {
                        transform.position = startPos.position;
                    }
                }
                else
                {
                    if (Mathf.Abs(transform.position.z) <= Mathf.Abs(endPos.position.z))
                    {
                        transform.position = startPos.position;
                    }
                }
                break;
            default:
                break;
        }
    }
}
