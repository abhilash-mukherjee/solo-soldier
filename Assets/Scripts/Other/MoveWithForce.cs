using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithForce : MonoBehaviour
{
    // Start is called before the first frame update

    Rigidbody rigidbody;
    [SerializeField]
    private float moveForce = 10f;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.W))
        {
            rigidbody.AddForce(moveForce * Vector3.forward);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rigidbody.AddForce(moveForce * Vector3.back);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rigidbody.AddForce(moveForce * Vector3.left);
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            rigidbody.AddForce(moveForce * Vector3.right);
        }
    }
}
