using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI healthText;
    [SerializeField]
    private Slider healthSlider;
    private void OnEnable()
    {
        PlayerHealth.OnHealthChanged += ChangeHealthSlide;
        EnemyHealth.OnHealthChanged += ChangeHealthSlide;
        GunShipHealth.OnHealthChanged += ChangeHealthSlide;
        TankHealth.OnHealthChanged += ChangeHealthSlide;
    }
    private void OnDisable()
    {
        PlayerHealth.OnHealthChanged -= ChangeHealthSlide;
        EnemyHealth.OnHealthChanged -= ChangeHealthSlide;
        GunShipHealth.OnHealthChanged -= ChangeHealthSlide;
        TankHealth.OnHealthChanged -= ChangeHealthSlide;
    }
    private void ChangeHealthSlide(GameObject _gameObject, int _CurrentHealth, int _startingHealth)
    {
        if(_gameObject == transform.parent.gameObject && transform.parent.gameObject.CompareTag("GunShip"))
        {
            healthSlider.maxValue = _startingHealth;
            healthSlider.value = _CurrentHealth;
            healthText.text = ((float)_CurrentHealth *100f / (float)_startingHealth).ToString() + "%";
        }
        if(_gameObject == transform.parent.gameObject && transform.parent.gameObject.CompareTag("Enemy"))
        {
            healthSlider.maxValue = _startingHealth;
            healthSlider.value = _CurrentHealth;
            healthText.text = ((float)_CurrentHealth *100f / (float)_startingHealth).ToString() + "%";
        }

        else if(_gameObject.transform.parent != null)
        {
            if(_gameObject == transform.parent.gameObject && transform.parent.gameObject.CompareTag("Tank"))
            {
                healthSlider.maxValue = _startingHealth;
                healthSlider.value = _CurrentHealth;
                healthText.text = ((float)_CurrentHealth * 100f / (float)_startingHealth).ToString() + "%";
            }
        }
        
        else if(_gameObject.layer == LayerMask.NameToLayer("Player") && transform.parent.gameObject.CompareTag("Player"))
        {
            healthSlider.maxValue = _startingHealth;
            healthSlider.value = _CurrentHealth;
            healthText.text = ((float)_CurrentHealth * 100f / (float)_startingHealth).ToString() + "%";
        }
    }
}
