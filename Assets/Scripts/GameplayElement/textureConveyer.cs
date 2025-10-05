using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class textureConveyer : MonoBehaviour
{
    [SerializeField] private Material conveyorMaterial;


    //List of entities on the conveyor
    private List<GameObject> entitiesOnConveyor = new List<GameObject>();
    private List<GameObject> removeList = new List<GameObject>();

    [SerializeField] private float conveyorSpeed = 0.2f;
    [SerializeField] private float textureSlow = 0.0f;

    private void Start()
    {
        // Set the conveyor material to the conveyor
        if (conveyorMaterial == null)
        {
            Debug.LogError("Conveyor material is not assigned.");
            return;
        }
        // Set the conveyor material to the conveyor
        GetComponent<Renderer>().material = conveyorMaterial;

    }

    void Update()
    {
        conveyorMaterial.mainTextureOffset += new Vector2(conveyorSpeed * textureSlow * Time.deltaTime, 0);
        // Move all entities on the conveyor
        foreach (GameObject entity in entitiesOnConveyor)
        {
            // Move the entity forward
            if (entity == null)
            {
                removeList.Add(entity);
            }
            else
            {
                entity.transform.position -= transform.right * conveyorSpeed * Time.deltaTime;
            }
        }


        foreach (GameObject entity in removeList)
        {
            entitiesOnConveyor.Remove(entity);
        }
        removeList.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        //check if the object is not already in the conveyor list
        if (entitiesOnConveyor.Contains(other.gameObject))
            return;

        if ((other.gameObject.CompareTag("Player")))
        {
            entitiesOnConveyor.Add(other.gameObject);
            return;
        }

        if (other.GetComponentInParent<NavMeshAgent>())
        {
            entitiesOnConveyor.Add(other.GetComponentInParent<NavMeshAgent>().gameObject);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (entitiesOnConveyor.Contains(other.gameObject))
        {
            entitiesOnConveyor.Remove(other.gameObject);
            return;
        }
        NavMeshAgent ouioui;
        ouioui = other.GetComponentInParent<NavMeshAgent>();
        //check if the object is not already in the conveyor list
        if (ouioui == null)
            return;
        if (entitiesOnConveyor.Contains(ouioui.gameObject))
        {
            entitiesOnConveyor.Remove(ouioui.gameObject);
        }

    }
}

