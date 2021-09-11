using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField]
    private float startingHealth = 10f;
    [SerializeField]
    private GameObject smokeParticle;
    [SerializeField]
    private GameObject explodeParticle;
    private float m_currentHealth;
    private void Awake()
    {
        m_currentHealth = startingHealth;
    }
    private void OnEnable()
    {
        GunShipGun.OnEnvironmentHit += Damage;
        EnemyGun.OnEnvironmentHit += Damage;
        PlayerGun.OnEnvironmentHit += Damage;
        GrenadeExplosionManager.OnEnvironmentHit += Damage;
    }
    private void OnDisable()
    {
        GunShipGun.OnEnvironmentHit -= Damage;
        EnemyGun.OnEnvironmentHit -= Damage;
        PlayerGun.OnEnvironmentHit -= Damage;
        GrenadeExplosionManager.OnEnvironmentHit -= Damage;
        
    }

    private void Damage(GameObject _gameObject, Vector3 _hitPoint, Vector3 _hitForce)
    {
        if(_gameObject == gameObject)
        {
            m_currentHealth--;
            if(m_currentHealth <= 0)
            {
                DestroyDestructible();
            }
            else
            {
                if(gameObject.GetComponent<Rigidbody>() != null)
                {
                    gameObject.GetComponent<Rigidbody>().AddForceAtPosition(_hitForce,_hitPoint);
                }
                Instantiate(smokeParticle, _hitPoint, transform.rotation);
            }
        }
    }
    private void Damage(GameObject _gameObject)
    {
        if(_gameObject == gameObject)
        {
            m_currentHealth--;
            if(m_currentHealth <= 0)
            {
                DestroyDestructible();
            }
            else
            {
                Instantiate(smokeParticle, transform.position, transform.rotation);
            }
        }
    }

    private void DestroyDestructible()
    {
        Explode();
        Destroy(gameObject);
    }

    private void Explode()
    {
        Instantiate(explodeParticle, transform.position, transform.rotation);
        AudioManager.Instance.PlaySoundOneShot("Explosion");
        Destroy(gameObject);
    }
}
