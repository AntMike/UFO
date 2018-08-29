using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedZoneSpawner : SpawnersBase {


    

    private void OnEnable()
    {
        TimeToSpawn = GiveRandomTime(MinTime, MaxTime);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        TimeToSpawn -= Time.deltaTime;
        if (TimeToSpawn <= 0)
        {
            if (SpawnObjects.Count > 0)
            {
                    SpawnedObject = SpawnObjects[Random.Range(0, SpawnObjects.Count - 1)];
                    if (!SpawnedObject.activeSelf)
                    {
                        EnableObject(SpawnedObject);
                        TimeToSpawn = GiveRandomTime(MinTime, MaxTime);
                    }
                
            }
            else
            {
                return;
            }
        }
    }

    public override void EnableObject(GameObject _object)
    {
        _object.transform.position = new Vector3(SpawnPoint.position.x + Random.Range(MinRadius.x, MaxRadius.x), SpawnPoint.position.y, SpawnPoint.position.z + Random.Range(MinRadius.z, MaxRadius.z));
        _object.SetActive(true);
    }
}
