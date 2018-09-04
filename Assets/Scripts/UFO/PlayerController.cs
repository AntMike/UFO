using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerController : ControllerBase
{
    [Header("Player Components")]
    public List<ParticleSystem> UFOParticle;
    public Transform UFOChild;
    public Transform UFOModel;
    private Rigidbody UFORgbd;

    public bool IsPlayerDead = false;

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
    public float MouseSensitivity = 2;
    private float _mouseSensativeX = 0.0f;
    private float _mouseSensativeY = 0.0f;



    public float ReloadTime = 0.3f;
    private float _reloading;
    private ShootScript _bulletBase;

    private new void Awake()
    {
        base.Awake();
        //disable cursor in play mode
        Cursor.visible = false;
        //set rigidbody abd transform
        UFORgbd = GetComponent<Rigidbody>();
        _reloading = ReloadTime;
        _bulletBase = GetComponent<ShootScript>();
    }

    private void Update()
    {
        if (!IsPlayerDead)
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

            //calculate reload time
            CalculateReload();

            //check if we need to shoot
            CheckShoot();
        }
    }



    /// <summary>
    /// Calculate reloading
    /// </summary>
    private void CalculateReload()
    {
        if (_reloading >= 0)
            _reloading -= Time.deltaTime;
    }


    private GameObject _currentBullet;

    /// <summary>
    /// Spawn the bullet when left mouse button is down
    /// </summary>
    private void CheckShoot()
    {
        if (Input.GetMouseButton(0))
        {
            if (_reloading <= 0)
            {
                _currentBullet = BulletPool.Instance.GetObjectFromPool();
                if(_currentBullet != null)
                {
                    _bulletBase.EnableObject(_currentBullet);
                    _reloading = ReloadTime;
                }
            }
        }
    }




    /// <summary>
    /// Move player
    /// </summary>
    private void PlayerMove()
    {
        _myTransform.Translate(new Vector3(_moveForce.x * Time.deltaTime, 0, _moveForce.y * Time.deltaTime));
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
        _myTransform.rotation = rotation;
    }

    private float _clampY = -60;
    private float _clampZ = 80;

    /// <summary>
    /// Keep euler angle between -360 and 360 degree
    /// </summary>
    /// <param name="angle"> Angle that we need to check and fix</param>
    /// <returns>That angle with fix</returns>
    private float ClampAngle(float angle)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;

        return Mathf.Clamp(angle, _clampY, _clampZ);
    }

    private float _axisDeadZone = 0.01f;
    private float _maxAxis = 1;
    private float _accelerationDumping = 2;

    /// <summary>
    /// Calculate vertical movement force
    /// </summary>
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

    private float _minRotateSpeed = 2;
    private float _forceSpeedMultiply = 20;

    /// <summary>
    /// Rotating UFO around himself in move and idle
    /// </summary>
    private void RotateUFOAroundHimself()
    {

        UFOModel.Rotate(0, _minRotateSpeed + Mathf.Abs(_moveForce.y) / _forceSpeedMultiply + Mathf.Abs(_moveForce.x) / _forceSpeedMultiply, 0);
    }

    private float _forceMultiply = 3;

    /// <summary>
    /// Set particle speed to UFO speed and turn off then in idle
    /// </summary>
    private void ParticleControll()
    {
        foreach (ParticleSystem _particle in UFOParticle)
        {
            if (_moveForce.y > 0)
            {
                _particle.startSpeed = _moveForce.y * _forceMultiply;
            }
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

    public override void DestroyAnimation(bool wasKilledBy)
    {
        IsPlayerDead = true;
        CrashParticle.Play();
        UIController.Instance.PlayerDeath();
    }
}
