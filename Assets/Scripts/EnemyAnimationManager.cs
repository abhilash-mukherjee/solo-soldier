using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationManager : MonoBehaviour
{
    private void OnEnable()
    {
        EnemyDamage.OnEnemyDieAnimationFinishedPlaying += EnemyDamage_OnEnemyDieAnimationFinishedPlaying;
        EnemyDamage.OnEnemyHitAnimationFinishedPlaying += EnemyDamage_OnEnemyHitAnimationFinishedPlaying;
        Health.OnDied += PlayDeathAnimationOnDied;
        Health.OnHit += PlayHitAnimationOnHit;
    }
    private void OnDisable()
    {
        EnemyDamage.OnEnemyDieAnimationFinishedPlaying -= EnemyDamage_OnEnemyDieAnimationFinishedPlaying;
        EnemyDamage.OnEnemyHitAnimationFinishedPlaying -= EnemyDamage_OnEnemyHitAnimationFinishedPlaying;
        Health.OnDied -= PlayDeathAnimationOnDied;
        Health.OnHit -= PlayHitAnimationOnHit;
    }

    private void PlayHitAnimationOnHit(GameObject passedObject)
    {
        if(passedObject.CompareTag("Enemy"))
        {
            AudioManager.Instance.PlaySound("Hit");
            gameObject.GetComponentInChildren<Animator>().Play("EnemyHit");
        }

    }

    private void PlayDeathAnimationOnDied(GameObject passedObject)
    {
        if (passedObject.CompareTag("Enemy"))
        {
            AudioManager.Instance.PlaySound("Death");
            gameObject.GetComponentInChildren<Animator>().Play("Die");
        }

    }

    private void EnemyDamage_OnEnemyHitAnimationFinishedPlaying()
    {
        GetComponentInChildren<Animator>().SetBool("HitEnemy", false);
    }

    private void EnemyDamage_OnEnemyDieAnimationFinishedPlaying()
    {
        gameObject.SetActive(false);
    }

}
