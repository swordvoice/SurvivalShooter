using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRateUpPowerUp : MonoBehaviour
{
    private PlayerShooting playerShooting;
    private void Awake()
    {
        Destroy(gameObject, 20);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerShooting = other.GetComponentInChildren<PlayerShooting>();
            if (!playerShooting.PowerUpIsActive())
            {
                playerShooting.ActivePowerUp();
            }
            Destroy(gameObject);
        }
    }
}
