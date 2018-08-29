﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedZoneSpawner : SpawnersBase {

    private void OnEnable()
    {
        TimeToSpawn = GiveRandomTime(MinTime, MaxTime);
    }

    void FixedUpdate()
    {
        CalculateZoneSpawn();
    }

    /// <summary>
    /// Calculate time and object to spawn
    /// </summary>
    private void CalculateZoneSpawn()
    {
        //count time to spawn new zone
        TimeToSpawn -= Time.deltaTime;
        if (TimeToSpawn <= 0)
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
            }
            else
            {
                return;
            }
        }
    }

    /// <summary>
    /// Enable spawned object
    /// </summary>
    /// <param name="_object">object to enable</param>
    public override void EnableObject(GameObject _object)
    {
        _object.transform.position = new Vector3(SpawnPoint.position.x + Random.Range(MinRadius.x, MaxRadius.x), SpawnPoint.position.y, SpawnPoint.position.z + Random.Range(MinRadius.z, MaxRadius.z));
        _object.SetActive(true);
    }
}
