using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBulletController : MonoBehaviour
{
    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private int environmentLayerIndex = 8;
    [SerializeField]
    float explosionRadius = 5f;
    [SerializeField]
    float explosionForce = 70f;
    [SerializeField]
    int damage = 4;
    [SerializeField]
    float YOffsetOfTarger = 2f;
    private GameObject player;
    private GameObject explosionObjet;
    private bool bulletCollided = false;
    private bool damageDone = false;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        if (player == null)
            return;
        transform.LookAt(player.transform.position + new Vector3 (0f,YOffsetOfTarger,0f));
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (bulletCollided == true)
            return;
        bulletCollided = true;
        if(collision.gameObject.layer == environmentLayerIndex || collision.gameObject.CompareTag("Player"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        Debug.Log("Boom");
        PlayExplosionAnimation();
        Collider[] _colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        if(_colliders.Length != 0)
        {
            AddExplosionForce(_colliders);
            AddExplosionDamage(_colliders);
        }
        AudioManager.Instance.PlaySoundOneShot("Explosion");
        Destroy(gameObject);
        
    }

    private void AddExplosionForce( Collider[] _colliders)
    {
        foreach (Collider _collider in _colliders)
        {
            if (_collider.gameObject.CompareTag("Destructible"))
            {
                _collider.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }
    }

    private void AddExplosionDamage(Collider[] _colliders)
    {
        foreach (Collider _collider in _colliders)
        {
            if (_collider.gameObject.CompareTag("Player"))
            {
                if (damageDone == true)
                    return;
                damageDone = true;
                _collider.GetComponent<PlayerHealth>().TakeDamage(damage);
            }
        }
    }

    private void PlayExplosionAnimation()
    {
        explosionObjet = Instantiate(explosionPrefab, transform.position, transform.rotation);
    }
}
