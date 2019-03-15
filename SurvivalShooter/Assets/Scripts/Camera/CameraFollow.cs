using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Transform target;
    public float smoothing = 5f;
    Vector3 offset;
    private void Start()
    {
        Vector3 po = new Vector3(0, 0, 0);
        offset = transform.position - po;
    }
    private void FixedUpdate()
    {
        Vector3 targetCamPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos,smoothing * Time.deltaTime);
    }
}
