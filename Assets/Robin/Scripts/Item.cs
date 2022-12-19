using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public GameObject top;

    public bool active;

    void Start()
    {
        top = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        if(active)
        {

        }
    }
}
