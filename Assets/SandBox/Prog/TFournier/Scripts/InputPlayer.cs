using System;
using UnityEngine;

public class InputPlayer : MonoBehaviour
{
    float lifePoints = 50;
    
    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Input.GetAxisRaw("Vertical") * Vector3.forward * 0.1f;
        direction += Input.GetAxisRaw("Horizontal") * Vector3.right * 0.1f;
        
        transform.position += direction;
    }
}
