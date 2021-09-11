using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public delegate void HitHandler(GameObject gameObject);
    public delegate void PlayerHitHandler(GameObject gameObject, int dmg);
    public delegate void HealthHandler(GameObject gameObject, int currentHealth, int startingHealth);
    public static event HealthHandler OnHealthChanged;
    public delegate void DeathHandler(GameObject gameObject);
    public static event PlayerHitHandler OnPlayerHit;
    public static event DeathHandler OnDied;
    public delegate void GrenadeCountHandler(int _grenadeCount);
    public static event GrenadeCountHandler OnDied_GrenadeCount;
    [SerializeField]
    private int startingHealth = 10;
    private int currentHealth;
    [HideInInspector]
    public int CurrentHealth
    {
        get { return currentHealth; }
    }

    private void Awake()
    {
        currentHealth = startingHealth;
    }

    public void TakeDamage( int dmg)
    {
        if(dmg > 2)
        {
            GetComponent<PlayerGun>().StopFiring();
        }
        currentHealth -= dmg;
        OnHealthChanged?.Invoke(gameObject, currentHealth, startingHealth);
        if (currentHealth <= 0)
        {
            GetComponent<PlayerMovement>().StopPlayerMovement();
            GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemyList)
            {
                if (enemy.GetComponent<EnemyGun>() != null)
                    enemy.GetComponent<EnemyGun>().StopFiringWhenPlayerDies();
            }

            GameObject[] gunShipList = GameObject.FindGameObjectsWithTag("GunShip");
            foreach (GameObject gunShip in gunShipList)
            {
                if(gunShip.GetComponent<GunShipGun>() != null)
                    gunShip.GetComponent<GunShipGun>().StopFiringWhenPlayerDies();
            }
            
            GameObject[] tankList = GameObject.FindGameObjectsWithTag("Tank");
            foreach (GameObject tank in tankList)
            {
                if(tank.GetComponent<TankGunController>() != null)
                    tank.GetComponent<TankGunController>().StopFiringWhenPlayerDies();
            }
            GetComponent<PlayerGun>().StopFiring();
            Die();
            return;
        }
        OnPlayerHit?.Invoke(gameObject,dmg);
    }

    private void Die()
    {
        OnDied_GrenadeCount?.Invoke(gameObject.GetComponent<PlayerGrenadeCounter>().GrenadeCount);
        OnDied?.Invoke(gameObject);
    }
}
