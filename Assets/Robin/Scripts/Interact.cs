using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Interact : MonoBehaviour
{
    Item item;

    public GameObject handPhysic;
    public GameObject hand;

    public InputActionProperty grip;

    public float disToPickUp;
    public LayerMask layer;

    public bool handClosed;
    float value;
    public GameObject target;

    float speed;
    Vector3 vel;
    Vector3 prev;

    public LayerMask ignoreLayer;
    public float maxDis;
    Transform ignoreTarget;
    XRDirectInteractor script;
    float distance;

    Quaternion initialObjectRotation;
    Quaternion initialControllerRotation;
    public bool set;

    float startTime;

    void Start()
    {
        script = hand.GetComponent<XRDirectInteractor>();
    }

    void Update()
    {
        value = grip.action.ReadValue<float>();

        vel = (transform.position - prev) * Time.deltaTime;
        prev = transform.position;
        speed = vel.magnitude;

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
            if(Time.time - startTime > 0.3)
            {
                handPhysic.SetActive(true);
            }
        }
    }

    void FixedUpdate()
    {
        if(!handClosed)
        {
            if(item != null)
            {
                item.pickedUp = false;
                item = null;
            }

            script.enabled = true;
            set = false;

            Collider[] colliders = Physics.OverlapSphere(transform.position, disToPickUp, layer);
            Collider[] ignoreCollider = Physics.OverlapSphere(transform.position, disToPickUp, ignoreLayer);

            if(colliders.Length > 0)
            {
                target = colliders[0].gameObject;
            }
            else
            {                
                target = null;
            }

            if(ignoreCollider.Length > 0)
            {
                ignoreTarget = ignoreCollider[0].transform;
            }
            else
            {
                ignoreTarget = null;
            }
        }
        else
        {
            if(target && !ignoreTarget)
            {
                startTime = Time.time;

                if(target.CompareTag("Item") || target.CompareTag("Patty") || target.CompareTag("Pin"))
                {
                    handPhysic.SetActive(false);

                    if(target.GetComponent<Rigidbody>().isKinematic == true) target.GetComponent<Rigidbody>().isKinematic = false;

                    item = target.GetComponent<Item>();
                    item.pickedUp = true;

                    target.GetComponent<Rigidbody>().velocity = (transform.position - target.transform.position) / Time.fixedDeltaTime;

                    target.GetComponent<Rigidbody>().maxAngularVelocity = 20;

                    Quaternion deltaRot = transform.rotation * Quaternion.Inverse(target.transform.rotation);
                    Vector3 eulerRot = new Vector3(Mathf.DeltaAngle(0, deltaRot.eulerAngles.x), Mathf.DeltaAngle(0, deltaRot.eulerAngles.y),
                    Mathf.DeltaAngle(0, deltaRot.eulerAngles.z));

                    eulerRot *= 0.95f;
                    eulerRot *= Mathf.Deg2Rad;
                    target.GetComponent<Rigidbody>().angularVelocity = eulerRot / Time.fixedDeltaTime;
                }
                else if(target.CompareTag("Rot"))
                {
                    handPhysic.SetActive(false);

                    if(set == false)
                    {
                        target.transform.eulerAngles = new Vector3(target.transform.eulerAngles.x, target.transform.eulerAngles.y, -target.transform.eulerAngles.z);
                        initialObjectRotation = target.transform.rotation;
                        initialControllerRotation = transform.rotation;                   
                        
                        set = true;
                    }

                    Quaternion controllerAngularDifference = initialControllerRotation * Quaternion.Inverse(transform.rotation);
                    Quaternion axisZ = (controllerAngularDifference * initialObjectRotation);

                    target.transform.rotation = Quaternion.Euler(target.transform.eulerAngles.x, target.transform.eulerAngles.y, -axisZ.eulerAngles.z);
                }
            }
            else if(!target && ignoreTarget)
            {
                handPhysic.SetActive(false);
                distance = Vector3.Distance(ignoreTarget.position, transform.position);

                if(distance > maxDis)
                {
                    script.enabled = false;
                    handPhysic.SetActive(true);
                }
            }
        }
    }
}
