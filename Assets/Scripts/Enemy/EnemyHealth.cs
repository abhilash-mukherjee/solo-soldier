using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public delegate void HitHandler(GameObject gameObject);
    public delegate void DeathHandler(GameObject gameObject);
    public delegate void AggroHandler(GameObject gameObject);
    public static event HitHandler OnHit;
    public static event DeathHandler OnDied;
    public static event AggroHandler OnAggroHit;
    public delegate void HealthHandler(GameObject gameObject, int currentHealth, int startingHealth);
    public static event HealthHandler OnHealthChanged;
    public delegate void EnemyDeathHandler();
    public static event EnemyDeathHandler OnEnemyDied;
    [SerializeField]
    private int startingHealth = 5;
    private int m_currentHealth;

    private void Awake()
    {
        m_currentHealth = startingHealth;
    }

    public void TakeDamage( int _dmg)
    {
        m_currentHealth -= _dmg;
        OnHealthChanged?.Invoke(gameObject, m_currentHealth, startingHealth);
        OnAggroHit?.Invoke(gameObject);
        if (m_currentHealth <= 0)
        {
            GetComponent<EnemyGun>().StopFiringWhenPlayerDies();
            Die();
            return;
        }
        OnHit?.Invoke(gameObject);
    }

    private void Die()
    {
        OnDied?.Invoke(gameObject);
        OnEnemyDied?.Invoke();
    }
}
