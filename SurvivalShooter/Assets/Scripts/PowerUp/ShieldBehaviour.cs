using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBehaviour : MonoBehaviour {
    private float activedTime=0;
    public float activeTime=10f;
    private void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            activedTime = activedTime + Time.deltaTime;
        }
        if(gameObject.activeInHierarchy && activedTime > activeTime)
        {
            gameObject.SetActive(false);
            activedTime = 0;
        }
    }
}
