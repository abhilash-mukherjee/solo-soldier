using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShipAnimationManager : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerDamage.OnPlayerDieAnimationFinishedPlaying += PlayerDamage_OnPlayerDieAnimationFinishedPlaying;
        PlayerDamage.OnPlayerHitAnimationFinishedPlaying += PlayerDamage_OnPlayerHitAnimationFinishedPlaying;
        PlayerHealth.OnDied += PlayDeathAnimationOnDied;
    }
    private void OnDisable()
    {
        PlayerDamage.OnPlayerDieAnimationFinishedPlaying -= PlayerDamage_OnPlayerDieAnimationFinishedPlaying;
        PlayerDamage.OnPlayerHitAnimationFinishedPlaying -= PlayerDamage_OnPlayerHitAnimationFinishedPlaying;
        PlayerHealth.OnDied -= PlayDeathAnimationOnDied;
    }

    private void PlayHitAnimationOnHit(GameObject passedObject)
    {
        if (passedObject.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySound("PlayerHit");
            gameObject.GetComponentInChildren<Animator>().Play("Hit");
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
