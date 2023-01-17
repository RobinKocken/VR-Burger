using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public GameObject ingredient;

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Right") || other.CompareTag("Left"))
        {
            if(other.GetComponent<Interact>().handClosed)
            {
                if(other.GetComponent<Interact>().target == null)
                {
                    other.GetComponent<Interact>().target = Instantiate(ingredient, other.transform.position, other.transform.rotation);
                }
            }
        }
    }
}
