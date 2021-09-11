using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GrenadeCounterDisplayManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI grenadeCount;
    private int m_grenadeCount = 0;
    private void Start()
    {
        m_grenadeCount = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerGrenadeCounter>().GrenadeCount;
        SetText();
    }
    private void OnEnable()
    {
        PlayerGrenadeCounter.OnGrenadeCountChanged += UpdateGrenadeCount;
        PlayerHealth.OnDied += DeactivateCounter;
    }


    private void OnDisable()
    {
        PlayerGrenadeCounter.OnGrenadeCountChanged -= UpdateGrenadeCount;
        PlayerHealth.OnDied -= DeactivateCounter;        
    }
    private void UpdateGrenadeCount(int _count)
    {
        m_grenadeCount = _count;
        SetText();
    }

    private void SetText()
    {
        if (m_grenadeCount >= 10)
        {
            grenadeCount.text = m_grenadeCount.ToString();
        }
        else
        {
            grenadeCount.text = "0" + m_grenadeCount.ToString();
        }
    }

    private void DeactivateCounter(GameObject _playerObject)
    {
        gameObject.SetActive(false);
    }

}
