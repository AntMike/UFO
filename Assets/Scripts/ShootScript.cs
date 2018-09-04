using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootScript : SpawnersBase  {


    private int _bulletDamage = 1;
    private float _bulletSpeed = 55;

    public override void EnableObject(GameObject objectToSpawn)
    {
        objectToSpawn.transform.eulerAngles = new Vector3(SpawnPoint.rotation.eulerAngles.x, SpawnPoint.rotation.eulerAngles.y, SpawnPoint.rotation.eulerAngles.z);
        objectToSpawn.GetComponent<Bullet>().Damage = _bulletDamage;
        objectToSpawn.GetComponent<Bullet>().Speed = _bulletSpeed;
        base.EnableObject(objectToSpawn);
    }

}
