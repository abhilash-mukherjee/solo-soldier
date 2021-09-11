using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    float sensetivity = 1;
    [SerializeField]
    float maxYCamera = 4, minYCamera = -4;
    private CinemachineComposer composer;
    private bool m_stopCameraRotation = false;

    private void Start()
    {
        composer = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineComposer>();
    }

    private void OnEnable()
    {
        PlayerHealth.OnDied += StopCameraMovement;
        GameManager.OnKeyCollected += StopCameraMovement;
    }
    private void OnDisable()
    {
        PlayerHealth.OnDied -= StopCameraMovement;
        GameManager.OnKeyCollected -= StopCameraMovement;
        
    }

    private void StopCameraMovement(GameObject _gameObject)
    {
        m_stopCameraRotation = true;
    }
    private void StopCameraMovement()
    {
        m_stopCameraRotation = true;
    }

    private void Update()
    {
        if (m_stopCameraRotation)
            return;
        float vertical = Input.GetAxis("Mouse Y") * sensetivity;
        composer.m_TrackedObjectOffset.y += vertical;
        composer.m_TrackedObjectOffset.y = Mathf.Clamp(composer.m_TrackedObjectOffset.y, minYCamera, maxYCamera);
    }
}
