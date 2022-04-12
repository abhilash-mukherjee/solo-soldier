using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public List<Sound> sounds;
    [SerializeField]
    private string backgroundMusicName;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.pitch = s.pitch;
            s.source.volume = s.volume;
            s.source.loop = s.loop;

        }
    }

    void Start()
    {
        PlaySound(backgroundMusicName);
    }
    public void PlaySound(string clipName)
    {
        var audio = sounds.First(s => s.name.Equals(clipName));
        if(audio == null)
        {
            Debug.LogWarning("Sound Does not exist");
        }
        else
        {
            audio.source.Play();
        }
        
    }
    public void PauseSound(string clipName)
    {
        var audio = sounds.First(s => s.name.Equals(clipName));
        if (audio == null)
        {
            Debug.LogWarning("Sound Does not exist");
        }
        else if (audio.source == null)
        {
            Debug.LogWarning("Sound Does not exist");
        }
        else
        {
            if(audio.source.isPlaying)
            audio.source.Stop();
        }

    }
    public void PlaySoundOneShot(string clipName)
    {
        var audio = sounds.First(s => s.name.Equals(clipName));
        if (audio == null)
        {
            Debug.LogWarning("Sound Does not exist");
        }
        else
        {
            audio.source.PlayOneShot(audio.source.clip);
        }

    }
}
