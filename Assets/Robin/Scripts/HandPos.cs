using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandPos : MonoBehaviour
{
    public Movement move;
    Rigidbody rb;

    public InputActionProperty grip;
    float gripValue;
    public InputActionProperty pinch;
    float pinchValue;

    public Transform target;
   
    public enum Side { Left, Right}
    public Side Hand;

    void Start()
    {
        rb = transform.root.GetComponent<Rigidbody>();
    }

    void Update()
    {
        gripValue = grip.action.ReadValue<float>();
        pinchValue = pinch.action.ReadValue<float>();
        InputHand();
    }

    void FixedUpdate()
    {
        TrackHand();
    }

    void TrackHand()
    {
        rb.velocity = (target.position - transform.position) / Time.fixedDeltaTime;

        rb.maxAngularVelocity = 20;

        Quaternion deltaRot = target.rotation * Quaternion.Inverse(transform.rotation);
        Vector3 eulerRot = new Vector3(Mathf.DeltaAngle(0, deltaRot.eulerAngles.x), Mathf.DeltaAngle(0, deltaRot.eulerAngles.y),
        Mathf.DeltaAngle(0, deltaRot.eulerAngles.z));

        eulerRot *= 0.95f;
        eulerRot *= Mathf.Deg2Rad;
        rb.angularVelocity = eulerRot / Time.fixedDeltaTime;
    }

    void InputHand()
    {
        if(Hand == Side.Left)
        {
            if(gripValue == 1)
            {
                move.grL = 1;
            }
            else
            {
                move.grL = 0; 
            }

            if(pinchValue == 1)
            {
                move.piL = 1;
            }
            else
            {
                move.piL = 0;
            }

            (move.dotL, move.heightL) = move.Dotting(transform);
        }
        else if(Hand == Side.Right)
        {
            if(gripValue == 1)
            {
                move.grR = 1;
            }
            else
            {
                move.grR = 0;
            }

            if(pinchValue == 1)
            {
                move.piR = 1;
            }
            else
            {
                move.piR = 0;
            }

            (move.dotR, move.heightR) = move.Dotting(transform);
        }
    }
}
