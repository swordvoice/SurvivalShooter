using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraUISetup : MonoBehaviour
{
    private PhotonView PV;
    public Camera mainCamera;
    public Camera miniMapCamera;
    public Slider healthSlider;
    public Image damageImage;
    public AudioClip deathClip;
    // Start is called before the first frame update
    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
