using Photon.Pun;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
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

    private bool powerUp=false;
    private float activedTime = 0f;
    public float activeTime = 10f;
    float timershoot;
    float timershell;
    Ray shootRay = new Ray();
    RaycastHit shootHit;
    int shootableMask;
    ParticleSystem gunParticles;
    LineRenderer gunLine;
    AudioSource gunAudio;
    Light gunLight;
    float effectsDisplayTime = 0.2f;

    void Awake ()
    {
        shootableMask = LayerMask.GetMask ("Shootable");
        gunParticles = GetComponent<ParticleSystem> ();
        gunLine = GetComponent <LineRenderer> ();
        gunAudio = GetComponent<AudioSource> ();
        gunLight = GetComponent<Light> ();
    }


    void Update ()
    {
        timershoot += Time.deltaTime;
        timershell += Time.deltaTime;
#if UNITY_ANDROID
        if (turnPad.GetTouch() && timershoot >= timeBetweenBullets && Time.timeScale != 0)
#else
        if (Input.GetButton("Fire1") && timershoot >= timeBetweenBullets && Time.timeScale != 0)
#endif
        {
            Shoot();
        }
#if UNITY_ANDROID
        if (touchButton.GetTouch() && timershell >= timeBetweenShells && Time.timeScale != 0)
#else
        if (Input.GetButton("Fire2") && timershell >= timeBetweenShells && Time.timeScale != 0)
#endif
        {
            ShellShoot();
        }

        if (timershoot >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects ();
        }
        if (powerUp)
        {
            CountPowerUpTime();
        }
    }


    public void DisableEffects ()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }

    void ShellShoot()
    {
        gunAudio.clip = shellfire;
        gunAudio.Play();
        timershell = 0f;
        Rigidbody shellInstance = Instantiate(shell, transform.position, transform.rotation) as Rigidbody;
        shellInstance.velocity = launchForce * transform.forward;
    }

    void Shoot ()
    {
        timershoot = 0f;

        gunAudio.clip = shootfire;
        gunAudio.Play ();

        gunLight.enabled = true;

        gunParticles.Stop ();
        gunParticles.Play ();

        gunLine.enabled = true;
        gunLine.SetPosition (0, transform.position);

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
        {
            EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();
            if(enemyHealth != null)
            {
                enemyHealth.TakeDamage (damagePerShot, shootHit.point);
            }
            gunLine.SetPosition (1, shootHit.point);
        }
        else
        {
            gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
        }
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
