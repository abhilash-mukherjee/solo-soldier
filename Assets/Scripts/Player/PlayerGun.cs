using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    public delegate void EnvironmentHitManager(GameObject _gameObject, Vector3 _hitPoint, Vector3 _hitForce);
    public static event EnvironmentHitManager OnEnvironmentHit;
    [SerializeField]
    [Range(0.5f, 1.5f)]
    private float fireRate = 0.3f;
    [SerializeField]
    [Range(1, 20)]
    private int damage =1;
    [SerializeField]
    private float hitForce = 50f;
    private float timer = 0;
    [SerializeField]
    GameObject gunMuzzlePoint;
    [SerializeField]
    ParticleSystem muzzleParticles;
    [SerializeField]
    private float maxFireDistance = 100f;
    [SerializeField]
    private int layerToAvoid = 9;
    [SerializeField]
    private GameObject dustParticles;
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
 
    public void StopFiring()
    {
        shouldFire = false;
    }
    public void StartFiring()
    {
        shouldFire = true;
    }
    private void FireGun()
    {
        if (shouldFire == false)
            return;
        DisableShootingAndMotion();
        PlayFireAnimation();
        ParticleSystem muzzleFlash = Instantiate(muzzleParticles, gunMuzzlePoint.transform.position, gunMuzzlePoint.transform.rotation);
        AudioManager.Instance.PlaySound("Fire");
        Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
        Debug.DrawRay(ray.origin, ray.direction * 50, Color.red, 2f);
        int layerMask = 1 << layerToAvoid;
        layerMask = ~layerMask;
        RaycastHit[] hits = Physics.RaycastAll(ray,maxFireDistance,layerMask); ;
        foreach(RaycastHit hit in hits)
        {
            if(hit.collider.gameObject.CompareTag("EnemySoldier"))
            {
                hit.collider.gameObject.transform.parent.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);

            }
            if (hit.collider.gameObject.CompareTag("GunShip"))
            {
                hit.collider.gameObject.transform.parent.gameObject.GetComponent<GunShipHealth>().TakeDamage(damage, hit.point);
            }
            if (hit.collider.gameObject.CompareTag("Tank"))
            {
                hit.collider.gameObject.GetComponent<TankHealth>().TakeDamage(damage, hit.point);
            }
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Environment"))
            {

                GameObject _hitObject = hit.collider.gameObject;

                if (_hitObject.GetComponent<Destructible>() != null)
                {
                    OnEnvironmentHit?.Invoke(_hitObject, hit.point, hitForce * ray.direction);
                }
                else
                {
                    Instantiate(dustParticles, hit.point, Quaternion.identity);

                }                  
            }
        }

    }
    void PlayFireAnimation()
    {
        animator.SetBool("Fire", true);

    }

    public void DisableShootingAndMotion()
    {
        GetComponent<PlayerMovement>().StopPlayerMovement();
    }

}
