using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedZonePool : PoolBase
{
    public static RedZonePool Instance;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
}
