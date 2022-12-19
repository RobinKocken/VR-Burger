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
    public float currentAngular;
    public float maxAngular;

    public float dotL, dotR;
    public float heightL, heightR;

    public float grL, grR;
    public float piL, piR;

    bool wbL, wbR;
    bool wfL, wfR;

    public float downForce;
    public float acceleration;
    public float breakingForce;

    public float currentAccelerationL;
    public float currentAccelerationR;
    float currentBreakingForceL;
    float currentBreakingForceR;

    float waitForSec;
    bool[] start = new bool[2];
    float[] startTime = new float[2];

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        origin.position = new Vector3(transform.position.x, origin.position.y, transform.position.z);
        origin.rotation = transform.rotation;

        transform.position = new Vector3(transform.position.x, camOffset.position.y - camOffset.position.y, transform.position.z);
        InputWheel();
    }

    void FixedUpdate()
    {
        Wheeling();
    }

    void InputWheel()
    {
        if(start[0])
        {
            if(Time.time - startTime[0] > 0.5)
            {
                start[0] = false;
                currentAccelerationL = 0;
            }
        }

        if(start[1])
        {
            if(Time.time - startTime[1] > 0.5)
            {
                start[1] = false;
                currentAccelerationR = 0;
            }
        }

        if(heightL < 0)
        {
            if(grL == 1 && piL == 1)
            {
                currentBreakingForceL = breakingForce;
                currentAccelerationL = 0;
            }
            else
            {
                currentBreakingForceL = 0;
            }

            if(dotL < 0)
            {
                if(grL == 1 && piL == 0 && !wbL && !wfL)
                {
                    wbL = true;
                }
                else if(grL == 0 || piL == 1)
                {
                    wbL = false;
                }
            }
            else if(dotL > 0)
            {
                if(grL == 1 && piL == 0 && !wbL && !wfL)
                {
                    wfL = true;
                }
                else if(grL == 0 || piL == 1)
                {
                    wfL = false;
                }
            }
        }

        if(heightR < 0)
        {
            if(grR == 1 && piR == 1)
            {
                currentBreakingForceR = breakingForce;
                currentAccelerationR = 0;
            }
            else
            {
                currentBreakingForceR = 0;
            }

            if(dotR < 0)
            {
                if(grR == 1 && piR == 0 && !wbR && !wfR)
                {
                    wbR = true;
                }
                else if(grR == 0 || piR == 1)
                {
                    wbR = false;
                }
            }
            else if(dotR > 0)
            {
                if(grR == 1 && piR == 0 && !wbR && !wfR)
                {
                    wfR = true;
                }
                else if(grR == 0 || piR == 1)
                {
                    wfR = false;
                }
            }
        }

        if(wbL && wbR)
        {
            if(dotL > 0 && dotR > 0)
            {
                currentAccelerationL = acceleration;
                currentAccelerationR = acceleration;

                startTime[0] = Time.time;
                startTime[1] = Time.time;

                start[0] = true;
                start[1] = true;

                wbL = false;
                wbR = false;
            }
        }
        else if(wfL && wfR)
        {
            if(dotL < 0 && dotR < 0)
            {
                currentAccelerationL = -acceleration;
                currentAccelerationR = -acceleration;

                startTime[0] = Time.time;
                startTime[1] = Time.time;

                start[0] = true;
                start[1] = true;

                wfL = false;
                wfR = false;
            }
        }

        if(wbL && !wfL && !wbR)
        {
            if(dotL > 0)
            {
                currentAccelerationL = acceleration;

                startTime[0] = Time.time;
                start[0] = true;

                wbL = false;
            }
        }
        else if(!wbL && wfL && !wfR)
        {
            if(dotL < 0)
            {
                currentAccelerationL = -acceleration;

                startTime[0] = Time.time;
                start[0] = true;

                wfL = false;
            }
        }

        if(wbR && !wfR && !wbL)
        {
            if(dotR > 0)
            {
                currentAccelerationR = acceleration;

                startTime[1] = Time.time;
                start[1] = true;

                wbR = false;
            }
        }
        if(!wbR && wfR && !wfL)
        {
            if(dotR < 0)
            {
                currentAccelerationR = -acceleration;

                startTime[1] = Time.time;
                start[1] = true;

                wfR = false;
            }
        }
    }
 
    void InputWheelOldVersion()
    {
        //Set bool to false after a frame true and accl to zero
        if(start[0])
        {            
            if(Time.time - startTime[0] > 0.5)
            {
                start[0] = false;
                currentAccelerationL = 0;
            }
        }

        if(start[1])
        {
            if(Time.time - startTime[1] > 0.5)
            {
                start[1] = false;
                currentAccelerationL = 0;
            }
        }

        if(start[2])
        {
            if(Time.time - startTime[2] > 0.5)
            {
                start[2] = false;
                currentAccelerationR = 0;
            }
        }

        if(start[3])
        {
            if(Time.time - startTime[3] > 0.5)
            {
                start[3] = false;
                currentAccelerationR = 0;
            }
        }

        //If height is below dot
        if(heightL < 0)
        {
            //If both grip and pinch are pressed; break
            if(grL == 1 && piL == 1)
            {
                currentBreakingForceL = breakingForce;
                currentAccelerationL = 0;
            }
            else
            {
                currentBreakingForceL = 0;
            }
            //If its behind dot
            if(dotL < 0)
            {
                //Check if can go forward
                if(grL == 1 && piL == 0 && !wbL && !wfL)
                {
                    wbL = true;
                }
            }
            else if(dotL > 0)//If its infront of dot
            {
                //Check if can reverse
                if(grL == 1 && piL == 0 && !wbL && !wfL)
                {
                    wfL = true;
                }
            }

            //Goes forward
            if(dotL > 0 && grL == 1 && piL == 0 && wbL && !wfL)
            {
                currentAccelerationL = acceleration;
                startTime[0] = Time.time;

                start[0] = true;
                wbL = false;
            }    
            else if(dotL < 0 && grL == 1 && piL == 0 && !wbL && wfL)//Reverse
            {
                currentAccelerationL = -acceleration;
                startTime[1] = Time.time;

                start[1] = true;
                wfL = false;
            }
        }

        //If height is below dot
        if(heightR < 0)
        {
            //If both grip and pinch are pressed; break
            if(grR == 1 && piR == 1)
            {
                currentBreakingForceR = breakingForce;
                currentAccelerationR = 0;
            }
            else
            {
                currentBreakingForceR = 0;
            }
            //If its behind dot
            if(dotR < 0)
            {
                //Check if can go forward
                if(grR == 1 && piR == 0 && !wbR && !wfR)
                {
                    wbR = true;
                }
            }            
            else if(dotR > 0)//If its infront of dot
            {
                //Check if can reverse
                if(grR == 1 && piR == 0 && !wbR && !wfR)
                {
                    wfR = true;
                }
            }

            //Goes forward
            if(dotR > 0 && grR == 1 && piR == 0 && wbR && !wfR)
            {
                currentAccelerationR = acceleration;
                startTime[2] = Time.time;

                start[2] = true;
                wbR = false;
            }            
            else if(dotR < 0 && grR == 1 && piR == 0 && !wbR && wfR)//Reverse
            {
                currentAccelerationR = -acceleration;
                startTime[3] = Time.time;

                start[3] = true;
                wfR = false;
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

        currentAngular = rb.angularVelocity.magnitude;
        rb.maxAngularVelocity = maxAngular;

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
