using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public delegate void HitHandler(GameObject gameObject);
    public delegate void DeathHandler(GameObject gameObject);
    public static event HitHandler OnHit;
    public static event DeathHandler OnDied;
    [SerializeField]
    private int startingHealth = 5;
    private int currentHealth;

    private void Awake()
    {
        currentHealth = startingHealth;
    }

    public void TakeDamage( int dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
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
    }
}
