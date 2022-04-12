using UnityEngine;
using System.Collections.Generic;

public class PowerUpManager : MonoBehaviour
{
    
    private int m_healthBoost;
    public int HealthBoost
    {
        set { m_healthBoost = value; }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Power Up");
            Destroy(gameObject);
            AudioManager.Instance.PlaySoundOneShot("PowerUp");
            var _player = GameObject.FindGameObjectWithTag("Player");
            if(_player != null)
            {
            var _healthComponent = _player?.GetComponent<PlayerHealth>();
                if (_healthComponent != null)
                {
                    _healthComponent.CurrentHealth += m_healthBoost;
                }

            }
        }
    }
}

