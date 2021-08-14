using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrenadeTossAnimationManager : MonoBehaviour
{
   public void StartMoving()
    {
        transform.parent.gameObject.GetComponent<PlayerMovement>().StartPlayerMovement();
    }
    public void StopGrenadeTossAnimation()
    {
        GetComponent<Animator>().SetBool("TossGrenade", false);
    }
    public void StartGrenadeToss()
    {
        transform.parent.gameObject.GetComponent<PlayerGrenadeTossController>().StartGrenadeToss();
    }
    public void StopGrenadeToss()
    {
        transform.parent.gameObject.GetComponent<PlayerGrenadeTossController>().StopGrenadeToss();
    }
    public void ReleaseGrenade()
    {
        transform.parent.gameObject.GetComponent<PlayerGrenadeTossController>().ReleaseGrenade();
    }
}
