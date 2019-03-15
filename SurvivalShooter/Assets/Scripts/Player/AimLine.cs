using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimLine : MonoBehaviour {
    LineRenderer gunLine;
    Ray shootRay = new Ray();
    RaycastHit shootHit;
    int shootableMask;
    public float range = 100f;
    // Use this for initialization
    void Awake () {
        shootableMask = LayerMask.GetMask("Shootable");
        gunLine = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position);
        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;
        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            gunLine.SetPosition(1, shootHit.point);
        }
        else
        {
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }
    }
}
