using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject powerUpPrefab;
    [SerializeField]
    private float powerUpYPosition = 1.7f;
    [SerializeField]
    private float powerUpXPosition = -52.71f;
    [SerializeField]
    private int healthBoost = 20;
    private void OnEnable()
    {
        GunShipDeathManager.OnDied += SpawnPowerUp;
    }
    private void OnDisable()
    {

        GunShipDeathManager.OnDied -= SpawnPowerUp;
    }

    private void SpawnPowerUp(GameObject _gameObject)
    {

        var _powerUp = Instantiate(powerUpPrefab, new Vector3(powerUpXPosition, powerUpYPosition, _gameObject.transform.position.z), Quaternion.identity);
        _powerUp.GetComponent<PowerUpManager>().HealthBoost = healthBoost;
    }
}
