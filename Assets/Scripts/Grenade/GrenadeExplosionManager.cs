using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeExplosionManager : MonoBehaviour
{
    public delegate void EnvironmentHitManager(GameObject _gameObject);
    public static event EnvironmentHitManager OnEnvironmentHit;
    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private float grenadeExplosionTime = 2f;
    [SerializeField]
    float explosionForce = 1000f;
    [SerializeField]
    float explosionRadius = 5f;
    [SerializeField]
    float environmentExplosionRadius = 5f;
    [SerializeField]
    int damage = 4;
    private GameObject m_explosionObjet;
    private bool m_damageDone = false;
    private bool m_hasCollided = false;
    private void OnCollisionEnter(Collision collision)
    {
        if (m_hasCollided == true)
             return;
        if(collision.gameObject.CompareTag("Enemy") 
            || collision.gameObject.CompareTag("Tank") 
            || collision.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
            m_hasCollided = true;
            StartCoroutine(Explode(grenadeExplosionTime));
        }  
        else if(collision.gameObject.CompareTag("GunShip"))
        {
            m_hasCollided = true;
            StartCoroutine(Explode(0f));
        }
    }
    
    IEnumerator Explode(float _time)
    {
        yield return new WaitForSeconds(_time);
        m_explosionObjet = Instantiate(explosionPrefab, transform.position, transform.rotation);
        AudioManager.Instance.PlaySoundOneShot("GrenadeExplosion");
        DoDamage(damage);
        Destroy(gameObject);
    }
    private void DoDamage(float _damage)
    {
        Collider[] _colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach(Collider _collider in _colliders)
        {
            if(_collider.gameObject.CompareTag("EnemySoldier"))
            {
                _collider.gameObject.transform.parent.GetComponent<EnemyHealth>().TakeDamage(damage);
            }
            else if(_collider.gameObject.CompareTag("Tank"))
            {
                _collider.gameObject.GetComponent<TankHealth>().TakeDamage(damage);
            }
            else if(_collider.gameObject.CompareTag("GunShip"))
            {
                _collider.gameObject.transform.parent.gameObject.GetComponent<GunShipHealth>().TakeDamage(damage);
            }
            else if(_collider.gameObject.GetComponent<Destructible>() != null)
            {
                AddExplosionForce(_collider.gameObject);
                OnEnvironmentHit?.Invoke(_collider.gameObject);
            }
        }
    }

    private void AddExplosionForce(GameObject _gameObject)
    {
        if(_gameObject.GetComponent<Rigidbody>()!= null)
        {
            _gameObject.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position,environmentExplosionRadius);
        }
    }
}
