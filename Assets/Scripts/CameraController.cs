using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraController : MonoBehaviour
{
    [SerializeField]
    float sensetivity = 1;
    [SerializeField]
    float maxYCamera = 4, minYCamera = -4;
    private CinemachineComposer composer;

    private void Start()
    {
        composer = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineComposer>();
    }

    private void Update()
    {
        
        float vertical = Input.GetAxis("Mouse Y") * sensetivity;
        composer.m_TrackedObjectOffset.y += vertical;
        composer.m_TrackedObjectOffset.y = Mathf.Clamp(composer.m_TrackedObjectOffset.y, minYCamera, maxYCamera);
    }
}
