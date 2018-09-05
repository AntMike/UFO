using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolBase : MonoBehaviour {

    /// <summary>
    /// List of pool
    /// </summary>
    public List<GameObject> SpawnObjects;



    public virtual void SetObjectToPool(GameObject objectToPool)
    {
        objectToPool.SetActive(false);
        SpawnObjects.Add(objectToPool);
    }

    public virtual GameObject GetObjectFromPool()
    {
        if (SpawnObjects.Count > 0)
        {
            GameObject _objectFromPool = SpawnObjects[0];
            SpawnObjects.Remove(_objectFromPool);
            return _objectFromPool;
        }
        else
            return null;
    }
}
