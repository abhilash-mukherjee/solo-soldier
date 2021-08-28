using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrenadeTossAnimationManager : MonoBehaviour
{
    private bool m_pauseGrendaeTossSound = false;
    private bool m_isPlayingGrenadeTossAnimation = false;
    public bool IsPlayingGrenadeTossAnimation
    {
        get { return m_isPlayingGrenadeTossAnimation; }
    }
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
 
    public void ReleaseGrenade()
    {
        transform.parent.gameObject.GetComponent<PlayerGrenadeTossController>().ReleaseGrenade();
    }
    public void GrenadeAnimationPlaying()
    {
        m_isPlayingGrenadeTossAnimation = true;
    }
    public void GrenadeAnimationPlayed()
    {
        m_isPlayingGrenadeTossAnimation = false;
    }
    public void StopGrenadeTossSound()
    {
        AudioManager.Instance.PauseSound("GrenadeToss");
    }
    public void AllowGrenadeTossSound()
    {
        m_pauseGrendaeTossSound = false;
    }
    public void DisAllowGrenadeTossSound()
    {
        m_pauseGrendaeTossSound = true;
    }
    private void Update()
    {
        if (m_pauseGrendaeTossSound == false)
            return;
        else
        {
            StopGrenadeTossSound();
        }

    }
}
