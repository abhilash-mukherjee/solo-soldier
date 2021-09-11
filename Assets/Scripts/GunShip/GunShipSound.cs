using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShipSound : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlaySound("helicopter");
    }
    private void OnDisable()
    {
        AudioManager.Instance.PauseSound("helicopter");
    }
}
