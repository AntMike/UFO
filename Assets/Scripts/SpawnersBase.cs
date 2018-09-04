using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnersBase : MonoBehaviour {

    /// <summary>
    /// Central point to spawn
    /// </summary>
    public Transform SpawnPoint;


    // Radius of spawn
    public Vector3 MinRadius;
    public Vector3 MaxRadius;

    // Spawn time range
    public float MinTime;
    public float MaxTime;

    protected float TimeToSpawn;

    //Current spawned object
    protected GameObject SpawnedObject;


    /// <summary>
    /// Return random number between min and max
    /// </summary>
    protected float GiveRandomTime(float min, float max)
    {
        return Random.Range(min, max);
    }

    /// <summary>
    /// Spawn object and reset timer
    /// </summary>
    protected void ObjectSpawn()
    {
        EnableObject(SpawnedObject);
        TimeToSpawn = GiveRandomTime(MinTime, MaxTime);
    }

    /// <summary>
    /// Enable spawned object
    /// </summary>
    /// <param name="objectToSpawn">object to enable</param>
    public virtual void EnableObject(GameObject objectToEnable)
    {
        objectToEnable.transform.position = new Vector3(SpawnPoint.position.x + Random.Range(MinRadius.x, MaxRadius.x), SpawnPoint.position.y + Random.Range(MinRadius.y, MaxRadius.y), SpawnPoint.position.z + Random.Range(MinRadius.z, MaxRadius.z));
        objectToEnable.SetActive(true);
    }


}
