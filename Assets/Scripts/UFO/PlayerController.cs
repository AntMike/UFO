using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Player Components")]
    public List<ParticleSystem> UFOParticle;
    public ParticleSystem CrashParticle;
    public ParticleSystem DamageParticle;
    public Transform UFOChild;
    public Transform UFOModel;
    private Transform UFOTrans;
    private Rigidbody UFORgbd;
    private HealthScript _health;

    //angle to tangage UFO
    private float _minTangageAngle = -15;
    private float _maxTangageAngle = 15;
    //time before UFO starts faling down
    private float _timeBeforeFalling = 3;
    private float _maxTimeBeforeFalling = 3;

    [Header("Move parametrs")]
    public float MoveAcceleration;
    public float MaxVerticalSpeed = 25;
    public float MaxHorizontalSpeed = 15;
    private Vector2 _moveForce;

    [Space]
    public UIController UIControll;
    public float MouseSensitivity = 2;
    private float _mouseSensativeX = 0.0f;
    private float _mouseSensativeY = 0.0f;

   
    private void Awake()
    {

        //disable cursor in play mode
        Cursor.visible = false;
        _health = GetComponent<HealthScript>();
        //set rigidbody abd transform
        UFORgbd = GetComponent<Rigidbody>();
        UFOTrans = transform;
    }

    private void Update()
    {
        if (!_health.IsPlayerDead)
        {
            //check if we need to falling down
            WaitForIdle();

            //calculate force to move
            MoveVertical();
            MoveHorizontal();

            //calculate rotating
            RotateUFOInMove();
            RotateUFOAroundHimself();

            //camera rotating
            MovePlayerToMouse();

            //activating particles
            ParticleControll();

            //player moves
            PlayerMove();
        }
    }




    /// <summary>
    /// Move player
    /// </summary>
    private void PlayerMove()
    {
        UFOTrans.Translate(new Vector3(_moveForce.x * Time.deltaTime, 0, _moveForce.y * Time.deltaTime));
    }

    /// <summary>
    /// Calculate mouse position and rotate player to mouse
    /// </summary>
    private void MovePlayerToMouse()
    {

        _mouseSensativeX += Input.GetAxis("Mouse X") * MouseSensitivity;
        _mouseSensativeY -= Input.GetAxis("Mouse Y") * MouseSensitivity;

        _mouseSensativeY = ClampAngle(_mouseSensativeY);

        // Set rotation
        Quaternion rotation = Quaternion.Euler(_mouseSensativeY, _mouseSensativeX, 0);
        UFOTrans.rotation = rotation;
    }

    /// <summary>
    /// Keep euler angle between -360 and 360 degree
    /// </summary>
    /// <param name="angle"> Angle that we need to check and fix</param>
    /// <returns>That angle with fix</returns>
    private float _clampY = -60;
    private float _clampZ = 80;
    private float ClampAngle(float angle)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;

        return Mathf.Clamp(angle, _clampY, _clampZ);
    }

    /// <summary>
    /// Calculate vertical movement force
    /// </summary>
    private float _axisDeadZone = 0.01f;
    private float _maxAxis = 1;
    private float _accelerationDumping = 2;
    private void MoveVertical()
    {
        if (Mathf.Abs(_moveForce.y) < MaxVerticalSpeed)
        {
            if (Mathf.Abs(Input.GetAxis("Vertical")) > _axisDeadZone)
            {
                if (Mathf.Abs(_moveForce.y + Input.GetAxis("Vertical")) < MaxVerticalSpeed)
                    _moveForce.y += Input.GetAxis("Vertical");
            }
            else
            {
                if (_moveForce.y > _maxAxis)
                {
                    _moveForce.y -= MoveAcceleration / _accelerationDumping * Time.deltaTime;
                }
                else if (_moveForce.y < -_maxAxis)
                {
                    _moveForce.y += MoveAcceleration / _accelerationDumping * Time.deltaTime;
                }
                else
                {
                    _moveForce.y = 0;
                }
            }
        }

    }

    /// <summary>
    /// Calculate horizontal movement force
    /// </summary>
    private void MoveHorizontal()
    {

        if (Mathf.Abs(_moveForce.x) < MaxHorizontalSpeed)
        {
            if (Input.GetAxis("Horizontal") != 0)
            {

                if (Mathf.Abs(_moveForce.x + Input.GetAxis("Horizontal")) < MaxHorizontalSpeed)
                    _moveForce.x += Input.GetAxis("Horizontal");
            }
            else
            {
                if (_moveForce.x > _accelerationDumping)
                {
                    _moveForce.x -= MoveAcceleration / _accelerationDumping * Time.deltaTime;
                }
                else if (_moveForce.x < -1)
                {
                    _moveForce.x += MoveAcceleration / _accelerationDumping * Time.deltaTime;
                }
                else
                {
                    _moveForce.x = 0;
                }
            }
        }
    }

    /// <summary>
    /// Check idle time to falling down the UFO
    /// </summary>
    private void WaitForIdle()
    {
        if (!Input.anyKey)
        {
            _timeBeforeFalling -= Time.deltaTime;
            if (_timeBeforeFalling <= 0)
            {
                UFORgbd.useGravity = true;
            }
        }
        else
        {
            if (_timeBeforeFalling != _maxTimeBeforeFalling)
            {
                _timeBeforeFalling = _maxTimeBeforeFalling;
                UFORgbd.useGravity = false;
            }
        }
    }

    /// <summary>
    /// Calculate tangage
    /// </summary>
    private void RotateUFOInMove()
    {
        float angH = Mathf.Lerp(_minTangageAngle, _maxTangageAngle, Mathf.InverseLerp(-MaxHorizontalSpeed, MaxHorizontalSpeed, _moveForce.x));
        float angV = Mathf.Lerp(_minTangageAngle, _maxTangageAngle, Mathf.InverseLerp(-MaxVerticalSpeed, MaxVerticalSpeed, _moveForce.y));
        UFOChild.localEulerAngles = new Vector3(angV, 0, -angH);
    }

    /// <summary>
    /// Rotating UFO around himself in move and idle
    /// </summary>
    private float _minRotateSpeed = 2;
    private float _forceSpeedMultiply = 20;
    private void RotateUFOAroundHimself()
    {

        UFOModel.Rotate(0, _minRotateSpeed + Mathf.Abs(_moveForce.y) / _forceSpeedMultiply + Mathf.Abs(_moveForce.x) / _forceSpeedMultiply, 0);
    }

    /// <summary>
    /// Set particle speed to UFO speed and turn off then in idle
    /// </summary>
    private float _forceMultiply;
    private void ParticleControll()
    {
        foreach (ParticleSystem _particle in UFOParticle)
        {
            if (_moveForce.y > 0)
                _particle.startSpeed = _moveForce.y * _forceMultiply;
            if (_moveForce.y <= 0)
            {
                _particle.Clear();
                _particle.Pause();
            }
            else
            {
                if (_moveForce.y > 0)
                {
                    _particle.Play();
                }
            }
        }
    }
}
