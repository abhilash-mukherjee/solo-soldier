using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShipHealth : MonoBehaviour
{
    [SerializeField]
    private GameObject smokeParticleObject;
    [SerializeField]
    private float xRotationMultiplier = 0.1f;
    [SerializeField]
    private float hitForce = 0.1f;
    [SerializeField]
    private int updateCallsToAchieveSmoothHitAnimation;
    [SerializeField]
    private int startingHealth = 5;
    private int currentHealth;
    private ParticleSystem smoke;
    private Transform transformWhenHit;
    private bool shouldSimulateHit = false;
    private int updateCalls =0;
    private Rigidbody body;
    private void Awake()
    {
        smoke = smokeParticleObject.GetComponentInChildren<ParticleSystem>();
        currentHealth = startingHealth;
    }

    public void TakeDamage(int dmg, Vector3 hitPosition)
    {
        transformWhenHit = transform;
        currentHealth -= dmg;
        Instantiate(smoke, hitPosition, Quaternion.identity);
        GetComponent<Animator>().Play("GunshipHit");
        if (shouldSimulateHit == false)
            shouldSimulateHit = true;
        if (currentHealth <= 0)
        {
            GetComponent<EnemyGun>().StopFiringWhenPlayerDies();
            Die();
            return;
        }
    }
    public void OnGunshipHitAnimationPlayed()
    {
        GetComponent<Animator>().Play("GunshipFly");
    }

    private void Die()
    {
    }
}
