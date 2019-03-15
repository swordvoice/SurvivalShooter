using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonPlayerShooting : MonoBehaviour
{
#if UNITY_ANDROID
    public SimpleTurnPad turnPad;
    public SimpleTouchButton touchButton;
#endif
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float timeBetweenShells = 2f;
    public float range = 100f;
    public Rigidbody shell;
    public float launchForce = 20f;
    public AudioClip shootfire;
    public AudioClip shellfire;

    private bool powerUp = false;
    private float activedTime = 0f;
    public float activeTime = 10f;
    float timershoot;
    float timershell;
    Ray shootRay = new Ray();
    int shootableMask;
    ParticleSystem gunParticles;
    LineRenderer gunLine;
    AudioSource gunAudio;
    Light gunLight;
    float effectsDisplayTime = 1f;

    private PhotonView PV;
    void Awake()
    {
        shootableMask = LayerMask.GetMask("Shootable");
        gunParticles = GetComponent<ParticleSystem>();
        gunLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponent<Light>();
        PV = GetComponent<PhotonView>();
    }


    void Update()
    {
        timershoot += Time.deltaTime;
        timershell += Time.deltaTime;
        if (PV.IsMine)
        {
#if UNITY_ANDROID
        if (turnPad.GetTouch() && timershoot >= timeBetweenBullets && Time.timeScale != 0)
#else
        if (Input.GetButton("Fire1") && timershoot >= timeBetweenBullets && Time.timeScale != 0)
#endif
            {
                Debug.Log(PhotonRoom.room.playerInGame);
                //RPC_Shoot();
                PV.RPC("RPC_Shoot", RpcTarget.AllBuffered);
            }
#if UNITY_ANDROID
        if (touchButton.GetTouch() && timershell >= timeBetweenShells && Time.timeScale != 0)
#else
            if (Input.GetButton("Fire2") && timershell >= timeBetweenShells && Time.timeScale != 0)
#endif
            {
                PV.RPC("RPC_ShellShoot", RpcTarget.AllBuffered);
            }
        }

        if (timershoot >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects();
        }
        if (powerUp)
        {
            CountPowerUpTime();
        }
    }


    public void DisableEffects()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }

    [PunRPC]
    void RPC_ShellShoot()
    {
        gunAudio.clip = shellfire;
        gunAudio.Play();
        timershell = 0f;
        Rigidbody shellInstance = Instantiate(shell, transform.position, transform.rotation) as Rigidbody;
        shellInstance.velocity = launchForce * transform.forward;
    }
    [PunRPC]
    void RPC_Shoot()
    {
        timershoot = 0f;

        RaycastHit shootHit;
        gunAudio.clip = shootfire;
        gunAudio.Play();

        gunLight.enabled = true;

        gunParticles.Stop();
        gunParticles.Play();

        gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position);

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {

            gunLine.SetPosition(1, shootHit.point);
            PhotonPlayerHealth playerHealth = shootHit.transform.GetComponent<PhotonPlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damagePerShot);
            }
            Debug.Log(shootHit.point);
            Debug.Log("Hit!");
            Debug.Log(gunLine.enabled);
        }
        else
        {
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
            Debug.Log("No Hit!");
        }
        Debug.Log("Shoot!");
    }
    public bool PowerUpIsActive()
    {
        return powerUp;
    }
    public void ActivePowerUp()
    {
        powerUp = true;
        timeBetweenBullets = timeBetweenBullets / 1.5f;
    }
    public void DeactivatePowerUp()
    {
        activedTime = 0;
        powerUp = false;
        timeBetweenBullets = timeBetweenBullets * 1.5f;
    }
    private void CountPowerUpTime()
    {
        activedTime += Time.deltaTime;
        if (activedTime >= activeTime)
        {
            DeactivatePowerUp();
        }
    }
}
