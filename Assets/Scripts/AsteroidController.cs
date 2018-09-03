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

        //disable collider
        _collider.enabled = false;

        //enable particle
        _explosion.Play();


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
        DistanceToPlayer();
    }


    private int _maxDistanceToPlayer = 50;
    /// <summary>
    /// Check distanse to player and destroy if distance more then max
    /// </summary>
    private void DistanceToPlayer()
    {
        if (tag == "AsteroidFromBelt")
        {
            if (gameObject.activeSelf)
            {
                if (Vector3.Distance(Player.position, _myTransform.position) > _maxDistanceToPlayer)
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

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<HealthScript>().TakeDamage(Damage);
            _myHealth.TakeDamage(_myHealth.MaxHP);
        }
    }
}
