using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainController : MonoBehaviour {

    //Disable Asteroids when they collision with ground
    private void OnCollisionEnter(Collision col)
    {
        if(col.transform.tag == "Asteroid" )
        {
            if(col.gameObject.GetComponent<AsteroidController>() != null)
            col.gameObject.GetComponent<AsteroidController>().AsteroidDestroy();
        }
    }
}
