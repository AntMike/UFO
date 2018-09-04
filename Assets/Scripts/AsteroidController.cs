using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : ControllerBase
{

    public ParticleSystem SpawnParticle;
    public float Damage = 1;
    private CapsuleCollider _collider;
    private MeshRenderer _renderer;
    public Transform Player;
    public bool IsDead = false;

    private new void Awake()
    {
        base.Awake();
        _collider = GetComponent<CapsuleCollider>();
        _renderer = GetComponent<MeshRenderer>();
    }

    private void OnEnable()
    {
        IsDead = false;
        StartCoroutine(EnableAsteroid());
    }

    /// <summary>
    /// Destroy Asteroid
    /// </summary>
    public override void DestroyAnimation(bool wasKilledBy)
    {
        if (!IsDead)
        {
            IsDead = true;
            UIController.instance.AddScore(1);
            StartCoroutine(DestroyAsteroid());
        }
    }

    private float _waitForFullParticle = 1.8f;
    private float _waitForEndParticle = 1.8f;
    private IEnumerator DestroyAsteroid()
    {

        //disable collider
        _collider.enabled = false;

        //enable particle
        CrashParticle.Play();


        yield return new WaitForSeconds(_waitForFullParticle);

        //disable asteroid
        _renderer.enabled = false;
        yield return new WaitForSeconds(_waitForEndParticle);

        //disable object and change object to start state
        gameObject.SetActive(false);


        yield return new WaitForEndOfFrame();
    }

    private Vector3 _minScale = new Vector3(0.1f, 0.1f, 0.1f);
    private IEnumerator EnableAsteroid()
    {
        _renderer.enabled = true;
        _collider.enabled = true;
        _myTransform.localScale = _minScale;
        while (_myTransform.localScale.x < 1)
        {
            _myTransform.localScale = new Vector3(_myTransform.localScale.x + Time.deltaTime, _myTransform.localScale.y + Time.deltaTime, _myTransform.localScale.z + Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
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
                    DestroyAnimation(false);
                }
            }
        }
    }

    /// <summary>
    /// Check collision with player
    /// </summary>
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<HealthScript>().TakeDamage(Damage);
            _myHealth.TakeDamage(_myHealth.MaxHP);
        }
    }
}
