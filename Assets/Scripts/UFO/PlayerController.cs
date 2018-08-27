using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float speed;
    public float turnspeed;
    public Transform child;
    public float minAngle;
    public float maxAngle;

    public float maxVerticalSpeed = 25;
    public float maxHorizontalSpeed = 15;
    Rigidbody rbody;

    private Transform playerTransform;
    private Vector3 relative;


    // Use this for initialization
    void Start()
    {
        rbody = GetComponent<Rigidbody>();
        playerTransform = transform;
    }

    Vector3 nextVerticalVelocity;
    Vector3 nextHorizontalVelocity;
    void FixedUpdate()
    {

        child.Rotate(0, 2 + Mathf.Abs(rbody.velocity.z) / 20 + Mathf.Abs(rbody.velocity.x) / 20, 0);

        MoveVertical();
        MoveHorizontal();
        RotateUFO();

        Debug.Log(rbody.velocity);

    }

    private void MoveVertical()
    {
        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.001f)
        {
            nextVerticalVelocity = rbody.velocity + (Vector3.forward * turnspeed * Input.GetAxis("Vertical") * Time.deltaTime);
            if (Mathf.Abs(nextVerticalVelocity.z) < maxVerticalSpeed)
                rbody.velocity = nextVerticalVelocity;
        }
        else
        {
            if (rbody.velocity.z < -1)
            {
                rbody.velocity += Vector3.forward * speed / 2 * Time.deltaTime;
            }
            else
            {
                if (rbody.velocity.z > 1)
                {
                    rbody.velocity += Vector3.back * speed / 2 * Time.deltaTime;
                }
                else
                {
                    rbody.velocity = new Vector3(rbody.velocity.x, rbody.velocity.y, 0);
                }
            }
        }

    }

    private void MoveHorizontal()
    {
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.001f)
        {
            nextHorizontalVelocity = rbody.velocity + (Vector3.right * turnspeed * Input.GetAxis("Horizontal") * Time.deltaTime);
            if (Mathf.Abs(nextHorizontalVelocity.x) < maxHorizontalSpeed)
                rbody.velocity = nextHorizontalVelocity;
        }
        else
        {
            if (rbody.velocity.x < -1)
            {
                rbody.velocity += Vector3.right * turnspeed / 2 * Time.deltaTime;
            }
            else
            {
                if (rbody.velocity.x > 1)
                {
                    rbody.velocity += Vector3.left * turnspeed / 2 * Time.deltaTime;
                }
                else
                {
                    rbody.velocity = new Vector3(0, rbody.velocity.y, rbody.velocity.z);
                }
            }
        }
    }

    private void RotateUFO()
    {

            float angH = Mathf.Lerp(minAngle, maxAngle, Mathf.InverseLerp(-maxHorizontalSpeed, maxHorizontalSpeed, rbody.velocity.x));
        float angV = Mathf.Lerp(minAngle, maxAngle, Mathf.InverseLerp(-maxVerticalSpeed, maxVerticalSpeed, rbody.velocity.z));
                playerTransform.eulerAngles = new Vector3(angV, 0, -angH);



    }
}
