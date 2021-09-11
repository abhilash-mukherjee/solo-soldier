using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrenadeTimerManager : MonoBehaviour
{
    [SerializeField]
    private Image fill;
    float m_time = 0;
    float m_maxTime;
    private void Start()
    {
        m_maxTime = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerGrenadeTossController>().GrenadeTossRate;
    }
    private void OnEnable()
    {
        PlayerGrenadeTossController.OnGrenadeTossed += ReloadGrenadeTimer;
    }
    private void OnDisable()
    {
        PlayerGrenadeTossController.OnGrenadeTossed -= ReloadGrenadeTimer;
    }
    private void ReloadGrenadeTimer()
    {
        m_time = 0f;
        fill.fillAmount = 0f;
    }
    private void Update()
    {
        UpdateGrenadeTimer();
        CheckGrenadeCount();
    }
    private void UpdateGrenadeTimer()
    {
        if (m_time >= m_maxTime)
            return;
        m_time += Time.deltaTime;
        fill.fillAmount = m_time / m_maxTime;
    }

    private void CheckGrenadeCount()
    {
        if(GameObject.FindGameObjectWithTag("Player") != null)
        {
            if(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerGrenadeCounter>().GrenadeCount == 0)
            {
                m_time = 0f;
                fill.fillAmount = 0f;
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
