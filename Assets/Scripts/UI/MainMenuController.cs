using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    GameObject[] levels;
    [SerializeField]
    private GameObject menuBoard, levelBoard;
    [SerializeField]
    private float lerpSpeed = 10f;
    private int m_highestLevelAchieved;
    private bool m_isLastBattleWon;
    private bool m_menuBarScaleCompleted = false;
    private bool m_levelSpritesFilled = false;
    private void Start()
    {
        menuBoard.transform.localScale = Vector3.zero;
        levelBoard.transform.localScale = Vector3.zero;
        m_highestLevelAchieved = GameManager.Instance.highestLevelAchieved;
        m_isLastBattleWon = GameManager.Instance.isLastBattleWon;
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].GetComponent<LevelSpriteFillController>().LevelIsInactive();
        }

    }
    private void Update()
    {
        MenuBarLerpAnimation();
    }

    private void MenuBarLerpAnimation()
    {
        if (m_menuBarScaleCompleted)
            return;
        menuBoard.transform.localScale = Vector3.Slerp(menuBoard.transform.localScale, Vector3.one, Time.deltaTime * lerpSpeed);
        levelBoard.transform.localScale = Vector3.Slerp(levelBoard.transform.localScale, Vector3.one, Time.deltaTime * lerpSpeed);
        if(menuBoard.transform.localScale == Vector3.one && levelBoard.transform.localScale == Vector3.one)
        {
            FillLevelSprites();
            m_menuBarScaleCompleted = true;
        }
    }
    private void FillLevelSprites()
    {
        if (m_levelSpritesFilled)
            return;
        for(int i =0; i<levels.Length; i++)
        {
            if(i <= m_highestLevelAchieved)
            {
                if(i==m_highestLevelAchieved && m_isLastBattleWon == true)
                {
                    levels[i].GetComponent<LevelSpriteFillController>().ActivateLevel();
                }

                else
                {
                    levels[i].GetComponent<LevelSpriteFillController>().LevelIsActive();
                }
            }
            else
            {
                levels[i].GetComponent<LevelSpriteFillController>().LevelIsInactive();
            }
        }

        m_levelSpritesFilled = true;
    }
}
