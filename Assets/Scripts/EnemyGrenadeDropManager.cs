using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGrenadeDropManager : MonoBehaviour
{
    [SerializeField]
    private GameObject grenadePrefab;
    [SerializeField]
    private Vector3 grenadeSpawnOffset = new Vector3(0f, 2f, 0f);
    private void OnEnable()
    {
        EnemyDamage.OnEnemyDieAnimationFinishedPlaying += SpawnGrenade;
    }
    private void OnDisable()
    {
        EnemyDamage.OnEnemyDieAnimationFinishedPlaying -= SpawnGrenade;
        
    }

    private void SpawnGrenade(GameObject _enemyObject)
    {
        if(transform.GetChild(0).gameObject == _enemyObject)
        {
            Vector3 _spawnPosition = transform.position + grenadeSpawnOffset;
            GameObject _prefab = Instantiate(grenadePrefab,_spawnPosition, new Quaternion(3f,3f,2f,9f));
            Destroy(_prefab.GetComponent<GrenadeExplosionManager>());
            _prefab.AddComponent<Rigidbody>();
        }
    }
}
