using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    public delegate void EnvironmentHitManager(GameObject _gameObject, Vector3 _hitPoint, Vector3 _hitForce);
    public static event EnvironmentHitManager OnEnvironmentHit;
    [SerializeField]
    [Range(0.2f,3f)]
    private float fireRate = 2f;
    [SerializeField]
    [Range(1,10)]
    private int damage;
    [SerializeField]
    private float hitForce = 50f;
    private float timer = 0;
    [SerializeField]
    GameObject gunMuzzlePoint;
    [SerializeField]
    GameObject firePoint;
    [SerializeField]
    ParticleSystem muzzleParticles;
    private Animator animator;
    [SerializeField]
    float quaternionX, quaternionY, quaternionZ, quaternionW;
    private bool shouldFire = false;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    private void OnEnable()
    {
        AggroDetection.OnAggroFire += AggroDetection_OnAggroFire;
    }

    private void OnDisable()
    {
        AggroDetection.OnAggroFire -= AggroDetection_OnAggroFire;
    }
    private void AggroDetection_OnAggroFire(GameObject enemyObject)
    {
        if(enemyObject == gameObject)
        shouldFire = true;
    }
    public void StopFiringWhenPlayerDies()
    {
        shouldFire = false;
    }
    
    void Update()
    {
        if (shouldFire == false  || GameObject.FindGameObjectWithTag("Player") == null)
            return;
        timer += Time.deltaTime;
        if(timer >= fireRate)
        {
                timer = 0;
                FireGun();
        }
        
    }

    private void FireGun()
    {   
        PlayFireAnimation();
        ParticleSystem muzzleFlash = Instantiate(muzzleParticles, gunMuzzlePoint.transform.position, gunMuzzlePoint.transform.rotation );
        AudioManager.Instance.PlaySound("EnemyFire");
        GameObject _player = GameObject.FindGameObjectWithTag("Player");
        if (_player == null)
            return;
        Ray ray = new Ray(firePoint.transform.position, _player.transform.position - transform.position);
        Debug.DrawRay(ray.origin, ray.direction * 50, Color.red, 2f);
        RaycastHit hitInfo;
        if (Physics.Raycast(firePoint.transform.position, ray.direction, out hitInfo, 100, LayerMask.GetMask("Player", "Environment"))) 
        {
            var hitCollider = hitInfo.collider;
            var playerHealth = hitCollider.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null )
            {
                playerHealth.TakeDamage(damage);
            }
            else if(hitCollider.gameObject.GetComponent<Destructible>()!=null)
            {
                GameObject _hitObject = hitCollider.gameObject;

                if (_hitObject.GetComponent<Destructible>() != null)
                {
                    OnEnvironmentHit?.Invoke(_hitObject, hitInfo.point, hitForce * ray.direction);
                    Debug.Log("Destructible is Hit : " + _hitObject);
                }
            }
        }

    }
    void PlayFireAnimation()
    {
        animator.Play("Fire");

    }
    

}
