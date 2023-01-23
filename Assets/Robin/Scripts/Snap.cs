using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Snap : MonoBehaviour
{
    public Item item;
    public GameObject objectItem;

    bool once;

    void Update()
    {
        if(objectItem != null)
        {
            if(!item.pickedUp)
            {
                if(!once)
                {
                    objectItem.GetComponent<Rigidbody>().isKinematic = true;

                    once = true;
                }

                objectItem.transform.position = transform.position;
                objectItem.transform.rotation = transform.rotation;
            }
            else if(item.pickedUp && once)
            {
                objectItem.GetComponent<Rigidbody>().isKinematic = false;

                objectItem = null;
                item = null;
                once = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {      
        if(other.CompareTag("Item") || other.CompareTag("Patty"))
        {
            Debug.Log(other);
            objectItem = other.gameObject;
            item = objectItem.GetComponent<Item>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Item") || other.CompareTag("Patty"))
        {
            if(objectItem)
            {
                objectItem = null;
                item = null;
            }
        }
    }
}
