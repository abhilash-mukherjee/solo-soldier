using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTransformPositionWithInputSmoothlyAndRotation : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private float speed = 5f, turnSpeed = 30f;
    void Update()
    {
        if(Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += speed * Time.deltaTime * transform.right;
        }

        if(Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += speed * Time.deltaTime * -transform.right;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += speed * Time.deltaTime * transform.forward;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += speed * Time.deltaTime * -transform.forward;
        }
        if(Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(turnSpeed * Time.deltaTime * Vector3.up);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(turnSpeed * Time.deltaTime * Vector3.down);
        }
    }
}
