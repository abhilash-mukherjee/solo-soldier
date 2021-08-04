using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootSteps : MonoBehaviour
{
    // Start is called before the first frame update
    public void Step()
    {
        AudioManager.Instance.PlaySoundOneShot($"PlayerStep{Random.Range(1, 5)}");
    }

}
