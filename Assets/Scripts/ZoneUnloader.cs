using UnityEngine;
using UnityEngine.Rendering;

public class ZoneUnloader : MonoBehaviour
{
    [SerializeField] GameObject unloader;

    private void OnTriggerEnter(Collider other)
    {

        Transform[] tabTransf = unloader.GetComponentsInChildren<Transform>();

        for (int i = 0; i < tabTransf.Length; i++)
        {
            Destroy(tabTransf[i].gameObject);
        }
        Destroy(unloader);

       GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);

        go.transform.position = transform.position;
        go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y, go.transform.position.z - 15);
        go.transform.localScale = new Vector3(5, 5, 5);

        Destroy(gameObject);
    }
}
