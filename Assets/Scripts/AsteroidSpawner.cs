using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : SpawnersBase {

    public bool IsAsteroidBelt = false;
    private float _timeToLive;
    private float _minLiveTime = 15;
    private float _maxLiveTime = 20;

    private void OnEnable()
    {
        TimeToSpawn = GiveRandomTime(MinTime,MaxTime);
        _timeToLive = GiveRandomTime(_minLiveTime, _maxLiveTime);
    }

    private void OnDisable()
    {
        TurnOffAllAsteroids();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(!IsAsteroidBelt)
        _timeToLive -= Time.deltaTime;
        TimeToSpawn -= Time.deltaTime;
        if (TimeToSpawn <= 0 && _timeToLive > 0)
        {
            if (SpawnObjects.Count > 0)
            {

                    SpawnedObject = SpawnObjects[Random.Range(0, SpawnObjects.Count - 1)];
                    if(!SpawnedObject.activeSelf)
                    {
                        EnableObject(SpawnedObject);
                        TimeToSpawn = GiveRandomTime(MinTime, MaxTime);
                    } else
                {
                    foreach(GameObject _object in SpawnObjects)
                    {
                        if (!_object.activeSelf)
                        {
                            SpawnedObject = _object;
                            EnableObject(SpawnedObject);
                            TimeToSpawn = GiveRandomTime(MinTime, MaxTime);
                            break;
                        }
                    }
                }

            }
            else
            {
                return;
            }
        }
        if (_timeToLive < -_minLiveTime && gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

    private void TurnOffAllAsteroids()
    {
        foreach (GameObject _object in SpawnObjects)
        {
            _object.SetActive(false);
        }
    }

    public override void EnableObject(GameObject _object)
    {
        _object.transform.position = new Vector3(SpawnPoint.position.x + Random.Range(MinRadius.x, MaxRadius.x), SpawnPoint.position.y, SpawnPoint.position.z + Random.Range(MinRadius.z, MaxRadius.z));
        _object.GetComponent<Rigidbody>().velocity = Vector3.zero;
        _object.SetActive(true);
    }
}
