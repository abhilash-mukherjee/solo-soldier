using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    [SerializeField]
    private float angularSpeed = 10f;
    [SerializeField]
    private string KEY_SOUND;
    private Vector3 m_rotationVector;
    
    private void Update()
    {
        m_rotationVector = new Vector3(0f, 0f,Time.deltaTime * angularSpeed);
        transform.Rotate(m_rotationVector);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Key collected");
            AudioManager.Instance.PlaySoundOneShot(KEY_SOUND);
            KeySpawner.isKeySpawned = false;
            GameManager.Instance.UnlockNextLevel();
        }
    }
}
