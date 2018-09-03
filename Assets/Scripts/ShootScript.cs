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
        CalculateReload();
        BulletSpawn();
    }


    /// <summary>
    /// Calculate reload time
    /// </summary>
    private void CalculateReload()
    {
        if (_reloading >= 0)
            _reloading -= Time.deltaTime;
    }

    /// <summary>
    /// Spawn the bullet when left mouse button is down
    /// </summary>
    private void BulletSpawn()
    {
        if (Input.GetMouseButton(0))
        {
            if (_reloading <= 0)
            {
                foreach (GameObject _bullet in SpawnObjects)
                {
                    if (!_bullet.activeSelf)
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
        objectToSpawn.transform.eulerAngles = new Vector3(SpawnPoint.rotation.eulerAngles.x , SpawnPoint.rotation.eulerAngles.y, SpawnPoint.rotation.eulerAngles.z);
        //enable
        objectToSpawn.SetActive(true);
    }

}
