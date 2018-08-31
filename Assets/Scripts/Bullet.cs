﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float Speed;
    private Transform _bulletTransform;
    private float _timeToLive;
    private float _maxTimeToLive = 2;
    private int _damage = 1;

    private void Awake()
    {
        _bulletTransform = GetComponent<Transform>();
    }

    //Reset all timers
    private void OnEnable()
    {
        _timeToLive = _maxTimeToLive;
    }


    private void Update()
    {
        if (gameObject.activeSelf)
        {
            _timeToLive -= Time.deltaTime;
            if (_timeToLive > 0)
                BulletMove();
            else
                gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Asteroid" || tag == "AsteroidFromBelt" )
        {
            col.gameObject.GetComponent<HealthScript>().TakeDamage(_damage);
            gameObject.SetActive(false);
        }
    }


    /// <summary>
    /// Move player
    /// </summary>
    private void BulletMove()
    {
        _bulletTransform.Translate(new Vector3(0, 0, Speed * Time.deltaTime));
    }

}