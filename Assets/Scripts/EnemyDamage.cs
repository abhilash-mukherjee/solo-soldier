using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public delegate void EnemyDeathHandler(GameObject enemyObject);
    public static event EnemyDeathHandler OnEnemyDieAnimationFinishedPlaying;
    public delegate void EnemyHitHandler(GameObject enemyObject);
    public static event EnemyHitHandler OnEnemyHitAnimationFinishedPlaying;
    private bool deathStarted = false;
    private bool deathEnd = false;
    [SerializeField]
    private float yVelocityDuringDeath;
    public void EnemyDied()
    {
        OnEnemyDieAnimationFinishedPlaying?.Invoke(gameObject);
        deathEnd = true;
    }
    public void EnemyHit()
    {
        OnEnemyHitAnimationFinishedPlaying?.Invoke(gameObject);
    }
    public void DeathStarted()
    {
        deathStarted = true;
    }

    private void Update()
    {
        if(deathStarted && !deathEnd)
        {
            gameObject.transform.Translate(Time.deltaTime * yVelocityDuringDeath * new Vector3(0f, -1f, 0f));
        }
    }
}
