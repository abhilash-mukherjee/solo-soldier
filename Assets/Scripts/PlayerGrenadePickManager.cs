using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrenadePickManager : MonoBehaviour
{
    public delegate void GrenadePickHandler();
    public static event GrenadePickHandler OnGrenadePicked;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Grenade"))
        {
            if (collision.gameObject.GetComponent<GrenadeExplosionManager>() == null)
            {
                OnGrenadePicked?.Invoke();
                AudioManager.Instance.PlaySoundOneShot("GrenadePick");
                Destroy(collision.gameObject);
            }
        }
    }
}
