using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {


    public Transform _lookAt;
    public Transform _follow;



    /// <summary>Get the LookAt target for the Aim component in the CinemachinePipeline.</summary>
    public Transform LookAt
    {         
            get { return _lookAt; }
            set { _lookAt = value; }
        
    }

    /// <summary>Get the Follow target for the Body component in the CinemachinePipeline.</summary>
    public Transform Follow
    {
        get { return _follow; }
        set { _follow = value; }

    }

    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 desiredPosition = _follow.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(_lookAt);
	}

}
