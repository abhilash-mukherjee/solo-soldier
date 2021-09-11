using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShieldMovementController : MonoBehaviour
{
    [SerializeField]
    private GameObject tank;
    private void Update()
    {
        if(tank != null)
            transform.position = tank.transform.position;
    }
}
