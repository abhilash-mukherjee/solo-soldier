using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeExplosionManager : MonoBehaviour
{
    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private float grenadeExplosionTime = 2f;
    [SerializeField]
    float explosionForce = 70f;
    [SerializeField]
    float explosionRadius = 5f;
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
            if(_collider.gameObject.CompareTag("Enemy"))
            {
                _collider.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
            }
            else if(_collider.gameObject.CompareTag("Tank"))
            {
                _collider.gameObject.GetComponent<TankHealth>().TakeDamage(damage);
            }
            else if(_collider.gameObject.CompareTag("GunShip"))
            {
                _collider.gameObject.transform.parent.gameObject.GetComponent<GunShipHealth>().TakeDamage(damage);
            }
        }
    }
}
