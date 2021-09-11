using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeSoundManager : MonoBehaviour
{
    private bool m_hasCollided = false;
    private void OnCollisionEnter(Collision collision)
    {
        if (m_hasCollided == true)
            return;
        if (collision.gameObject.CompareTag("Enemy")
            || collision.gameObject.CompareTag("Tank")
            || collision.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
            m_hasCollided = true;
            AudioManager.Instance.PlaySoundOneShot("GrenadeHitGround");
        }
    }
}
