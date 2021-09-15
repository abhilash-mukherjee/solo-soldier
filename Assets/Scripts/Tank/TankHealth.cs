using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankHealth : MonoBehaviour
{
    public delegate void HealthHandler(GameObject gameObject, int currentHealth, int startingHealth);
    public static event HealthHandler OnHealthChanged;
    public delegate void TankDeathHandler(GameObject _gameObject);
    public static event TankDeathHandler OnDied;
    public delegate void DeathHandler();
    public static event DeathHandler OnTankDied;
    [SerializeField]
    private GameObject parentObject;
    [SerializeField]
    private GameObject smokeParticleObject;
    [SerializeField]
    private GameObject destroyParticleObject;
    [SerializeField]
    private Vector3 destroyParticleScale = new Vector3(2f,2f,2f);
    [SerializeField]
    private int startingHealth = 30;
    private int m_currentHealth;
    public int CurrentHealth
    {
        get { return m_currentHealth; }
        set { m_currentHealth = value;
            OnHealthChanged?.Invoke(gameObject, m_currentHealth, startingHealth);
        }
    }
    private bool shouldSimulateHit = false;
    private void Awake()
    {
        m_currentHealth = startingHealth;
    }
    private void OnDestroy()
    {
        Destroy(parentObject);
    }
    public void TakeDamage(int dmg, Vector3 hitPosition)
    {
        m_currentHealth -= dmg;
        OnHealthChanged?.Invoke(gameObject, m_currentHealth, startingHealth);
        Instantiate(smokeParticleObject, hitPosition, Quaternion.identity);
        if (shouldSimulateHit == false)
            shouldSimulateHit = true;
        if (m_currentHealth <= 0)
        {
            GetComponent<TankGunController>().StopFiringWhenTankDies();
            Die();
            return;
        }
        Debug.Log("Tank Health: " + m_currentHealth);
    }
    public void TakeDamage(int dmg)
    {
        m_currentHealth -= dmg;
        OnHealthChanged?.Invoke(gameObject, m_currentHealth, startingHealth);
        if (shouldSimulateHit == false)
            shouldSimulateHit = true;
        if (m_currentHealth <= 0)
        {
            GetComponent<TankGunController>().StopFiringWhenTankDies();
            Die();
            return;
        }
        Debug.Log("Tank Health: " + m_currentHealth);
    }

    private void Die()
    {
        GameObject _explosion = Instantiate(destroyParticleObject, transform.position, Quaternion.identity);
        _explosion.transform.localScale = destroyParticleScale;
        AudioManager.Instance.PlaySoundOneShot("TankDestroy");
        GetComponent<TankGunController>().StopFiringWhenTankDies();
        Destroy(gameObject);
        OnTankDied?.Invoke();
        OnDied?.Invoke(gameObject);    
    }
}
