using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankHealth : MonoBehaviour
{
    public delegate void HealthHandler(GameObject gameObject, int currentHealth, int startingHealth);
    public static event HealthHandler OnHealthChanged;
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
    private int currentHealth;
    private bool shouldSimulateHit = false;
    private void Awake()
    {
        currentHealth = startingHealth;
    }
    private void OnDestroy()
    {
        Destroy(parentObject);
    }
    public void TakeDamage(int dmg, Vector3 hitPosition)
    {
        currentHealth -= dmg;
        OnHealthChanged?.Invoke(gameObject, currentHealth, startingHealth);
        Instantiate(smokeParticleObject, hitPosition, Quaternion.identity);
        if (shouldSimulateHit == false)
            shouldSimulateHit = true;
        if (currentHealth <= 0)
        {
            GetComponent<TankGunController>().StopFiringWhenTankDies();
            Die();
            return;
        }
    }
    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        OnHealthChanged?.Invoke(gameObject, currentHealth, startingHealth);
        if (shouldSimulateHit == false)
            shouldSimulateHit = true;
        if (currentHealth <= 0)
        {
            GetComponent<TankGunController>().StopFiringWhenTankDies();
            Die();
            return;
        }
    }

    private void Die()
    {
        GameObject _explosion = Instantiate(destroyParticleObject, transform.position, Quaternion.identity);
        _explosion.transform.localScale = destroyParticleScale;
        AudioManager.Instance.PlaySoundOneShot("TankDestroy");
        GetComponent<TankGunController>().StopFiringWhenTankDies();
        Destroy(gameObject);
        
    }
}
