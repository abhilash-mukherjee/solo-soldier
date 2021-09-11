using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    [SerializeField]
    private WheelProperties[] leftWheels;
    [SerializeField]
    private WheelProperties[] rightWheels;
    [SerializeField]
    private WheelProperties[] leftUselessWheels;
    [SerializeField]
    private WheelProperties[] rightUselessWheels;
    [SerializeField]
    float maxMotorTorque = 100f;
    [SerializeField]
    float maxStreerAngle = 30f;
    [SerializeField]
    float breakForce = 2000f;
    [SerializeField]
    private float turnMultiplier = 0.5f;
    private float m_HorizontalInput;
    private float m_VerticalInput;
    private bool m_ShouldBreak = false;

    [System.Serializable]
    public class WheelProperties
    {
        public WheelCollider wheelCollider;
        public Transform wheelTransform;
    }
    private void GetInput()
    {
        m_HorizontalInput = Input.GetAxis("Horizontal");
        m_VerticalInput = Input.GetAxis("Vertical");
        m_ShouldBreak = Input.GetKey(KeyCode.Space);
    }
    private void Accelerate()
    {
        float rightTurnMultiplier = 0;
        float leftTurnMultiplier = 0;
        if (m_HorizontalInput > 0 && m_VerticalInput > 0)
        {
            leftTurnMultiplier = turnMultiplier;
            rightTurnMultiplier = 0;
        }
        else if (m_HorizontalInput < 0 && m_VerticalInput > 0)
        {
            leftTurnMultiplier = 0;
            rightTurnMultiplier = -turnMultiplier;
        }
        else if (m_HorizontalInput < 0 && m_VerticalInput < 0)
        {
            leftTurnMultiplier = 0;
            rightTurnMultiplier = turnMultiplier;
        }
        else if (m_HorizontalInput > 0 && m_VerticalInput < 0)
        {
            leftTurnMultiplier = -turnMultiplier;
            rightTurnMultiplier = 0;
        }
        foreach (WheelProperties _wheel in rightWheels)
        {
            _wheel.wheelCollider.motorTorque = maxMotorTorque * (m_VerticalInput + m_HorizontalInput * rightTurnMultiplier);
        }
        foreach (WheelProperties _wheel in leftWheels)
        {
            _wheel.wheelCollider.motorTorque = maxMotorTorque * (m_VerticalInput + m_HorizontalInput * leftTurnMultiplier);
        }
    }
    private void Break()
    {
        foreach (WheelProperties _wheel in rightWheels)
        {
            if (m_ShouldBreak == true)
                _wheel.wheelCollider.brakeTorque = breakForce;
            else
                _wheel.wheelCollider.brakeTorque = 0;
        }
        foreach (WheelProperties _wheel in leftWheels)
        {
            if (m_ShouldBreak == true)
                _wheel.wheelCollider.brakeTorque = breakForce;
            else
                _wheel.wheelCollider.brakeTorque = 0;
        }
    }
    //private void Steer()
    //{
    //    foreach(WheelProperties _wheel in rightWheels)
    //    {
    //        _wheel.wheelCollider.steerAngle = maxStreerAngle * m_HorizontalInput;
    //    }
    //    foreach (WheelProperties _wheel in leftWheels)
    //    {
    //        _wheel.wheelCollider.steerAngle = maxStreerAngle * m_HorizontalInput;
    //    }
    //}
    private void UpdateWheelPose()
    {
        foreach (WheelProperties _wheel in rightWheels)
        {
            UpdateWheelPose(_wheel.wheelCollider, _wheel.wheelTransform);
        }
        foreach (WheelProperties _wheel in leftWheels)
        {
            UpdateWheelPose(_wheel.wheelCollider, _wheel.wheelTransform);
        }
    }
    private void UpdateWheelPose(WheelCollider _wheelCollider, Transform _transform)
    {
        Vector3 _pos = _transform.transform.position;
        Quaternion _quat = _transform.transform.rotation;
        _wheelCollider.GetWorldPose(out _pos, out _quat);
        _transform.position = _pos;
        _transform.rotation = _quat;
    }
    private void FixedUpdate()
    {
        GetInput();
        Accelerate();
        Break();
        // Steer();
        UpdateWheelPose();
    }
}
