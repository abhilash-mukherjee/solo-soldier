using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AggroDetection : MonoBehaviour
{
    public delegate void AggroHandler(Transform transform);
    public static event AggroHandler OnAggro;
    public delegate void FireHandler();
    public static event FireHandler OnAggroFire;
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerMovement>();
        if(player != null)
        {
            Debug.Log("Aggroed");
            OnAggro?.Invoke(player.transform);
            OnAggroFire?.Invoke();
        }
    }
}
