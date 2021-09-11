using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LevelSpriteFillController : MonoBehaviour
{
    [SerializeField]
    private Image levelImage;
    [SerializeField]
    private Image levelImageBorder;
    [SerializeField]
    private Image lineFill;
    [SerializeField]
    private TextMeshProUGUI levelTitle;
    [SerializeField]
    private Color32 inActiveTextColor, activeTextColor, inActiveImageColor, activeImageColor;
    [SerializeField]
    private float m_lineFillTime, m_circleFillTime;
    private bool m_activateLevel = false;
    private bool m_shouldFillLine = false;
    private bool m_shouldFillCircle = false;
    private float m_time = 0;
    public void LevelIsActive()
    {
        if(lineFill != null)
        {
            lineFill.fillAmount = 1.0f;
            lineFill.color = activeTextColor;
        }
        levelImageBorder.fillAmount = 1.0f;
        levelImageBorder.color = activeTextColor;
        levelImage.color = activeImageColor;
        levelTitle.color = activeTextColor;
        GetComponent<Button>().interactable = true;
    }
    public void LevelIsInactive()
    {
        if (lineFill != null)
        {
            lineFill.fillAmount = 0f;
        }
        levelImageBorder.fillAmount = 0f;
        levelImage.color = inActiveImageColor;
        levelTitle.color = inActiveTextColor;
        GetComponent<Button>().interactable = false;
    }
    public void ActivateLevel()
    {
        m_activateLevel = true;
        m_shouldFillLine = true;
    }

    private void Update()
    {
        if (m_activateLevel == false)
            return;
        if(lineFill != null)
        {
            FillLine();
        }
        FillCircle();
    }

    private void FillLine()
    {
        if (m_shouldFillLine == false)
            return;
        if (m_time > m_lineFillTime)
        {
            m_time = 0;
            m_shouldFillCircle = true;
            m_shouldFillLine = false;
            return;
        }
        m_time += Time.deltaTime;
        lineFill.fillAmount = m_time / m_lineFillTime;
    }

    private void FillCircle()
    {
        if (m_shouldFillCircle == false)
            return;
        if (m_time > m_circleFillTime)
        {
            m_time = 0;
            m_shouldFillCircle = false;
            m_activateLevel = false;
            ChangeTextColor();
            ChangeImageColor();
            GetComponent<Button>().interactable = true;
            return;
        }
        m_time += Time.deltaTime;
        levelImageBorder.fillAmount = m_time / m_circleFillTime;
    }

    private void ChangeImageColor()
    {
        levelImage.color = activeImageColor;
    }

    private void ChangeTextColor()
    {
        levelTitle.color = activeTextColor;
    }

}
