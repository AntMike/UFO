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

            SpawnedObject = AsteroidPool.Instance.GetObjectFromPool();
            if(SpawnedObject != null)
            {
                ObjectSpawn();
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
            RedZonePool.Instance.SetObjectToPool(gameObject);
        }
    }

    /// <summary>
    /// Enable spawned object
    /// </summary>
    /// <param name="objectToEnable">object to enable</param>
    public override void EnableObject(GameObject objectToEnable)
    {
        //reset rigidbody
        objectToEnable.GetComponent<Rigidbody>().velocity = Vector3.zero;
        objectToEnable.GetComponent<Rigidbody>().useGravity = !IsAsteroidBelt;
        
        base.EnableObject(objectToEnable);
        
    }
}
