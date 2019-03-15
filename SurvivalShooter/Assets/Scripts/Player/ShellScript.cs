using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellScript : MonoBehaviour {

    int shootableMask;
    int damage = 100;
    public float maxLifeTime = 2f;
    public float explosionRadius = 5f;
    public float explosionForce = 1000f;
    public ParticleSystem explosionParticles;
    public AudioSource explosionAudio;

    private void Awake()
    {
        shootableMask = LayerMask.GetMask("Shootable");
    }

    // Use this for initialization
    void Start () {
        Destroy(gameObject, maxLifeTime);
		
	}
    private void OnTriggerEnter(Collider other)
    {
        Collider[] collider = Physics.OverlapSphere(transform.position, explosionRadius, shootableMask);
        for(int i = 0; i < collider.Length; i++)
        {
            Rigidbody enemyRigidBody = collider[i].GetComponent<Rigidbody>();
            if(!enemyRigidBody)
            {
                continue;
            }
            enemyRigidBody.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            EnemyHealth enemyHealth = enemyRigidBody.GetComponent<EnemyHealth>();
            enemyHealth.TakeDamage(damage);    
        }
        explosionParticles.transform.parent = null;

        // Play the particle system.
        explosionParticles.Play();
        explosionAudio.enabled = true;
        // Play the explosion sound effect.
        explosionAudio.Play();

        // Once the particles have finished, destroy the gameobject they are on.
        Destroy(explosionParticles.gameObject, explosionParticles.main.duration);
        Destroy(gameObject);
    }
}
