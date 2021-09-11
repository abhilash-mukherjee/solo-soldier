using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireAnimationManager : MonoBehaviour
{
    public void StartMoving()
    {
        transform.parent.gameObject.GetComponent<PlayerMovement>().StartPlayerMovement();
    }
    public void StopPlayerFireAnimation()
    {
        GetComponent<Animator>().SetBool("Fire", false);
    }
}
