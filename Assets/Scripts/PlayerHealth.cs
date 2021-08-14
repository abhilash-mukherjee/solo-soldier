using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public delegate void HitHandler(GameObject gameObject);
    public delegate void DeathHandler(GameObject gameObject);
    public static event HitHandler OnHit;
    public static event DeathHandler OnDied;
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
        Debug.Log("Health = "+ currentHealth);
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
                if(gunShip.GetComponent<GunShipFire>() != null)
                    gunShip.GetComponent<GunShipFire>().StopFiringWhenPlayerDies();
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
        OnHit?.Invoke(gameObject);
    }

    private void Die()
    {
        OnDied?.Invoke(gameObject);
    }
}
