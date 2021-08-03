using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private CharacterController characterController;
    private Animator animator;
    [SerializeField]
    private float forwardSpeed = 400f;
    [SerializeField]
    private float backwardSpeed = 100f;
    [SerializeField]
    private float turnSpeed = 200f;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Mouse X");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontal, 0, vertical);
        if(!IsPlaying(animator,"Fire"))
        {
            animator.SetFloat("Speed", vertical);
        }
        transform.Rotate(Vector3.up, horizontal * turnSpeed * Time.deltaTime);
        if(vertical != 0)
        {
            float speed = (vertical > 0) ? forwardSpeed : backwardSpeed;
            characterController.SimpleMove(vertical * transform.forward * Time.deltaTime * speed);
        }

    }
    bool IsPlaying(Animator anim, string stateName)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(stateName) &&
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            return true;
        else
            return false;
    }
}
