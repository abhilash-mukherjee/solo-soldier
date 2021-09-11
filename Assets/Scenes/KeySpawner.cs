using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject keyPrefab;
    [SerializeField]
    private Vector3 keySpanPosition;
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
        Instantiate(keyPrefab, keySpanPosition, Quaternion.identity);
    }
}
