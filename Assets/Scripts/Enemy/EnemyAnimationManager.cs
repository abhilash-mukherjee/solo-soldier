using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationManager : MonoBehaviour
{
    public delegate void EnemyDeathHandler();
    public static event EnemyDeathHandler OnEnemyDied;
    private void OnEnable()
    {
        EnemyDamage.OnEnemyDieAnimationFinishedPlaying += EnemyDamage_OnEnemyDieAnimationFinishedPlaying;
        EnemyDamage.OnEnemyHitAnimationFinishedPlaying += EnemyDamage_OnEnemyHitAnimationFinishedPlaying;
        EnemyHealth.OnDied += PlayDeathAnimationOnDied;
        EnemyHealth.OnHit += PlayHitAnimationOnHit;
    }
    private void OnDisable()
    {
        EnemyDamage.OnEnemyDieAnimationFinishedPlaying -= EnemyDamage_OnEnemyDieAnimationFinishedPlaying;
        EnemyDamage.OnEnemyHitAnimationFinishedPlaying -= EnemyDamage_OnEnemyHitAnimationFinishedPlaying;
        EnemyHealth.OnDied -= PlayDeathAnimationOnDied;
        EnemyHealth.OnHit -= PlayHitAnimationOnHit;
    }

    private void PlayHitAnimationOnHit(GameObject passedObject)
    {
        if(passedObject == gameObject)
        {
            AudioManager.Instance.PlaySoundOneShot("EnemyHit");
            gameObject.GetComponentInChildren<Animator>().Play("EnemyHit");
        }

    }

    private void PlayDeathAnimationOnDied(GameObject passedObject)
    {
        if (passedObject == gameObject)
        {
            AudioManager.Instance.PlaySoundOneShot("EnemyDeath");
            gameObject.GetComponentInChildren<Animator>().Play("Die");
        }

    }

    private void EnemyDamage_OnEnemyHitAnimationFinishedPlaying(GameObject enemyObject)
    {
        try
        {
            if (enemyObject.transform.parent.gameObject == gameObject)
            {
                GetComponentInChildren<Animator>().SetBool("HitEnemy", false);
            }
        } 
        catch
        {
            return;
        }
    }

    private void EnemyDamage_OnEnemyDieAnimationFinishedPlaying(GameObject enemyObject)
    {
        try
        {
            if (enemyObject.transform.parent.gameObject == gameObject)
            {
                Destroy(gameObject);
                OnEnemyDied?.Invoke();
            }
        }
        catch
        {
            return;
        }
    }

}
