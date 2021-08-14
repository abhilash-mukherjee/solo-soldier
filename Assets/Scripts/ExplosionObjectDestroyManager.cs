using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionObjectDestroyManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float duration = GetComponent<ParticleSystem>().main.duration;
        StartCoroutine(DestroyParticleSystem(duration));

    }

    IEnumerator DestroyParticleSystem(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
