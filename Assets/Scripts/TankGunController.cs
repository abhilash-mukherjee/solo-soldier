using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankGunController : MonoBehaviour
{
    [SerializeField]
    [Range(0.2f, 10f)]
    private float fireRate = 2f;
    [SerializeField]
    [Range(1, 10)]
    private int damage;
    [SerializeField]
    GameObject mainGun;
    [SerializeField]
    GameObject tankBulletPrefab;
    [SerializeField]
    private float bulletForceMultiplierXZ = 20f, bulletForceMultiplierY = 2f;
    [SerializeField]
    GameObject gunMuzzlePoint;
    [SerializeField]
    GameObject firePoint;
    [SerializeField]
    ParticleSystem muzzleParticles;
    [SerializeField]
    private float timeDelayBetweenFireAnimationStartAndSoundPlay = 0.5f;
    [SerializeField]
    private float timeDelayBetweenSoundPlayAndShellHit = 0.5f;
    [SerializeField]
    private Animator animator;
    private bool shouldFire = true;
    private float timer = 0;
    private GameObject player;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (shouldFire == false)
            return;
        timer += Time.deltaTime;
        if (timer >= fireRate)
        {
            timer = 0;
            FireGun();
        }
    }
    private void FireGun()
    {
        PlayFireAnimation();
        StartCoroutine(PlaySoundAndShootParticles(timeDelayBetweenFireAnimationStartAndSoundPlay));
    }
    IEnumerator PlaySoundAndShootParticles(float time)
    {
        yield return new WaitForSeconds(time);
        ParticleSystem muzzleFlash = Instantiate(muzzleParticles, gunMuzzlePoint.transform.position, gunMuzzlePoint.transform.rotation);
        AudioManager.Instance.PlaySound("GunShipFire");
        ShootBullet();
        StartCoroutine(FireAfterPause(timeDelayBetweenSoundPlayAndShellHit));
    }

    private void ShootBullet()
    {
        Vector3 direction = player.transform.position - firePoint.transform.position;
        direction.y = 0f;
        direction.Normalize();
        GameObject instantiatedBullet = Instantiate(tankBulletPrefab, firePoint.transform.position, Quaternion.identity);
        instantiatedBullet.GetComponent<Rigidbody>().AddForce(direction * bulletForceMultiplierXZ, ForceMode.Impulse);
        instantiatedBullet.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(Vector3.up)* bulletForceMultiplierY, ForceMode.Impulse);
    }

    void PlayFireAnimation()
    {
        animator.SetBool("Fire",true);
    }

    IEnumerator FireAfterPause(float time)
    {
        yield return new WaitForSeconds(time);
        //Ray ray = new Ray(firePoint.transform.position, gameObject.transform.forward);
        //Debug.DrawRay(ray.origin, ray.direction * 50, Color.red, 2f);
        //RaycastHit hitInfo;
        //if (Physics.Raycast(ray, out hitInfo, 100))
        //{
        //    var hitCollider = hitInfo.collider;
        //    var playerHealth = hitCollider.gameObject.GetComponent<PlayerHealth>();
        //    if (playerHealth != null)
        //    {
        //        playerHealth.TakeDamage(damage);
        //    }
        //}
    }
    public void StopFiringWhenPlayerDies()
    {
        shouldFire = false;
    }
    public void StopFiringWhenTankDies()
    {
        shouldFire = false;
    }
}
