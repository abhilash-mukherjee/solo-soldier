using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrenadeCounter : MonoBehaviour
{
    public delegate void GrenadeTimerHandler(int _grenadeCount);
    public static event GrenadeTimerHandler OnGrenadeCountChanged;
    [SerializeField]
    private int grenadeCount = 0;
    public int GrenadeCount
    {
        get { return grenadeCount; }
    }
    private void OnEnable()
    {
        PlayerGrenadePickManager.OnGrenadePicked += UpdateGrenadeCount;
        PlayerGrenadeTossController.OnGrenadeTossed += ReduceGrenadeCount;
    }
    private void OnDisable()
    {
        PlayerGrenadePickManager.OnGrenadePicked -= UpdateGrenadeCount;
        PlayerGrenadeTossController.OnGrenadeTossed -= ReduceGrenadeCount;

    }

    private void Start()
    {
        grenadeCount = GameManager.Instance.currentGrenadeCout;
        OnGrenadeCountChanged?.Invoke(grenadeCount);
    }
    private void UpdateGrenadeCount()
    {
        grenadeCount++;
        Debug.Log("GrenadeCount = " + grenadeCount);
        OnGrenadeCountChanged?.Invoke(grenadeCount);
    }
    public void ReduceGrenadeCount()
    {
        grenadeCount--;
        OnGrenadeCountChanged?.Invoke(grenadeCount);
    }

    
}
