using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField]
    [Range(0.2f, 1.5f)]
    private float fireRate = 0.3f;
    [SerializeField]
    [Range(1f, 10f)]
    private float damage;
    private float timer = 0;
    [SerializeField]
    GameObject gunMuzzlePoint;
    [SerializeField]
    ParticleSystem muzzleParticles;
    [SerializeField]
    private float maxFireDistance = 100f;
    private Animator animator;
    private bool shouldFire = true;
    
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
    public void StopFiringWhenPlayerDies()
    {
        shouldFire = false;
    }

    private void FireGun()
    {
        if (shouldFire == false)
            return;
        PlayFireAnimation();
        ParticleSystem muzzleFlash = Instantiate(muzzleParticles, gunMuzzlePoint.transform.position, gunMuzzlePoint.transform.rotation);
        AudioManager.Instance.PlaySound("Fire");
        Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
        Debug.DrawRay(ray.origin, ray.direction * 50, Color.red, 2f);
        /*RaycastHit hitInfo;
        if (Physics.Raycast(ray,out hitInfo, 100))
        {
            Debug.Log("Fired");
            var hitCollider = hitInfo.collider;
            if(hitCollider.gameObject.CompareTag("EnemySoldier"))
            {
                hitCollider.gameObject.transform.parent.gameObject.GetComponent<EnemyHealth>().TakeDamage(1);
            }
        }*/

        // new code
        RaycastHit[] hits = Physics.RaycastAll(ray,maxFireDistance); ;
        foreach(RaycastHit hit in hits)
        {
            if(hit.collider.gameObject.CompareTag("EnemySoldier"))
            {
                hit.collider.gameObject.transform.parent.gameObject.GetComponent<EnemyHealth>().TakeDamage(1);

            }
        }
        //end of new code

    }
    void PlayFireAnimation()
    {
        animator.Play("Fire");

    }
    

}
