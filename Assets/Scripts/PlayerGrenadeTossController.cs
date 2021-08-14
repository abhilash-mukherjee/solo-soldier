using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrenadeTossController : MonoBehaviour
{
    [SerializeField]
    [Range(5f, 10f)]
    private float grenadeTossRate = 6f;
    [SerializeField]
    [Range(1, 10)]
    private int damage = 1;
    private float timer = 0;
    [SerializeField]
    private float maxGrenadeTossDistance = 100f;
    [SerializeField]
    private int layerToAvoid = 9;
    [SerializeField]
    private GameObject grenadeTossPoint;
    [SerializeField]
    private GameObject grenadePrefab;
    private Animator animator;
    private bool shouldTossGrenade = true;
    private GameObject m_Grenade;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    void Update()
    {
        if (shouldTossGrenade == false)
            return;
        timer += Time.deltaTime;
        if (timer >= grenadeTossRate)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                timer = 0;
                TossGrenade();
            }
        }

    }
    public void StopGrenadeToss()
    {
        shouldTossGrenade = false;
    }

    public void StartGrenadeToss()
    {
        shouldTossGrenade = true;
    }
    private void TossGrenade()
    {
        PlayGrenadeTossAnimation();
         m_Grenade = Instantiate(grenadePrefab, grenadeTossPoint.transform.position, grenadeTossPoint.transform.rotation);
        m_Grenade.transform.parent = grenadeTossPoint.transform;
        //AudioManager.Instance.PlaySound("Fire");
        //Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
        //Debug.DrawRay(ray.origin, ray.direction * 50, Color.red, 2f);
        //int layerMask = 1 << layerToAvoid;
        //layerMask = ~layerMask;
        //RaycastHit[] hits = Physics.RaycastAll(ray, maxFireDistance, layerMask); ;
        //foreach (RaycastHit hit in hits)
        //{
        //    Debug.Log(hit.collider.gameObject);
        //    if (hit.collider.gameObject.CompareTag("EnemySoldier"))
        //    {
        //        hit.collider.gameObject.transform.parent.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);

        //    }
        //    if (hit.collider.gameObject.CompareTag("GunShip"))
        //    {
        //        hit.collider.gameObject.transform.parent.gameObject.GetComponent<GunShipHealth>().TakeDamage(damage, hit.point);
        //    }
        //    if (hit.collider.gameObject.CompareTag("Tank"))
        //    {
        //        hit.collider.gameObject.GetComponent<TankHealth>().TakeDamage(damage, hit.point);
        //    }
        //}

    }
    void PlayGrenadeTossAnimation()
    {
        if(!(animator.GetCurrentAnimatorStateInfo(0).IsName("Die") || animator.GetCurrentAnimatorStateInfo(0).IsName("Hit")))
        {
            Debug.Log(animator.GetCurrentAnimatorStateInfo(0).fullPathHash);
            animator.enabled = true;
            DisableShootingAndMotion();
            animator.SetBool("TossGrenade", true);
        }
    }

    public void DisableShootingAndMotion()
    {
        GetComponent<PlayerGun>().StopFiring();
        GetComponent<PlayerMovement>().StopPlayerMovement();
    }
    public void ReleaseGrenade()
    {
        Debug.Log("GrenadeReleased");
        Rigidbody rb = m_Grenade.AddComponent<Rigidbody>();
        Vector3 dir = new Vector3(0, 1, 1);
        rb.AddForce(transform.TransformPoint(dir) * 20f, ForceMode.Impulse);
    }
}
