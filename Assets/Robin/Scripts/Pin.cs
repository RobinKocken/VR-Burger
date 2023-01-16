using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Pin : MonoBehaviour
{
    public bool paid;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Pin"))
        {
            paid = true;
        }
    }
}
