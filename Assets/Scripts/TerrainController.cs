using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainController : MonoBehaviour {
    private void OnCollisionEnter(Collision col)
    {
        if(col.transform.tag == "Asteroid")
        {
            col.gameObject.SetActive(false);
        }
    }
}
