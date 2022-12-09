using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public delegate void HitHandler(GameObject gameObject);
    public delegate void PlayerHitHandler(GameObject gameObject, int dmg);
    public delegate void HealthHandler(GameObject gameObject, int currentHealth, int startingHealth);
    public static event HealthHandler OnHealthChanged;
    public delegate void DeathHandler(GameObject gameObject);
    public delegate void PlayerDeathHandler();
    public static event PlayerHitHandler OnPlayerHit;
    public static event DeathHandler OnDied;
    public static event PlayerDeathHandler OnPlayerDied;
    public delegate void GrenadeCountHandler(int _grenadeCount);
    public static event GrenadeCountHandler OnDied_GrenadeCount;
    [SerializeField]
    private int startingHealth = 10;
    [SerializeField] private IntVariable currentHealth;
    private int m_currentHealth;
    [HideInInspector]
    public int CurrentHealth
    {
        get { return m_currentHealth; }
        set
        {
            if (value >= 100)
            {
                m_currentHealth = 100;
            }
            else if (value <= 0)
            {
                m_currentHealth = 0;
            }
            else
            {
                m_currentHealth = value;

            }
            OnHealthChanged?.Invoke(gameObject, m_currentHealth, startingHealth);
        }
    }

    private void Awake()
    {
        m_currentHealth = startingHealth;
        currentHealth.Value = startingHealth;
    }
    private void Start()
    {
        Debug.Log($"Player health = {m_currentHealth}");
        
    }
    private void OnDestroy()
    {
        Debug.Log("Inside OnDestroy of player");
    }
    public void TakeDamage(int dmg)
    {
        if (dmg > 2)
        {
            GetComponent<PlayerGun>().StopFiring();
        }
        m_currentHealth -= dmg;
        currentHealth.Value = m_currentHealth;
        if (m_currentHealth <= 0)
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
                if (gunShip.GetComponent<GunShipGun>() != null)
                    gunShip.GetComponent<GunShipGun>().StopFiringWhenPlayerDies();
            }

            GameObject[] tankList = GameObject.FindGameObjectsWithTag("Tank");
            foreach (GameObject tank in tankList)
            {
                if (tank.GetComponent<TankGunController>() != null)
                    tank.GetComponent<TankGunController>().StopFiringWhenPlayerDies();
            }
            GetComponent<PlayerGun>().StopFiring();
            Die();
            return;
        }
        OnPlayerHit?.Invoke(gameObject, dmg);
        OnHealthChanged?.Invoke(gameObject, m_currentHealth, startingHealth);
    }

    private void Die()
    {
        OnDied_GrenadeCount?.Invoke(gameObject.GetComponent<PlayerGrenadeCounter>().GrenadeCount);
        OnDied?.Invoke(gameObject);
        OnPlayerDied?.Invoke();
    }
}
