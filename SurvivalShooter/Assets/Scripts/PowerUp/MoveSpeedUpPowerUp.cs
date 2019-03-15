using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpeedUpPowerUp : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private void Awake()
    {
        Destroy(gameObject, 20);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerMovement = other.GetComponent<PlayerMovement>();
            if (!playerMovement.PowerUpIsActive())
            {
                playerMovement.ActivePowerUp();
            }
            Destroy(gameObject);
        }
    }
}
