using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySmokeParticles : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroySmoke(GetComponent<ParticleSystem>().main.duration));
    }
    IEnumerator DestroySmoke(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
