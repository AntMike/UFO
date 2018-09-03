using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour {

    [Header("Player HP")]
    public float MaxHP = 3;
    [HideInInspector]
    public bool IsPlayerDead = false;
    private float _healthPoint;
    private PlayerController _player;
    private AsteroidController _asteroid;

    private void Awake()
    {
        _healthPoint = MaxHP;
        if (tag == "Player")
        {
            _player = GetComponent<PlayerController>();
        }
        if (tag == "Asteroid" || tag == "AsteroidFromBelt")
        {
            _asteroid = GetComponent<AsteroidController>();
        }

    }

    /// <summary>
    /// Calculate damage
    /// </summary>
    public void TakeDamage(float damage)
    {
        if (tag == "Player")
        {

            _healthPoint -= damage;
            _player.DamageParticle.Play();
            _player.UIControll.ShowPlayerHealth(_healthPoint, MaxHP);
            if (_healthPoint <= 0)
            {
                IsPlayerDead = true;
                _player.CrashParticle.Play();
                _player.UIControll.PlayerDeath();
            }
        }
        if (tag == "Asteroid" || tag == "AsteroidFromBelt")
        {

                _healthPoint -= damage;
                _asteroid.DamageParticle.Play();
            if (_healthPoint <= 0)
            {
                _asteroid.AsteroidDestroy();
            }
        }
    }
}
