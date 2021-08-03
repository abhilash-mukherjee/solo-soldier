using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    [Range(0.2f,1.5f)]
    private float fireRate = 0.3f;
    [SerializeField]
    [Range(1f,10f)]
    private float damage;
    private float timer = 0;
    [SerializeField]
    GameObject gunMuzzlePoint;
    [SerializeField]
    ParticleSystem muzzleParticles;
    private Animator animator;
   

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= fireRate)
        {
            if(Input.GetButtonDown("Fire1"))
            {
                timer = 0;
                FireGun();
            }
        }
        
    }

    private void FireGun()
    {
        PlayFireAnimation();
        ParticleSystem muzzleFlash = Instantiate(muzzleParticles, gunMuzzlePoint.transform.position, gunMuzzlePoint.transform.rotation);
        AudioManager.Instance.PlaySound("Fire");
        Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
        Debug.DrawRay(ray.origin, ray.direction * 50, Color.red, 2f);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray,out hitInfo, 100))
        {
            var hitCollider = hitInfo.collider;
            var health = hitCollider.gameObject.GetComponent<Health>();
            if (health != null )
            {
                if(hitCollider == hitCollider.gameObject.GetComponent<ColliderList>().ListOfColliders[0])
                {
                    health.TakeDamage(1);
                }
            }
        }

    }
    void PlayFireAnimation()
    {
        animator.Play("Fire");

    }
    

}
