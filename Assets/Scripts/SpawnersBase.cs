using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnersBase : MonoBehaviour {
    /// <summary>
    /// List of pool
    /// </summary>
    public List<GameObject> SpawnObjects;

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
    public abstract void EnableObject(GameObject objectToSpawn);
}
