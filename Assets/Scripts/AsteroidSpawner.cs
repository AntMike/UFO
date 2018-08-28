using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour {


    public List<GameObject> Asteroids;
    public Transform SpawnPoint;
    public float Radius;
    public float MinTime;
    public float MaxTime;

    private float _timeToSpawn;
    private float _spawnRadius;
    private GameObject _currentAsteroid;

    private void OnEnable()
    {
        _timeToSpawn = GiveRandomTime();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        _timeToSpawn -= Time.deltaTime;
        if (_timeToSpawn <= 0)
        {
            if (Asteroids.Count > 0)
            {
                while(true)
                {
                    _currentAsteroid = Asteroids[Random.Range(0, Asteroids.Count - 1)];
                    if(!_currentAsteroid.activeSelf)
                    {
                        EnableAsteroid(_currentAsteroid);
                        _timeToSpawn = GiveRandomTime();
                        break;
                    }
                }
            }
            else
            {
                return;
            }
        }
    }


    private float GiveRandomTime()
    {
        return Random.Range(MinTime, MaxTime);
    }

    private void EnableAsteroid(GameObject _asteroid)
    {
        _asteroid.transform.position = new Vector3(SpawnPoint.position.x + Random.Range(-Radius, Radius), SpawnPoint.position.y, SpawnPoint.position.z + Random.Range(-Radius, Radius));
        _asteroid.GetComponent<Rigidbody>().velocity = Vector3.zero;
        _asteroid.SetActive(true);
    }
}
