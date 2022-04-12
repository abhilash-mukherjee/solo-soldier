using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrenadeTossController : MonoBehaviour
{
    public delegate void GrenadeTimerHandler();
    public static event GrenadeTimerHandler OnGrenadeTossed;
    [SerializeField]
    [Range(3f, 10f)]
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
    [SerializeField]
    private float grenadeForce = 100f;
    [SerializeField]
    private float grenadeTimeInAir = 3f;
    [SerializeField]
    private Vector3 grenadeImpulsePoint = new Vector3(0f,0.15f,0f);
    public float GrenadeTossRate
    {
        get { return grenadeTossRate; }
    }
    private Animator animator;
    private bool m_shouldTossGrenade = true;
    private bool m_shouldReleaseGrenade = false;
    private GameObject m_Grenade;
    public GameObject Grenade
    {
        get { return m_Grenade; }
    }
    private RaycastHit[] m_hits;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    void Update()
    {
        if (m_shouldTossGrenade == false)
            return;
        timer += Time.deltaTime;
        if (timer >= grenadeTossRate)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                if(GetComponent<PlayerGrenadeCounter>().GrenadeCount > 0)
                {
                    TossGrenade();
                }
            }
        }
        if(m_shouldReleaseGrenade == false && m_Grenade != null)
        {
            m_Grenade.transform.position = grenadeTossPoint.transform.position;
        }

    }
    private void TossGrenade()
    {
        m_hits = HitRaycastAndReturnHitResults();
        if(m_Grenade != null && m_Grenade.GetComponent<Rigidbody>() == null)
        {
            //Suppose the grenade is instantiated but player is hit before releasing the grenade.
            //Then the grenade will keep floating in free space forever
            //To avoid this we check if the previous grenade had rigidbody attached to it
            //If yes, then it's a normal explosive grenade, otherwise it's a dummy grenade with no use
            //So, destroy the grenade
            Destroy(m_Grenade);
        }
        m_Grenade = Instantiate(grenadePrefab, grenadeTossPoint.transform.position, grenadeTossPoint.transform.rotation);
        PlayGrenadeTossAnimation();
        PlayGrenadeTossSound();
    }

    private void PlayGrenadeTossSound()
    {
        AudioManager.Instance.PlaySoundOneShot("GrenadeToss");
    }

    void PlayGrenadeTossAnimation()
    {
        if(!(animator.GetCurrentAnimatorStateInfo(0).IsName("Die") || animator.GetCurrentAnimatorStateInfo(0).IsName("Hit")))
        {
            animator.enabled = true;
            DisableShootingAndMotion();
            animator.SetBool("TossGrenade", true);
        }
    }
    public void StopGrenadeToss()
    {
        m_shouldTossGrenade = false;
    }

    public void StartGrenadeToss()
    {
        m_shouldTossGrenade = true;
    }
    public void ReleaseGrenade()
    {
        m_shouldReleaseGrenade = true;
        if(m_Grenade != null)
        {
            m_Grenade.AddComponent<Rigidbody>();
        }
        if(m_hits.Length == 0)
        {
            Ray _ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
            ApplyForce(grenadeTossPoint.transform.position + 100 * _ray.direction);
            m_Grenade = null;
            m_shouldReleaseGrenade = false;
            timer = 0;
        }
        else
        {
            Vector3 _grenadeTarget = m_hits[0].point;
            ApplyForce(_grenadeTarget);
            m_Grenade = null;
            m_shouldReleaseGrenade = false;
            timer = 0;
        }
        OnGrenadeTossed?.Invoke();
    }

    public RaycastHit[] HitRaycastAndReturnHitResults()
    {
        Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
        Debug.DrawRay(ray.origin, ray.direction * 50, Color.red, 2f);
        return Physics.RaycastAll(ray, maxGrenadeTossDistance, LayerMask.GetMask("Environment", "MainGun"));
    }
    public void ApplyForce(Vector3 _target)
    {
        Vector3 _dir = _target- m_Grenade.transform.position;
        Vector3 _dirXZ = _dir;
        _dirXZ.y = 0;
        _dirXZ.Normalize();
        Vector3 _xzForce = CalculateHorizontalForce(_dir) * _dirXZ;  
        Vector3 _yForce = CalculateVerticalForce(_dir) * Vector3.up;
        m_Grenade.GetComponent<Rigidbody>().AddForceAtPosition(_xzForce +_yForce, grenadeImpulsePoint, ForceMode.Impulse );
    }
    public float CalculateVerticalForce(Vector3 _directionVector)
    {
        float _time = grenadeTimeInAir;
        Vector3 _directionVectorY = _directionVector;
        float _distY = _directionVectorY.y;
        float _mass = m_Grenade.GetComponent<Rigidbody>().mass;
        float _gravity = Physics.gravity.magnitude;
        return( (_mass * _distY / _time) + (_gravity * _mass * _time / 2.0f));

    }
    
    public float CalculateHorizontalForce(Vector3 _directionVector)
    {
        Vector3 _directionVectorXZ = _directionVector;
        _directionVectorXZ.y = 0;
        float _distXZ = _directionVectorXZ.magnitude;
        float _time = grenadeTimeInAir;
        float _mass = m_Grenade.GetComponent<Rigidbody>().mass;
        return (_mass * _distXZ / _time);
    }
    public void DisableShootingAndMotion()
    {
        GetComponent<PlayerGun>().StopFiring();
        GetComponent<PlayerMovement>().StopPlayerMovement();
    }
}
