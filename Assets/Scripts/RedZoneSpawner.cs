using System.Collections;
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
            SpawnedObject = RedZonePool.Instance.GetObjectFromPool();
            if(SpawnedObject != null)
            {
                ObjectSpawn();
            } else
            {
                TimeToSpawn = GiveRandomTime(MinTime, MaxTime);
            }
        }
    }
}
