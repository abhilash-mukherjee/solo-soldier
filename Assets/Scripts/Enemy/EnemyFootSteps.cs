using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFootSteps : MonoBehaviour
{
    // Start is called before the first frame update
    public void Step()
    {
        AudioManager.Instance.PlaySoundOneShot($"EnemyStep{Random.Range(1, 5)}");
    }

}
