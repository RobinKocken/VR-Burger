using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Transform origin;
    public CharacterController controller;

    public Transform cam;
    public float disToCamera;

    public float movingSpeed;
    float runningSpeed;
    public float velocityRight, velocityLeft;
    bool left, right;

    void Start()
    {
        
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, cam.position.y - disToCamera, transform.position.z);

        Moving();
    }

    void Moving()
    {
        if(velocityLeft > 0.1 && velocityRight > 0.1)
        {
            Debug.Log("Moving");
            controller.Move(new Vector3(0, 0, movingSpeed) * Time.deltaTime);
        }
        //else
        //{
        //    if(velocityLeft > 0.1 && velocityRight == 0)
        //    {

        //    }
        //    else if(velocityRight > 0.1 && velocityLeft == 0)
        //    {

        //    }
        //}
    }
}
