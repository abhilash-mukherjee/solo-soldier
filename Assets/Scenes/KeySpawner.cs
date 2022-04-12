using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject keyPrefab;
    [SerializeField]
    private Vector3 keySpawnPosition;
    public static bool isKeySpawned = false;
    private void OnEnable()
    {
        GameManager.OnLevelFinished += SpawnKey;
    }
    private void OnDisable()
    {
        GameManager.OnLevelFinished -= SpawnKey;
        
    }

    private void SpawnKey()
    {
        Instantiate(keyPrefab, keySpawnPosition, Quaternion.identity);
        isKeySpawned = true;
    }
}
