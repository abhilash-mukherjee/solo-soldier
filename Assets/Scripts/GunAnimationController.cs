using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAnimationController : MonoBehaviour
{
    public void StopFireAnimation()
    {
        GetComponent<Animator>().SetBool("Fire", false);
    }
}
