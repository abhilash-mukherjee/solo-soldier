using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCanvasController : MonoBehaviour
{
    [SerializeField]
    GameObject targetSprite, grenadeCounter, grenadeTimer, gameOverText, mainMenuButton, successText;
    [SerializeField]
    private float lerpSpeed = 10f;
    [SerializeField]
    private string LOAD_MAIN_MENU_AUDIO;
    private bool m_shouldPlayGameOverAnimation = false;
    private bool m_shouldPlaySuccessAnimation = false;

    private void Awake()
    {
        gameOverText.transform.localScale = Vector3.zero;
        successText.transform.localScale = Vector3.zero;
        mainMenuButton.SetActive(false);
        gameOverText.SetActive(false);
        successText.SetActive(false);
    
    }
    private void OnEnable()
    {
        GameManager.OnGameOver += LoadGameOver;
        GameManager.OnLevelFinished += LoadSuccessMessege;
    }
    private void OnDisable()
    {
        GameManager.OnGameOver -= LoadGameOver;
        GameManager.OnLevelFinished -= LoadSuccessMessege;
        
    }

    private void LoadSuccessMessege()
    {
        successText.SetActive(true);
        m_shouldPlaySuccessAnimation = true;
    }
    private void LoadGameOver()
    {
        targetSprite.SetActive(false);
        grenadeCounter.SetActive(false);
        grenadeTimer.SetActive(false);
        mainMenuButton.SetActive(true);
        gameOverText.SetActive(true);
        m_shouldPlayGameOverAnimation = true;
    }

    private void Update()
    {
        GameOverAnimation();
        SuccessAnimation();
    }

    private void SuccessAnimation()
    {
        if (!m_shouldPlaySuccessAnimation)
            return;
        if (successText.transform.localScale == Vector3.one)
        {
            m_shouldPlaySuccessAnimation = false;
            return;
        }
        successText.transform.localScale = Vector3.Slerp(successText.transform.localScale, Vector3.one, Time.deltaTime * lerpSpeed);
    }

    private void GameOverAnimation()
    {
        if (!m_shouldPlayGameOverAnimation)
            return;
        if(gameOverText.transform.localScale == Vector3.one)
        {
            m_shouldPlayGameOverAnimation = false;
            return;
        }
        gameOverText.transform.localScale = Vector3.Slerp(gameOverText.transform.localScale, Vector3.one, Time.deltaTime * lerpSpeed);
    }

    public void LoadMainMenu()
    {
        AudioManager.Instance.PlaySound(LOAD_MAIN_MENU_AUDIO);
        GameManager.Instance.LoadMainMenu();
    }
}
