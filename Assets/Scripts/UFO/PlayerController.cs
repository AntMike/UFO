using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Player Components")]

    public List<ParticleSystem> UFOParticle;
    public Transform UFOChild;
    public Transform UFOModel;
    private Transform UFOTrans;
    private Rigidbody UFORgbd;

    //angle to tangage UFO
    private float _minTangageAngle = -15;
    private float _maxTangageAngle = 15;
    //time before UFO starts faling down
    private float _timeBeforeFalling = 3;

    [Header("Move parametrs")]
    public float MoveAcceleration;
    public float MaxVerticalSpeed = 25;
    public float MaxHorizontalSpeed = 15;
    private Vector2 _moveForce;

    [Header("Player HP")]
    public Image HealthBar;
    public float MaxHP = 3;
    private float _healthPoint;

    [Space]
    public float MouseSensitivity = 2;
    private float _mouseSensativeX = 0.0f;
    private float _mouseSensativeY = 0.0f;

   
    private void Awake()
    {

        _healthPoint = MaxHP;

        //disable cursor in play mode
        Cursor.visible = false;
        //set rigidbody abd transform
        UFORgbd = GetComponent<Rigidbody>();
        UFOTrans = transform;
    }

    private void FixedUpdate()
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


    /// <summary>
    /// Check collision with red zone
    /// </summary>
    private void OnCollisionEnter(Collision col)
    {
        if(col.transform.tag == "Asteroid")
        {
            TakeDamage(1);
            col.gameObject.GetComponent<AsteridController>().AsteroidDestroy();
        }
    }

    /// <summary>
    /// Calculate damage
    /// </summary>
    public void TakeDamage(float _damage)
    {
        _healthPoint -= _damage;
        HealthBar.fillAmount = (_healthPoint / MaxHP);
    }


    /// <summary>
    /// Move player
    /// </summary>
    private void PlayerMove()
    {
        transform.Translate(new Vector3(_moveForce.x * Time.deltaTime, 0, _moveForce.y * Time.deltaTime));
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
    private float ClampAngle(float angle)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;

        return Mathf.Clamp(angle, -60, 80);
    }
    
    /// <summary>
    /// Calculate vertical movement force
    /// </summary>
    private void MoveVertical()
    {
        if (Mathf.Abs(_moveForce.y) < MaxVerticalSpeed)
        {
            if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.01f)
            {
                if (Mathf.Abs(_moveForce.y + Input.GetAxis("Vertical")) < MaxVerticalSpeed)
                    _moveForce.y += Input.GetAxis("Vertical");
            }
            else
            {
                if (_moveForce.y > 1)
                {
                    _moveForce.y -= MoveAcceleration / 2 * Time.deltaTime;
                }
                else if (_moveForce.y < -1)
                {
                    _moveForce.y += MoveAcceleration / 2 * Time.deltaTime;
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
                if (_moveForce.x > 1)
                {
                    _moveForce.x -= MoveAcceleration / 2 * Time.deltaTime;
                }
                else if (_moveForce.x < -1)
                {
                    _moveForce.x += MoveAcceleration / 2 * Time.deltaTime;
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
            if (_timeBeforeFalling != 3)
            {
                _timeBeforeFalling = 3;
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
    private void RotateUFOAroundHimself()
    {

        UFOModel.Rotate(0, 2 + Mathf.Abs(_moveForce.y) / 20 + Mathf.Abs(_moveForce.x) / 20, 0);
    }
    
    /// <summary>
    /// Set particle speed to UFO speed and turn off then in idle
    /// </summary>
    private void ParticleControll()
    {
        foreach (ParticleSystem _particle in UFOParticle)
        {
            if (_moveForce.y > 0)
                _particle.startSpeed = _moveForce.y * 2;
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
