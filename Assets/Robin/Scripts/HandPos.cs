using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandPos : MonoBehaviour
{
    public InputActionProperty grip;
    float gripValue;

    public Movement move;

    public Transform target;
    Rigidbody rb;
    
    public enum Side { Left, Right}
    public Side Hand;

    bool moveLeft, moveRight;
    float velo;
    Vector3 prevPos;

    void Start()
    {
        rb = transform.root.GetComponent<Rigidbody>();
    }

    void Update()
    {
        gripValue = grip.action.ReadValue<float>();
        MoveHand();
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

    void MoveHand()
    {
        if(moveLeft)
        {
            if(gripValue == 1)
            {
                velo = (transform.position - prevPos).magnitude / Time.deltaTime;
                prevPos = transform.position;
            }
            else
            {
                velo = 0;
            }
        }
        else if(moveRight)
        {
            if(gripValue == 1)
            {
                velo = (transform.position - prevPos).magnitude / Time.deltaTime;
                prevPos = transform.position;
            }
            else
            {
                velo = 0;
            }
        }

        if(Hand == Side.Left)
        {
            move.velocityLeft = velo;
        }
        else if(Hand == Side.Right)
        {
            move.velocityRight = velo;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(Hand == Side.Left)
        {
            if(other.CompareTag("Left"))
            {
                moveLeft = true;
            }
        }

        if(Hand == Side.Right)
        {
            if(other.CompareTag("Right"))
            {
                moveRight = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(Hand == Side.Left)
        {
            if(other.CompareTag("Left"))
            {
                moveLeft = false;
                velo = 0;
            }
        }

        if(Hand == Side.Right)
        {
            if(other.CompareTag("Right"))
            {
                moveRight = false;
                velo = 0;
            }
        }
    }
}
