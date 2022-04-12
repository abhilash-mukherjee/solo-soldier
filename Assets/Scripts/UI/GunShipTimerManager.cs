using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GunShipTimerManager : MonoBehaviour
{
    [SerializeField]
    private Image fill;
    [SerializeField]
    private TextMeshProUGUI messege;
    [SerializeField][TextArea]
    private string messege1, messege2, messege3;
    [SerializeField]
    private Sprite[] checkPointSprites;
    [SerializeField]
    private float timerCheckPoint1, timerCheckPoint2, timerCheckPoint3;
    float m_time = 0;
    float m_maxTime;
    private void OnEnable()
    {
        if (GunShipTimer.gunShipTimers.Count != 0)
        {
            m_time = GunShipTimer.gunShipTimers[0].TimerTime;
        }
        CheckForGunShipSpawner();
        fill.fillAmount = 1f;
        messege.text = messege1;
        fill.sprite = checkPointSprites[0];
        var gunShiSpawner = GameObject.FindGameObjectWithTag("GunShipSpawner");
        if(gunShiSpawner != null)
        {
            m_maxTime = gunShiSpawner.GetComponent<GunShipTimer>().GunShipReturnPeriod;
        }
        UpdateGunShipTimer();
    }

    private void CheckForGunShipSpawner()
    {
        if (GameObject.FindGameObjectWithTag("GunShipSpawner") == null || GameObject.FindGameObjectWithTag("GunShip") != null)
            gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        m_time = 0;
        fill.fillAmount = 1f;
        messege.text = messege1;
        fill.sprite = checkPointSprites[0];
    }

    private void Update()
    {
        UpdateGunShipTimer();
    }
    private void UpdateGunShipTimer()
    {
        if(GunShipTimer.gunShipTimers.Count != 0)
        {
            m_time = GunShipTimer.gunShipTimers[0].TimerTime;
        }
        if(KeySpawner.isKeySpawned == true)
        {
            gameObject.SetActive(false);
            return;
        }
        if (m_time >= m_maxTime)
        {
            m_time = 0;
            return;
        }        
        fill.fillAmount = (m_maxTime - m_time )/ m_maxTime;
        UpdateColorAndMessege();
    }


    private void UpdateColorAndMessege()
    {
        if (fill.fillAmount >= timerCheckPoint1)
        {
            fill.sprite = checkPointSprites[0];
            messege.text = messege1;

        }
        else if (fill.fillAmount >= timerCheckPoint2)
        {
            fill.sprite = checkPointSprites[1];
            messege.text = messege2;
        }
        else if (fill.fillAmount >= timerCheckPoint3)
        {
            fill.sprite = checkPointSprites[2];
            messege.text = messege3;
        }
    }
}
