using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainController : MonoBehaviour {

    //Disable Asteroids when they collision with ground
    private void OnTriggerEnter(Collider col)
    {

            if(col.gameObject.GetComponent<ControllerBase>() != null)
            col.gameObject.GetComponent<ControllerBase>().DestroyAnimation(false);
        
    }
}
