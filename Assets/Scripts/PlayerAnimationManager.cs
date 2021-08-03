using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerDamage.OnPlayerDieAnimationFinishedPlaying += PlayerDamage_OnPlayerDieAnimationFinishedPlaying;
        PlayerDamage.OnPlayerHitAnimationFinishedPlaying += PlayerDamage_OnPlayerHitAnimationFinishedPlaying;
        Health.OnDied += PlayDeathAnimationOnDied;
        Health.OnHit += PlayHitAnimationOnHit;
    }
    private void OnDisable()
    {
        PlayerDamage.OnPlayerDieAnimationFinishedPlaying -= PlayerDamage_OnPlayerDieAnimationFinishedPlaying;
        PlayerDamage.OnPlayerHitAnimationFinishedPlaying -= PlayerDamage_OnPlayerHitAnimationFinishedPlaying;
        Health.OnDied -= PlayDeathAnimationOnDied;
        Health.OnHit -= PlayHitAnimationOnHit;
    }

    private void PlayHitAnimationOnHit(GameObject passedObject)
    {
        if(passedObject.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySound("Hit");
            gameObject.GetComponentInChildren<Animator>().Play("Hit");
        }

    }

    private void PlayDeathAnimationOnDied(GameObject passedObject)
    {
        if (passedObject.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySound("Death");
            gameObject.GetComponentInChildren<Animator>().Play("Die");
        }

    }

    private void PlayerDamage_OnPlayerHitAnimationFinishedPlaying()
    {
        GetComponentInChildren<Animator>().SetBool("PlayerHit", false);
    }

    private void PlayerDamage_OnPlayerDieAnimationFinishedPlaying()
    {
        gameObject.SetActive(false);
    }

}
