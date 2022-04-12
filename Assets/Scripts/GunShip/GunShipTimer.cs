using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShipTimer : MonoBehaviour, ISavable
{
    public delegate void TimerHandler();
    public static event TimerHandler OnGunShipTimerEnd;
    public static List<GunShipTimer> gunShipTimers= new List<GunShipTimer>();
    [SerializeField]
    private float pauseTime = 2f;
    [SerializeField]
    private int childIndexForGunshipTimer = 6;
    [SerializeField]
    private float gunShipReturnPeriod = 180f;
    public float GunShipReturnPeriod
    {
        get { return gunShipReturnPeriod; }
    }
    private bool m_runTimer = false;
    private float m_time = 0;
    public float TimerTime 
    {
        get { return m_time; }
    }
    private void Update()
    {
        Timer();
    }
    
    private void Timer()
    {
        if (m_runTimer == false)
            return;
        else
        {
            if (m_time >= gunShipReturnPeriod)
            {
                m_runTimer = false;
                m_time = 0;
                OnGunShipTimerEnd?.Invoke();
                return;
            }
            else if (KeySpawner.isKeySpawned == true)
            {
                m_runTimer = false;
                m_time = 0;
                return;
            }
            else
            {
                m_time += Time.deltaTime;
            }
        }
    }

    private void OnEnable()
    {
        gunShipTimers.Add(this);
        Debug.Log( "The list of gunship timers has length =  " + gunShipTimers.Count);
        GunShipDeathManager.OnGunShipDied += StartGunshipTimer;
    }

    private void OnDisable()
    {
        gunShipTimers.Remove(this);
        GunShipDeathManager.OnGunShipDied -= StartGunshipTimer;

    }

    private void StartGunshipTimer()
    {
        Debug.Log("Inside Start GunShip Timer");
        StartCoroutine(StartTimerAfterPause(pauseTime));
    }

    IEnumerator StartTimerAfterPause(float _time)
    {
        yield return new WaitForSeconds(_time);
        Debug.Log("Timer Started");
        StartTimer(0f);
    }

    private void StartTimer(float _time)
    {
        if (m_runTimer == false)
        {
            Debug.Log("Inside Start Timer Function");
            m_runTimer = true;
            m_time = _time;
            if (GameCanvasController.CanvasList.Count != 0)
            {
                var timer = GameCanvasController.CanvasList[0].transform.GetChild(childIndexForGunshipTimer).GetComponent<GunShipTimerManager>();
                if (timer != null)
                {
                    Debug.Log("Timer Displayed On Canvas");
                    timer.gameObject.SetActive(true);
                }
            }
        }
    }

    private int CheckGunShipCountInCurrentScene()
    {
        var _gunShips = GameObject.FindGameObjectsWithTag("GunShip");
        if (_gunShips == null || _gunShips.Length == 0)
        {
            return 0;
        }
        else
        {
            return _gunShips.Length;
        }
    }

    public void PopulateSaveData(SaveData sd)
    {
        sd.m_gunShipTimer = m_time;
        Debug.Log("Timer data saved. Time = " + m_time);
    }

    public void LoadFromSaveData(SaveData sd)
    {
        Debug.Log("Inside LoadSaveData of GunShipTimer. Gunship count in this scene = " + CheckGunShipCountInCurrentScene());
        if(CheckGunShipCountInCurrentScene() == 0) 
        {
            StartTimer(sd.m_gunShipTimer);
        } 
    }
}
