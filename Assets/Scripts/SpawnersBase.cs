using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnersBase : MonoBehaviour {
    public List<GameObject> SpawnObjects;
    public Transform SpawnPoint;
    public Vector3 MinRadius;
    public Vector3 MaxRadius;
    public float MinTime;
    public float MaxTime;

    protected float TimeToSpawn;
    protected GameObject SpawnedObject;


    protected float GiveRandomTime(float _min, float _max)
    {
        return Random.Range(_min, _max);
    }

    public abstract void EnableObject(GameObject _object);
}
