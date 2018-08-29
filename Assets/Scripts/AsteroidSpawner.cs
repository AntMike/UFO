using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : SpawnersBase {

    public bool IsAsteroidBelt = false;
    private float _timeToLive;
    private float _minLiveTime = 15;
    private float _maxLiveTime = 20;

    //Reset all timers
    private void OnEnable()
    {
        TimeToSpawn = GiveRandomTime(MinTime,MaxTime);
        _timeToLive = GiveRandomTime(_minLiveTime, _maxLiveTime);
    }

    //Disable all asteroid in the end
    private void OnDisable()
    {
        TurnOffAllAsteroids();
    }

    private void FixedUpdate()
    {
        CalculateAsteroidSpawn();
    }

    /// <summary>
    /// Calculate time and object to spawn
    /// </summary>
    private void CalculateAsteroidSpawn()
    {
        //check if its asteroid belt
        //if no - count time to disable red zone
        if (!IsAsteroidBelt)
            _timeToLive -= Time.deltaTime;
        //count time to spawn new asteroid
        TimeToSpawn -= Time.deltaTime;
        if (TimeToSpawn <= 0 && _timeToLive > 0)
        {
            //chack if we have objects to spawn
            if (SpawnObjects.Count > 0)
            {
                //choose random object from list
                SpawnedObject = SpawnObjects[Random.Range(0, SpawnObjects.Count - 1)];
                //check if object is active now
                if (!SpawnedObject.activeSelf)
                {
                    ObjectSpawn();
                }
                else
                {
                    //if previous object was activ take first unactive object from list
                    foreach (GameObject _object in SpawnObjects)
                    {
                        if (!_object.activeSelf)
                        {
                            SpawnedObject = _object;
                            ObjectSpawn();
                            break;
                        }
                    }
                }

            }
        }
    }

    /// <summary>
    /// Disable red zone
    /// </summary>
    private void DisableRedZone()
    {
        if (_timeToLive < -_minLiveTime && gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Turn off all asteroids from list
    /// </summary>
    private void TurnOffAllAsteroids()
    {
        foreach (GameObject _object in SpawnObjects)
        {
            _object.SetActive(false);
        }
    }

    /// <summary>
    /// Enable spawned object
    /// </summary>
    /// <param name="_object">object to enable</param>
    public override void EnableObject(GameObject _object)
    {
        //set position
        _object.transform.position = new Vector3(SpawnPoint.position.x + Random.Range(MinRadius.x, MaxRadius.x), SpawnPoint.position.y, SpawnPoint.position.z + Random.Range(MinRadius.z, MaxRadius.z));
        //reset velocity
        _object.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //enable
        _object.SetActive(true);
    }
}
