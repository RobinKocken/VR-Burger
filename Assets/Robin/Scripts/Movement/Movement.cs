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

    public float currentAccelerationL;
    public float currentAccelerationR;
    public float currentBreakingForceL;
    public float currentBreakingForceR;

    float[] startTime = new float[3];

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

        Debug.Log(currentAccelerationR);
    }

    void FixedUpdate()
    {
        Wheeling();
    }

    void InputWheel()
    {
        //Set bool to false after a frame true and accl to zero
        if(wbL)
        {            
            if(Time.time - startTime[1] > 0.5)
            {
                wbL = false;
                currentAccelerationL = 0;
            }
        }

        if(wfL)
        {
            if(Time.time - startTime[1] > 0.5)
            {
                wfL = false;
                currentAccelerationL = 0;
            }
        }

        if(wbR)
        {
            if(Time.time - startTime[2] > 0.5)
            {
                wbR = false;
                currentAccelerationR = 0;
            }
        }

        if(wfR)
        {
            if(Time.time - startTime[3] > 0.5)
            {
                wfR = false;
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
            }    
            else if(dotL < 0 && grL == 1 && piL == 0 && !wbL && wfL)//Reverse
            {
                currentAccelerationL = -acceleration;
                startTime[1] = Time.time;
            }
        }
        else
        {
            wbL = false;
            wfL = false;

            currentAccelerationL = 0;
            currentBreakingForceL = 0;
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
            }            
            else if(dotR < 0 && grR == 1 && piR == 0 && !wbR && wfR)//Reverse
            {
                currentAccelerationR = -acceleration;
                startTime[3] = Time.time;
            }
        }
        else
        {
            wbR = false;
            wfR = false;

            currentAccelerationR = 0;
            currentBreakingForceR = 0;
        }
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

                //Move in Right direction
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
