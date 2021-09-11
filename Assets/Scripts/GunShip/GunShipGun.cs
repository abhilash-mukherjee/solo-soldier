using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShipGun : MonoBehaviour
{
    public delegate void EnvironmentHitManager(GameObject _gameObject, Vector3 _hitPoint, Vector3 _hitForce);
    public static event EnvironmentHitManager OnEnvironmentHit;
    [SerializeField]
    [Range(0.2f,10f)]
    private float fireRate = 2f;
    [SerializeField]
    [Range(1,10)]
    private int damage = 2;
    [SerializeField]
    private float hitForce = 50f;
    [SerializeField]
    private float maxFireDistance = 200f;
    [SerializeField]
    GameObject gunMuzzlePoint;
    [SerializeField]
    GameObject firePoint;
    [SerializeField]
    ParticleSystem muzzleParticles;
    [SerializeField]
    private float turnSpeed = 400f;
    [SerializeField]
    private float timeDelayBetweenFireAnimationStartAndSoundPlay = 0.5f;
    [SerializeField]
    private float timeDelayBetweenSoundPlayAndGunHit = 0.5f;
    private Animator animator;
    private bool shouldFire = true;
    private float timer = 0;
    private GameObject player;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void StopFiringWhenPlayerDies()
    {
        shouldFire = false;
    }
    public void StopFiringWhenGunShipDies()
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
        Vector3 direction = player.transform.position - gameObject.transform.position;
        Quaternion newDirection = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, newDirection, Time.deltaTime * turnSpeed);
       
    }
    private void FireGun()
    {   
        PlayFireAnimation();
    }
    void PlayFireAnimation()
    {
        animator.Play("GunshipFire");
        StartCoroutine(PlaySoundAndShootParticles(timeDelayBetweenFireAnimationStartAndSoundPlay));

    }
    
    IEnumerator PlaySoundAndShootParticles(float time)
    {
        yield return new WaitForSeconds(time);
        ParticleSystem muzzleFlash = Instantiate(muzzleParticles, gunMuzzlePoint.transform.position, gunMuzzlePoint.transform.rotation);
        AudioManager.Instance.PlaySound("GunShipFire");
        StartCoroutine(FireAfterPause(timeDelayBetweenSoundPlayAndGunHit));
    }
    IEnumerator FireAfterPause(float time)
    {
        yield return new WaitForSeconds(time);
        Ray ray = new Ray(firePoint.transform.position, gameObject.transform.forward * maxFireDistance);
        Debug.DrawRay(ray.origin, ray.direction * 50, Color.red, 2f);
        RaycastHit[] m_hitInfo;
        m_hitInfo = Physics.RaycastAll(ray,maxFireDistance, LayerMask.GetMask("Player"));
        if (m_hitInfo.Length == 0)
        {
            Debug.Log("Hit Nothing");
            yield return null;
        }
        else if (m_hitInfo[0].collider.gameObject.CompareTag("Player"))
        {
            var hitCollider = m_hitInfo[0].collider;
            var playerHealth = hitCollider.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
        else if (m_hitInfo[0].collider.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
            GameObject _hitObject = m_hitInfo[0].collider.gameObject;
            
            if(_hitObject.GetComponent<Destructible>() != null)
            {
                OnEnvironmentHit?.Invoke(_hitObject, m_hitInfo[0].point, hitForce * ray.direction);
                Debug.Log("Destructible is Hit : " + _hitObject);
            }
        }
    }

}
