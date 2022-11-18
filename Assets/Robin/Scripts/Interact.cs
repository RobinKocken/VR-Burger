using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    public InputActionProperty grip;

    public float disToPickUp;
    public LayerMask layer;

    bool handClosed;
    float value;
    Rigidbody target;

    float speed;
    Vector3 vel;
    Vector3 prev;

    void Start()
    {
        
    }

    void Update()
    {
        value = grip.action.ReadValue<float>();

        vel = (transform.position - prev) * Time.deltaTime;
        prev = transform.position;
        speed = vel.magnitude;
    }

    void FixedUpdate()
    {
        if(value == 1)
        {
            handClosed = true;
        }
        else
        {
            handClosed = false;
        }

        if(!handClosed)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, disToPickUp, layer);

            if(colliders.Length > 0)
            {
                target = colliders[0].transform.root.GetComponent<Rigidbody>();
            }
            else
            {
                target = null;
            }
        }
        else
        {
            if(target)
            {
                target.velocity = (transform.position - target.transform.position) / Time.fixedDeltaTime;

                target.maxAngularVelocity = 20;

                Quaternion deltaRot = transform.rotation * Quaternion.Inverse(target.transform.rotation);
                Vector3 eulerRot = new Vector3(Mathf.DeltaAngle(0, deltaRot.eulerAngles.x), Mathf.DeltaAngle(0, deltaRot.eulerAngles.y),
                Mathf.DeltaAngle(0, deltaRot.eulerAngles.z));

                eulerRot *= 0.95f;
                eulerRot *= Mathf.Deg2Rad;
                target.angularVelocity = eulerRot / Time.fixedDeltaTime;
            }
        }
    }
}
