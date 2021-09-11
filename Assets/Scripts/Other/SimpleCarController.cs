using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCarController : MonoBehaviour
{
    [SerializeField]
    private WheelCollider frontRightCollider, frontLeftCollider;
    [SerializeField]
    private WheelCollider backRightCollider, backLeftCollider;
    [SerializeField]
    Transform frontRightWheel, frontLeftWheel, backRightWheel, backLeftWheel;
    [SerializeField]
    private float maxMotorTorque = 50f;
    [SerializeField]
    private float maxBreakTorque = 30f;
    [SerializeField]
    private float maxStreerAngle = 30f;
    private float m_HorizontalInput;
    private float m_VerticalInput;
    private bool m_ShouldBreak = false;
    private void GetInput()
    {
        m_HorizontalInput = Input.GetAxis("Horizontal");
        m_VerticalInput = Input.GetAxis("Vertical");
        m_ShouldBreak = Input.GetKey(KeyCode.Space);
    }
    private void Accelerate()
    {
        frontLeftCollider.motorTorque = m_VerticalInput * maxMotorTorque;
        frontRightCollider.motorTorque = m_VerticalInput *maxMotorTorque;
    }
    private void Steer()
    {
        frontLeftCollider.steerAngle = m_HorizontalInput * maxStreerAngle;
        frontRightCollider.steerAngle = m_HorizontalInput * maxStreerAngle;
    }
    private void Break()
    {
        if(m_ShouldBreak == true)
        {
            frontLeftCollider.brakeTorque = maxBreakTorque;
            frontRightCollider.brakeTorque =  maxBreakTorque;
        }

        else
        {
            frontLeftCollider.brakeTorque = 0;
            frontRightCollider.brakeTorque = 0;
        }
        
    }
    private void UpdateWeelPose()
    {
        UpdateWeelPose(frontRightCollider,frontRightWheel);
        UpdateWeelPose(frontLeftCollider,frontLeftWheel);
        UpdateWeelPose(backRightCollider,backRightWheel);
        UpdateWeelPose(backLeftCollider,backLeftWheel);
    }
    private void UpdateWeelPose(WheelCollider _collider, Transform _transform)
    {
        Vector3 _pos = _transform.position;
        Quaternion _quat = transform.rotation;
        _collider.GetWorldPose(out _pos, out _quat);
        _transform.position = _pos;
        _transform.rotation = _quat;

    }
  
    private void FixedUpdate()
    {
        GetInput();
        Accelerate();
        Break();
        Steer();
        UpdateWeelPose();
        
    }

}
