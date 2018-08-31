using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour {

    public float Damage = 1;
    public ParticleSystem DamageParticle;
    private ParticleSystem _explosion;
    private CapsuleCollider _collider;
    private MeshRenderer _renderer;
    private HealthScript _myHealth;
    public Transform Player;
    private Transform _myTransform;


    private void Awake()
    {
        _explosion = GetComponent<ParticleSystem>();
        _collider = GetComponent<CapsuleCollider>();
        _renderer = GetComponent<MeshRenderer>();
        _myHealth = GetComponent<HealthScript>();
        _myTransform = GetComponent<Transform>();
    }

    /// <summary>
    /// Destroy Asteroid
    /// </summary>
    public void AsteroidDestroy()
    {
        StartCoroutine(DestroyAsteroid());
    }

    private float _waitForFullParticle = 1.8f;
    private float _waitForEndParticle = 1.8f;
    private IEnumerator DestroyAsteroid()
    {
        //enable particle
        _explosion.Play();

        //disable collider
        _collider.enabled = false;
        yield return new WaitForSeconds(_waitForFullParticle);

        //disable asteroid
        _renderer.enabled = false;
        yield return new WaitForSeconds(_waitForEndParticle);

        //disable object and change object to start state
        gameObject.SetActive(false);
        _renderer.enabled = true;
        _collider.enabled = true;
        yield return new WaitForEndOfFrame();
    }


    private void Update()
    {
        if (tag == "AsteroidFromBelt")
        {
            if (gameObject.activeSelf)
            {
                if (Vector3.Distance(Player.position, _myTransform.position) > 50)
                {
                    _myHealth.TakeDamage(_myHealth.MaxHP);
                }
            }
        }
    }

    /// <summary>
    /// Check collision with player
    /// </summary>
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<HealthScript>().TakeDamage(Damage);
            _myHealth.TakeDamage(_myHealth.MaxHP);
        }
    }
}
