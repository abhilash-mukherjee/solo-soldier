using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private AggroDetection aggroDetection;
    private NavMeshAgent navMeshAgent ;
    private Animator animator;
    private Transform target;
    
    private void Awake()
    {
        aggroDetection = GetComponent<AggroDetection>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }
    private void OnEnable()
    {
        AggroDetection.OnAggro += AggroDetection_OnAggro;
    }
    private void OnDisable()
    {
        AggroDetection.OnAggro -= AggroDetection_OnAggro;
    }
    private void AggroDetection_OnAggro(Transform target, GameObject whichEnemy)
    {
        if(whichEnemy == gameObject) // This means that the AggroDetection script attached with this gameObject was triggered
        {
            this.target = target;
        }
        
        
    }
    private void Update()
    {
        if(target != null)
        {
            navMeshAgent.SetDestination(target.position);
            gameObject.transform.LookAt(target);
            float velocity = navMeshAgent.velocity.magnitude;
            animator.SetFloat("Speed", velocity);
        }
    }
}
