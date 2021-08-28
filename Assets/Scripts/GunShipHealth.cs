using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShipHealth : MonoBehaviour
{
    public delegate void GunShipDeathHandler();
    public static event GunShipDeathHandler OnGunShipDied;
    public delegate void HealthHandler(GameObject gameObject, int currentHealth, int startingHealth);
    public static event HealthHandler OnHealthChanged;

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



    private int m_currentHealth;
    public int CurrentHealth
    {
        get { return m_currentHealth; }
    }


    private ParticleSystem smoke;
    private Transform transformWhenHit;
    private bool shouldSimulateHit = false;
    private int updateCalls =0;
    private Rigidbody body;
    private void Awake()
    {
        smoke = smokeParticleObject.GetComponentInChildren<ParticleSystem>();
        m_currentHealth = startingHealth;
    }


    public void TakeDamage(int dmg, Vector3 hitPosition)
    {
        transformWhenHit = transform; 
        m_currentHealth -= dmg;
        OnHealthChanged?.Invoke(gameObject, m_currentHealth, startingHealth);
        Instantiate(smoke, hitPosition, Quaternion.identity);
        GetComponent<Animator>().Play("GunshipHit");
        if (shouldSimulateHit == false)
            shouldSimulateHit = true;
        if (m_currentHealth <= 0)
        {
            GetComponent<GunShipGun>().StopFiringWhenGunShipDies();
            Die();
            return;
        }
    }
    public void TakeDamage(int dmg)
    {
        transformWhenHit = transform;
        m_currentHealth -= dmg;
        OnHealthChanged?.Invoke(gameObject, m_currentHealth, startingHealth);
        GetComponent<Animator>().Play("GunshipHit");
        if (shouldSimulateHit == false)
            shouldSimulateHit = true;
        if (m_currentHealth <= 0)
        {
            GetComponent<GunShipGun>().StopFiringWhenPlayerDies();
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
        OnGunShipDied?.Invoke();
    }
}
