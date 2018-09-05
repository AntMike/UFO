using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ControllerBase : MonoBehaviour {


    [Header("Player HP")]
    public float MaxHP = 3;
    public float HealthPoint;


    [Space]
    //particles
    public ParticleSystem CrashParticle;
    public ParticleSystem DamageParticle;


    //components
    protected Transform _myTransform;

    //get components
    protected virtual void Awake()
    {
        HealthPoint = MaxHP;
        _myTransform = GetComponent<Transform>();
    }

    /// <summary>
    /// Calculate damage
    /// </summary>
    public void TakeDamage(float damage)
    {
        HealthPoint -= damage;
        DamageParticle.Play();
        if (tag == "Player")
        {
            UIController.Instance.ShowPlayerHealth(HealthPoint, MaxHP);
        }

        if (HealthPoint <= 0)
        {
            DestroyAnimation(true);
        }
    }


    /// <summary>
    /// Destroy animation 
    /// </summary>
    public abstract void DestroyAnimation(bool wasKilledBy);

}
