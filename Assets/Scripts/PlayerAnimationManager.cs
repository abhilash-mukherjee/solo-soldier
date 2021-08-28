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
        PlayerHealth.OnDied += PlayDeathAnimationOnDied;
        PlayerHealth.OnPlayerHit += PlayHitAnimationOnHit;
    }
    private void OnDisable()
    {
        PlayerDamage.OnPlayerDieAnimationFinishedPlaying -= PlayerDamage_OnPlayerDieAnimationFinishedPlaying;
        PlayerDamage.OnPlayerHitAnimationFinishedPlaying -= PlayerDamage_OnPlayerHitAnimationFinishedPlaying;
        PlayerHealth.OnDied -= PlayDeathAnimationOnDied;
        PlayerHealth.OnPlayerHit -= PlayHitAnimationOnHit;
    }

    private void PlayHitAnimationOnHit(GameObject passedObject, int dmg)
    {
        if(passedObject.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySound("PlayerHit");
            
           if(dmg <=1)
            {
                if (gameObject.GetComponentInChildren<PlayerGrenadeTossAnimationManager>().IsPlayingGrenadeTossAnimation == false)
                {
                    gameObject.GetComponentInChildren<Animator>().Play("Hit");
                    StopOnGoingAnimations();

                }
            }
            else if(dmg >1)
            {
                StopOnGoingAnimations();
                gameObject.GetComponentInChildren<Animator>().Play("Hit");
                gameObject.GetComponent<PlayerGrenadeTossController>().StopGrenadeToss();
            }
        }

    }
    private void StopOnGoingAnimations()
    {
        StopGrenadeAnimation();
    }
    private void StopFireAnimation()
    {
        gameObject.GetComponentInChildren<Animator>().SetBool("Fire", false);
    }
    private void StopGrenadeAnimation()
    {
        gameObject.GetComponentInChildren<Animator>().SetBool("TossGrenade", false);
        if(gameObject.GetComponent<PlayerGrenadeTossController>().Grenade != null)
        {
            Destroy(gameObject.GetComponent<PlayerGrenadeTossController>().Grenade);
        }
    }

    private void PlayDeathAnimationOnDied(GameObject passedObject)
    {
        if (passedObject.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySound("PlayerDeath");
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
