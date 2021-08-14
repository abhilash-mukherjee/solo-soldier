using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public delegate void PlayerDeathHandler();
    public static event PlayerDeathHandler OnPlayerDieAnimationFinishedPlaying;
    public delegate void PlayerHitHandler();
    public static event PlayerHitHandler OnPlayerHitAnimationFinishedPlaying;
    private bool playerDeathStarted = false;
    private bool playerDeathEnd = false;
    [SerializeField]
    private GameObject playerParentObject;
    [SerializeField]
    private float yVelocityDuringDeath;
    [SerializeField]
    private float timeToMovePlayerDownwards;

    public void PlayerDied()
    {
        OnPlayerDieAnimationFinishedPlaying?.Invoke();
    }
    public void PlayerHit()
    {
        OnPlayerHitAnimationFinishedPlaying?.Invoke();
    }
    public void DeathStarted()
    {
        StartCoroutine(OffsetPlayerWhileDying(timeToMovePlayerDownwards));
    }
    public void StartFiring()
    {
        transform.parent.GetComponent<PlayerGun>().StartFiring();
    }

    IEnumerator OffsetPlayerWhileDying(float time)
    {
        yield return new WaitForSeconds(time);
        playerDeathStarted = true;
    }

    public void DeathEnd()
    {
        playerDeathEnd = true;
        playerParentObject.SetActive(false);
        
    }
    private void Update()
    {
        if (playerDeathStarted && !playerDeathEnd)
        {
            gameObject.transform.Translate(Time.deltaTime * yVelocityDuringDeath * new Vector3(0f, -1f, 0f));
        }
    }
}

