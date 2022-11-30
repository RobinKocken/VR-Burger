using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

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
    public float dotL;
    //[DoNotSerialize]
    public float dotR;

    public float acL, acR;
    public float brL, brR;
    public float tnL, tnR;

    public float acceleration;
    public float breakingForce;
    public float maxTurnAngle;

    float currentAccelerationL;
    float currentAccelerationR;
    float currentBreakingForceL;
    float currentBreakingForceR;
    float currentMaxTurnAngleL;
    float currentMaxTurnAngleR;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, camOffset.position.y - camOffset.position.y, transform.position.z);
    }

    void FixedUpdate()
    {
        Wheeling();
    }

    void Wheeling()
    {
        //Left
        //Acceleration Left Side
        if(acL == 1)
            currentAccelerationL = acceleration;
        else
            currentAccelerationL = 0;
        //Break Left Side
        if(brL == 1)
            currentBreakingForceL = breakingForce;
        else
            currentBreakingForceL = 0;

        //Right
        //Acceleration Right Side
        if(acR == 1)
            currentAccelerationR = acceleration;
        else
            currentAccelerationR = 0;
        //Breal
        if(brR == 1)
            currentBreakingForceR = breakingForce;
        else
            currentBreakingForceR = 0;

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

        colliderLeft.steerAngle = currentMaxTurnAngleL;
        colliderRight.steerAngle = currentMaxTurnAngleR;

        UpdateWheel(colliderLeft, wheelLeft);
        UpdateWheel(colliderRight, wheelRight);
    }

    void UpdateWheel(WheelCollider col, Transform trans)
    {
        col.GetWorldPose(out Vector3 pos, out Quaternion rot);

        trans.position = pos;
        trans.rotation = rot;
    }

    public float Dotting(Transform hand)
    {
        var dirToDot = Vector3.Normalize(hand.position - dotPos.position);

       float dot;
       return dot = Vector3.Dot(dotPos.forward, dirToDot);
    }
}
