using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidPool : PoolBase
{
    public static AsteroidPool Instance;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

}
