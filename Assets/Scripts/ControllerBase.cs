using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ControllerBase : MonoBehaviour {

    //particles
    public ParticleSystem CrashParticle;
    public ParticleSystem DamageParticle;


    //components
    protected Transform _myTransform;
    protected HealthScript _myHealth;

    //get components
    protected void Awake()
    {
        _myHealth = GetComponent<HealthScript>();
        _myTransform = GetComponent<Transform>();
    }

    /// <summary>
    /// Destroy animation 
    /// </summary>
    public abstract void DestroyAnimation(bool wasKilledBy);

}
