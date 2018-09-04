using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour {

    [Header("Player HP")]
    public float MaxHP = 3;
    public float HealthPoint;
    private ControllerBase _controller;

    private void Awake()
    {
        HealthPoint = MaxHP;

        _controller = GetComponent<ControllerBase>();


    }

    /// <summary>
    /// Calculate damage
    /// </summary>
    public void TakeDamage(float damage)
    {
        HealthPoint -= damage;
        _controller.DamageParticle.Play();
        if (tag == "Player")
        {
            UIController.instance.ShowPlayerHealth(HealthPoint, MaxHP);
        }

        if (HealthPoint <= 0)
        {
            _controller.DestroyAnimation(true);
        }
    }
}
