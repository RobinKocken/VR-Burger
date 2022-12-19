using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Interact : MonoBehaviour
{
    public Material blue;
    public Material oldMat;

    public GameObject handPhysic;
    public GameObject hand;

    public InputActionProperty grip;

    public float disToPickUp;
    public LayerMask layer;

    bool handClosed;
    float value;
    public GameObject target;

    float speed;
    Vector3 vel;
    Vector3 prev;

    public LayerMask ignoreLayer;
    public float maxDis;
    Transform ignoreTarget;
    XRDirectInteractor script;
    bool setOff;
    float distance;

    Quaternion initialObjectRotation;
    Quaternion initialControllerRotation;
    float angleDiff;
    public bool set;

    public List<GameObject> nearObjects;
    public GameObject closetsObject;
    float oldDis = 99;
    public Vector3 snapPos;
    Quaternion snapRot;


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
            if(Time.time - startTime > 0.5)
            {
                handPhysic.SetActive(true);
            }
        }
    }

    void FixedUpdate()
    {
        if(!handClosed)
        {
            script.enabled = true;
            set = false;

            if(closetsObject)
            {
                target.transform.position = snapPos;
                target.transform.rotation = snapRot;
                closetsObject = null;
            }

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
                setOff = true;
            }
            else
            {
                ignoreTarget = null;
            }
        }
        else
        {
            if(target && !setOff)
            {
                startTime = Time.time;

                if(target.CompareTag("Snap"))
                {
                    handPhysic.SetActive(false);
                    
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

                    target.transform.eulerAngles = new Vector3(target.transform.eulerAngles.x, target.transform.eulerAngles.y, -axisZ.eulerAngles.z);
                }
                else if(target.CompareTag("Item"))
                {
                    handPhysic.SetActive(false);

                    nearObjects.Remove(target);

                    target.GetComponent<Rigidbody>().velocity = (transform.position - target.transform.position) / Time.fixedDeltaTime;

                    target.GetComponent<Rigidbody>().maxAngularVelocity = 20;

                    Quaternion deltaRot = transform.rotation * Quaternion.Inverse(target.transform.rotation);
                    Vector3 eulerRot = new Vector3(Mathf.DeltaAngle(0, deltaRot.eulerAngles.x), Mathf.DeltaAngle(0, deltaRot.eulerAngles.y),
                    Mathf.DeltaAngle(0, deltaRot.eulerAngles.z));

                    eulerRot *= 0.95f;
                    eulerRot *= Mathf.Deg2Rad;
                    target.GetComponent<Rigidbody>().angularVelocity = eulerRot / Time.fixedDeltaTime;

                    foreach(GameObject g in nearObjects)
                    {
                        float dis = Vector3.Distance(target.transform.position, g.transform.position);

                        if(dis < oldDis)
                        {
                            closetsObject = g.gameObject;
                            oldDis = dis;
                        }
                    }                

                    if(closetsObject)
                    {
                        snapPos = closetsObject.transform.GetChild(0).transform.position;
                        snapRot = closetsObject.transform.rotation;
                    }
                }
            }
            else if(!target && setOff && ignoreTarget != null)
            {
                handPhysic.SetActive(false);
                distance = Vector3.Distance(ignoreTarget.position, transform.position);

                if(distance > maxDis)
                {
                    script.enabled = false;
                    setOff = false;
                    handPhysic.SetActive(true);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Item"))
        {
            nearObjects.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Item"))
        {
            nearObjects.Remove(other.gameObject);
        }
    }
}
