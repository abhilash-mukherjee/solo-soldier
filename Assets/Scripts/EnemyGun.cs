using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    [SerializeField]
    [Range(0.2f,3f)]
    private float fireRate = 2f;
    [SerializeField]
    [Range(1f,10f)]
    private float damage;
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
        if (shouldFire == false)
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
        Ray ray = new Ray(firePoint.transform.position, gameObject.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 50, Color.red, 2f);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray,out hitInfo, 100))
        {
            var hitCollider = hitInfo.collider;
            var playerHealth = hitCollider.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null )
            {
                    playerHealth.TakeDamage(1);
            }
        }

    }
    void PlayFireAnimation()
    {
        animator.Play("Fire");

    }
    

}
