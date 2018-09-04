using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : PoolBase
{
    public static BulletPool Instance;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

}
