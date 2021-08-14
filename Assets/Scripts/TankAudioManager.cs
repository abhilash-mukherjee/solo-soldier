using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TankAudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource[] tankAudioSources;
    private TankMovement tankMovement;

    void Start()
    {
        tankAudioSources[0].Play();
        tankMovement = gameObject.GetComponent<TankMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayMovingSound();
    }

    private void PlayMovingSound()
    {
        if (tankMovement.isMoving == true && !tankAudioSources[1].isPlaying)
        {
            tankAudioSources[1].PlayOneShot(tankAudioSources[1].clip);
        }
        else if (tankMovement.isMoving == false)
        {
            tankAudioSources[1].Stop();
        }
    }

    public enum SoundClipType
    {
        Idle,
        Moving,
        Fire
    }
}
