using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShipMovement : MonoBehaviour
{
    private GameObject player;
    private Vector3 startingPosition;
    [SerializeField]
    private float timeDelayBetweenFireAnimationStartAndSoundPlay = 0.5f;
    [SerializeField]
    private float timeDelayBetweenSoundPlayAndGunHit = 0.5f;
    [SerializeField]
    private float maximumYSeperationBetweenPlayerAndShip = 15f, maximumXZSeperationBetweenPlayerAndShip = 10f;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
            return;
        if ((transform.position.y - player.transform.position.y < maximumYSeperationBetweenPlayerAndShip))
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, maximumYSeperationBetweenPlayerAndShip, transform.position.z), Time.deltaTime * 0.5f);
        }
        if (Mathf.Sqrt(
            Mathf.Pow((transform.position.x - player.transform.position.x), 2f)
            + Mathf.Pow((transform.position.z - player.transform.position.z), 2f)
            )
            > maximumXZSeperationBetweenPlayerAndShip)
        {
            transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime * 0.5f);
        }
    }
}
