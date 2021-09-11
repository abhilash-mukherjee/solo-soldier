using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AggroDetection : MonoBehaviour
{
    public delegate void AggroHandler(Transform playertransform, GameObject thisEnemy);
    public static event AggroHandler OnAggro;
    public delegate void FireHandler(GameObject thisEnemy);
    public static event FireHandler OnAggroFire;
    private void OnEnable()
    {
        EnemyHealth.OnAggroHit += InvokeAggro;
    }
    private void OnDisable()
    {
        EnemyHealth.OnAggroHit -= InvokeAggro;
        
    }

    private void InvokeAggro(GameObject _gameObject)
    {
        if(_gameObject == gameObject)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if(player != null)
            {
                OnAggro?.Invoke(player.transform, gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerMovement>();
        if(player != null)
        {
            OnAggro?.Invoke(player.transform, gameObject);
            OnAggroFire?.Invoke(gameObject);
        }
    }
}
