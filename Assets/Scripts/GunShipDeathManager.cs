using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShipDeathManager : MonoBehaviour
{
    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private float explosionTime = 1f;
    void OnEnable()
    {
        GunShipHealth.OnGunShipDied += DestroyGunShip;
    }
    void OnDisable()
    {
        GunShipHealth.OnGunShipDied -= DestroyGunShip;
    }

    private void DestroyGunShip()
    {
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        AudioManager.Instance.PlaySoundOneShot("GrenadeExplosion");
        transform.GetChild(0).gameObject.SetActive(false);
        StartCoroutine(DestroyParent());
    }
    IEnumerator DestroyParent()
    {
        yield return new WaitForSeconds(explosionTime);
        Destroy(gameObject);
    }
}
