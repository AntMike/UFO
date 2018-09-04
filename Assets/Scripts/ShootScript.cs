using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootScript : SpawnersBase  {


    public override void EnableObject(GameObject objectToSpawn)
    {
        objectToSpawn.transform.eulerAngles = new Vector3(SpawnPoint.rotation.eulerAngles.x, SpawnPoint.rotation.eulerAngles.y, SpawnPoint.rotation.eulerAngles.z);
        base.EnableObject(objectToSpawn);
    }

}
