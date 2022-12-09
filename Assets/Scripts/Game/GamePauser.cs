using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePauser : MonoBehaviour
{
    [SerializeField] private GameObject pauseButton, resumePanel;
    [SerializeField] private KeyCode pauseKey;
    [SerializeField]
    private BoolVariable isGamePaused;
    private void Awake()
    {
        isGamePaused.Value = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(pauseKey)) PauseGame();
    }

    public void PauseGame()
    {
        isGamePaused.Value = true;

            Time.timeScale = 0f;
            pauseButton.SetActive(false);
            resumePanel.SetActive(true);

        
    }
    public void ResumeGame()
    {
        isGamePaused.Value = false;
        Time.timeScale = 1f;
        pauseButton.SetActive(true);
        resumePanel.SetActive(false);
    }

    public void QuitGame()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            SceneManager.LoadScene(0);
        }
        else
        Application.Quit();
    }

}
