using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : ControllerBase
{
    
    public float Damage = 1;
    private CapsuleCollider _collider;
    private MeshRenderer _renderer;
    public Transform Player;
    public bool IsNeedTrackPlayer = false;
    public bool IsDead = false;


    private float _waitForFullParticle = 1.8f;
    private float _waitForEndParticle = 1.8f;
    private Vector3 _minScale = new Vector3(0.1f, 0.1f, 0.1f);
    private int _maxDistanceToPlayer = 50;

    protected override void Awake()
    {
        base.Awake();
        _collider = GetComponent<CapsuleCollider>();
        _renderer = GetComponent<MeshRenderer>();
    }

    private void OnEnable()
    {
        IsDead = false;
        HealthPoint = MaxHP;
        StartCoroutine(EnableAsteroid());
    }

    private void Update()
    {
        DistanceToPlayer();
    }

    /// <summary>
    /// Destroy Asteroid
    /// </summary>
    public override void DestroyAnimation(bool wasKilledBy)
    {
        if (!IsDead)
        {
            IsDead = true;
            if(wasKilledBy)
            UIController.Instance.AddScore(1);
            StartCoroutine(DestroyAsteroid());
        }
    }



    /// <summary>
    /// /// Destroy animation
    /// /// </summary>
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
        AsteroidPool.Instance.SetObjectToPool(gameObject);

        yield return new WaitForEndOfFrame();
    }


    /// <summary>
    /// Enable animation
    /// </summary>
    /// <returns></returns>
    private IEnumerator EnableAsteroid()
    {
        //enable all components
        _renderer.enabled = true;
        _collider.enabled = true;

        //scale asteroid
        _myTransform.localScale = _minScale;
        while (_myTransform.localScale.x < 1)
        {
            _myTransform.localScale = new Vector3(_myTransform.localScale.x + Time.deltaTime, _myTransform.localScale.y + Time.deltaTime, _myTransform.localScale.z + Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForEndOfFrame();
    }

    

    /// <summary>
    /// Check distanse to player and destroy if distance more then max
    /// </summary>
    private void DistanceToPlayer()
    {
        if (IsNeedTrackPlayer)
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
            col.gameObject.GetComponent<PlayerController>().TakeDamage(Damage);
            this.TakeDamage(MaxHP);
        }
    }
}
