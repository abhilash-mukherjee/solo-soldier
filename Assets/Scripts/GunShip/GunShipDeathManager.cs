using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShipDeathManager : MonoBehaviour
{
    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private float explosionTime = 1f;

    public delegate void GunShipDeathHandler();
    public static event GunShipDeathHandler OnGunShipDied;
    public delegate void DeathHandler(GameObject _gameObject);
    public static event DeathHandler OnDied;
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
        OnGunShipDied?.Invoke();
        OnDied?.Invoke(gameObject);
    }
}
