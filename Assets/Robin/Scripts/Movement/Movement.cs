using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody rb;

    [Header("Transforms")]
    public Transform origin;
    public Transform cam;
    public Transform camOffset;
    public Transform dotPos;

    [Header("Wheelchair")]
    public WheelCollider colliderLeft;
    public WheelCollider colliderRight;
    public Transform wheelLeft;
    public Transform wheelRight;

    [Header("Variables")]
    public float currentSpeed;
    public float maxSpeed;

    //[DoNotSerialize] 
    public float dotL, dotR;
    //[DoNotSerialize]
    public float heightL, heightR;

    public float grL, grR;
    public float piL, piR;

    bool wbL, wbR;
    bool wfL, wfR;

    public float downForce;
    public float acceleration;
    public float breakingForce;

    float currentAccelerationL;
    float currentAccelerationR;
    float currentBreakingForceL;
    float currentBreakingForceR;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, camOffset.position.y - camOffset.position.y, transform.position.z);
        ControllWheel();
    }

    void FixedUpdate()
    {
        Wheeling();
    }

    void ControllWheel()
    {
        //Left
        if(heightL < 0)
        {
            //Acceleration Left side
            if(grL == 1 && piL == 0)
            {
                //Checking which direction
                if(dotL < 0 && !wbL && !wfL)
                {
                    wbL = true;
                }
                else if(dotL > 0 && !wfL && !wbL)
                {
                    wfL = true;
                }

                //Move in Light direction
                if(wbL && dotL > 0 && !wfL)
                {
                    currentAccelerationL = acceleration;
                    wbL = false;
                }
                else if(wfL && dotL < 0 && !wbL)
                {
                    currentAccelerationL = -acceleration;
                    wfL = false;
                }
            }
            else
            {
                currentAccelerationL = 0;

                wbL = false;
                wfL = false;
            }

            //Breaking Left
            if(grL == 1 && piL == 1)
            {
                currentBreakingForceL = breakingForce;
            }
            else
            {
                currentBreakingForceL = 0;
            }
        }

        //Right
        if(heightR < 0)
        {
            //Acceleration Right Side
            if(grR == 1 && piR == 0)
            {
                //Checking which direction
                if(dotR < 0 && !wbR && !wfR)
                {
                    wbR = true;
                }
                else if(dotR > 0 && !wfR && !wbR)
                {
                    wfR = true;
                }

                //Move in Right direction
                if(wbR && dotR > 0 && !wfR)
                {
                    currentAccelerationR = acceleration;
                    wbR = false;
                }
                else if(wfR && dotR < 0 && !wbR)
                {
                    currentAccelerationR = -acceleration;
                    wfR = false;
                }
            }
            else
            {
                currentAccelerationR = 0;

                wbR = false;
                wfR = false;
            }

            //Break Right
            if(grR == 1 && piR == 1)
            {
                currentBreakingForceR = breakingForce;
            }
            else
            {
                currentBreakingForceR = 0;
            }
        }
    }

    void Wheeling()
    {
        rb.AddForceAtPosition(downForce * Vector3.down, rb.centerOfMass);

        currentSpeed = rb.velocity.magnitude * 3.6f;

        if(currentSpeed > maxSpeed)
        {
            currentAccelerationL = 0;
            currentAccelerationR = 0;
        }

        colliderLeft.motorTorque = currentAccelerationL;
        colliderRight.motorTorque = currentAccelerationR;

        colliderLeft.brakeTorque = currentBreakingForceL;
        colliderRight.brakeTorque = currentBreakingForceR;

        UpdateWheel(colliderLeft, wheelLeft);
        UpdateWheel(colliderRight, wheelRight);
    }

    void UpdateWheel(WheelCollider col, Transform trans)
    {
        col.GetWorldPose(out Vector3 pos, out Quaternion rot);

        trans.position = pos;
        trans.rotation = rot;
    }

    public(float, float) Dotting(Transform hand)
    {
        var dirToDot = Vector3.Normalize(hand.position - dotPos.position);

        float dot;
        float height;

        dot = Vector3.Dot(dotPos.forward, dirToDot);
        height = Vector3.Dot(dotPos.up, dirToDot);

        return(dot, height);
    }
}
