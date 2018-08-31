using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform LookAtTarget;
    public Transform FollowTarget;

    // Dumping speed in units/sec.
    public float LookSpeed = 1.0F;
    public float FollowSpeed = 2.0F;


    private Quaternion _targetRot;
    private float _maxLookAtAngle;
    private float _minLookAtSpeed;

    private Transform _cameraTransform;

    private int _lookSpeedMultiply = 5;

    private void Awake()
    {
        _cameraTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        LookAtControll();
        FollowControll();
    }

    /// <summary>
    /// Look at object with lerp
    /// </summary>
    private void LookAtControll()
    {
        if (LookAtTarget != null)
        {
            _targetRot = Quaternion.LookRotation(LookAtTarget.position - _cameraTransform.position);
            if (Quaternion.Angle(_targetRot, _cameraTransform.rotation) < _maxLookAtAngle)
            {
                if (LookSpeed != _minLookAtSpeed)
                    LookSpeed = _minLookAtSpeed;
            }
            else
            {
                LookSpeed = (Quaternion.Angle(_targetRot, _cameraTransform.rotation)) / _lookSpeedMultiply;
            }

            _cameraTransform.rotation = Quaternion.Slerp(_cameraTransform.rotation, _targetRot, LookSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Follow the object with lerp
    /// </summary>
    private void FollowControll()
    {
        if (FollowTarget != null)
        {
            _cameraTransform.position = Vector3.Slerp(_cameraTransform.position, FollowTarget.position, FollowSpeed * Time.deltaTime);
        }
    }



}
