using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainController : MonoBehaviour {
    private void OnCollisionEnter(Collision col)
    {
        if(col.transform.tag == "Asteroid")
        {
            if(col.gameObject.GetComponent<AsteridController>() != null)
            col.gameObject.GetComponent<AsteridController>().AsteroidDestroy();
        }
    }
}
