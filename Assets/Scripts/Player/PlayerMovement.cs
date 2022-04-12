using System;
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
    [SerializeField]
    private BoolVariable isGamePaused;
    private bool shouldMove = true;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        LockCursor();
    }

    private static void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        PlayerHealth.OnDied += UnlockCursor;
        GameManager.OnKeyCollected += UnlockCursor;
        isGamePaused.Value = false;
    }
    private void OnDisable()
    {
        PlayerHealth.OnDied -= UnlockCursor;
        GameManager.OnKeyCollected -= UnlockCursor;
    }

    private void UnlockCursor(GameObject _gameObject)
    {
        Cursor.lockState = CursorLockMode.None;
    }
    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void StopPlayerMovement()
    {
        shouldMove = false;
    }
    public void StartPlayerMovement()
    {
        shouldMove = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (isGamePaused.Value == true) UnlockCursor();
        else LockCursor();
        if (shouldMove == false)
            return;
        float aim = Input.GetAxis("Mouse X");
        Debug.Log("Aim = " + aim);
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(horizontal, 0, vertical);
        if(!IsPlaying(animator,"Fire"))
        {
            animator.SetFloat("Speed", vertical);
        }
        transform.Rotate(Vector3.up, aim * turnSpeed * Time.deltaTime);
        if(vertical != 0 && !IsPlaying(animator, "Hit"))
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


