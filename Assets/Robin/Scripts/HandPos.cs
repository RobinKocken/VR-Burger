using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPos : MonoBehaviour
{
    public Transform target;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rb.velocity = (transform.position - target.transform.position) / Time.fixedDeltaTime;

        rb.maxAngularVelocity = 20;

        Quaternion deltaRot = transform.rotation * Quaternion.Inverse(target.transform.rotation);
        Vector3 eulerRot = new Vector3(Mathf.DeltaAngle(0, deltaRot.eulerAngles.x), Mathf.DeltaAngle(0, deltaRot.eulerAngles.y),
        Mathf.DeltaAngle(0, deltaRot.eulerAngles.z));

        eulerRot *= 0.95f;
        eulerRot *= Mathf.Deg2Rad;
        rb.angularVelocity = eulerRot / Time.fixedDeltaTime;
    }
}
