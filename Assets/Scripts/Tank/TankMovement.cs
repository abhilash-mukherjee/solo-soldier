using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour
{
    private GameObject target;
    [SerializeField]
    private GameObject tankBarrel;
    [SerializeField]
    private GameObject mainGun;
    [SerializeField]
    private float yOffsetForMainGunDirection = 1.5f;
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
    float maxMotorTorqueToMoveAwayFromPlayer = 500f;
    [SerializeField]
    float breakForce = 2000f;
    [SerializeField]
    private float turnMultiplier = 0.5f;
    [SerializeField]
    private float maximumXZSeperationBetweenPlayerAndTank = 35f;
    [SerializeField]
    private float minimumXZSeperationBetweenPlayerAndTank = 10f;
    [SerializeField]
    private float maxZPosition = 55f, minZPosition = -80f;
    [HideInInspector]
    public bool isMoving = false;
    private float m_VerticalMovement;
    private bool m_ShouldBreak = false;
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }


    [System.Serializable]
    public class WheelProperties
    {
        public WheelCollider wheelCollider;
        public Transform wheelTransform;
    }
 
    private void Accelerate(float _maxMotorTorque)
    {
        foreach (WheelProperties _wheel in rightWheels)
        {
            _wheel.wheelCollider.motorTorque = _maxMotorTorque * m_VerticalMovement;
        }
        foreach(WheelProperties _wheel in leftWheels)
        {
            _wheel.wheelCollider.motorTorque = _maxMotorTorque * m_VerticalMovement;
        }
    }
    private void Break()
    {
        foreach(WheelProperties _wheel in rightWheels)
        {
            if(m_ShouldBreak == true)
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

    private void MoveTowardsTarget()
    {
        float _distance = GunTargetDistanceAlongXZPlane(transform, target.transform);

        if ((transform.position.z >= maxZPosition || transform.position.z <= minZPosition) && _distance < maximumXZSeperationBetweenPlayerAndTank)
        {
            m_VerticalMovement = 0;
            isMoving = false;
        }

        else if (_distance > maximumXZSeperationBetweenPlayerAndTank)
        {
            m_ShouldBreak = false;
            m_VerticalMovement = target.transform.position.z - transform.position.z > 0 ? 1 : -1;
            Accelerate(maxMotorTorque);
            Break();
            isMoving = true;
        }
        else if (_distance < maximumXZSeperationBetweenPlayerAndTank && _distance > minimumXZSeperationBetweenPlayerAndTank)
        {
            m_ShouldBreak = true;
            m_VerticalMovement = 0;
            Accelerate(maxMotorTorque);
            Break();
            isMoving = false;
        }
        else if ( _distance < minimumXZSeperationBetweenPlayerAndTank)
        {
            m_ShouldBreak = false;
            m_VerticalMovement = target.transform.position.z - transform.position.z > 0 ? -1 : 1;
            Accelerate(maxMotorTorqueToMoveAwayFromPlayer);
            Break();
            isMoving = true;
        }
    }
    private float GunTargetDistanceAlongXZPlane(Transform _firingObject, Transform _target)
    {
        return Mathf.Sqrt(
            Mathf.Pow((_firingObject.position.x - _target.transform.position.x), 2f)
            + Mathf.Pow((_firingObject.position.z - _target.transform.position.z), 2f)
            );
    }
    private void LookAtTarget()
    {
        Quaternion newBarrelDirection = Quaternion.LookRotation(
            new Vector3(target.transform.position.x - tankBarrel.transform.position.x,
            0,
            target.transform.position.z - tankBarrel.transform.position.z));
        Quaternion newGunDirection = Quaternion.LookRotation(target.transform.position - mainGun.transform.position +
            new Vector3(0, yOffsetForMainGunDirection, 0));
        tankBarrel.transform.rotation = newBarrelDirection;
        mainGun.transform.rotation = newGunDirection;
    }
    private void Update()
    {
        LookAtTarget();
        MoveTowardsTarget();
    }
}
