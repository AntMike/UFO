using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootScript : SpawnersBase  {

    public float ReloadTime = 0.3f;
    private float _reloading;

    private void Awake()
    {
        _reloading = ReloadTime;
    }

    private void Update()
    {
        if(_reloading >= 0)
        _reloading -= Time.deltaTime;
        if(Input.GetMouseButton(0))
        {
            if(_reloading <= 0)
            {
                foreach(GameObject _bullet in SpawnObjects)
                {
                    if(!_bullet.activeSelf)
                    {
                        EnableObject(_bullet);
                        _reloading = ReloadTime;
                        break;
                    }
                }
            }
        }
    }


    public override void EnableObject(GameObject objectToSpawn)
    {
        //set position
        objectToSpawn.transform.position = new Vector3(SpawnPoint.position.x, SpawnPoint.position.y, SpawnPoint.position.z);
        objectToSpawn.transform.rotation = SpawnPoint.rotation;
        //enable
        objectToSpawn.SetActive(true);
    }

}
