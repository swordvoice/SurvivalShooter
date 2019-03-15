using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerUp : MonoBehaviour {
    private void Awake()
    {
        Destroy(gameObject, 20);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!other.transform.GetChild(4).gameObject.activeInHierarchy)
            {
                other.transform.GetChild(4).gameObject.SetActive(true);
            }
            Destroy(gameObject);
        }
        
    }
}
