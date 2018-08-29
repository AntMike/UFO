using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteridController : MonoBehaviour {

    public float Damage = 1;
    private ParticleSystem _explosion;
    private CapsuleCollider _collider;
    private MeshRenderer _renderer;


    private void Awake()
    {
        _explosion = GetComponent<ParticleSystem>();
        _collider = GetComponent<CapsuleCollider>();
        _renderer = GetComponent<MeshRenderer>();
    }

    public void AsteroidDestroy()
    {
        StartCoroutine(DestroyAsteroid());
    }

    private IEnumerator DestroyAsteroid()
    {
        _explosion.Play();
        _collider.enabled = false;
        yield return new WaitForSeconds(1.8f);
        _renderer.enabled = false;
        yield return new WaitForSeconds(0.7f);
        gameObject.SetActive(false);
        _renderer.enabled = true;
        _collider.enabled = true;
        yield return new WaitForEndOfFrame();
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            col.GetComponent<PlayerController>().TakeDamage(Damage);
            AsteroidDestroy();
        }
    }
}
