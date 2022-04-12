using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShipSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject gunShipPrefab;
    [SerializeField]
    private Vector3 prefabSpawnPosition;
    private void OnEnable()
    {
        GunShipTimer.OnGunShipTimerEnd += SpawnGunShip;
    }
    private void OnDisable()
    {
        
        GunShipTimer.OnGunShipTimerEnd -= SpawnGunShip;
    }

    private void SpawnGunShip()
    {
        Instantiate(gunShipPrefab, prefabSpawnPosition, Quaternion.identity);
        if (GameCanvasController.CanvasList.Count != 0)
        {
            var timer = GameCanvasController.CanvasList[0].transform.GetComponentInChildren<GunShipTimerManager>();
            if (timer != null)
            {
                timer.gameObject.SetActive(false);
            }
        }

    }
}
