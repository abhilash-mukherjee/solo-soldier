using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorManager : MonoBehaviour
{
    [SerializeField]
    private int MainMenueSceneIndex = 0;

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.buildIndex == MainMenueSceneIndex)
        {
            Debug.Log("Main Menu Loaded");
            Cursor.lockState = CursorLockMode.None;
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        PlayerHealth.OnPlayerDied -= UnlockCursor;

    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Debug.Log("Cursor Unlocked");
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        PlayerHealth.OnPlayerDied -= UnlockCursor;

    }

}
