using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    private string levelToLoadOnButtonClick;
    [SerializeField]
    private string LOAD_LEVEL_AUDIO;
    public void LoadLevel()
    {
        SceneManager.LoadScene(levelToLoadOnButtonClick);
        AudioManager.Instance.PlaySoundOneShot(LOAD_LEVEL_AUDIO);
    }
}
