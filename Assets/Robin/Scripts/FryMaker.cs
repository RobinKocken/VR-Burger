using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryMaker : MonoBehaviour
{
    public Item interact;

    public bool full;

    public Material blue;
    public GameObject item;
    public GameObject clone;

    int n;

    public List<GameObject> fries;

    void Update()
    {
        if(clone)
        {
            if(!interact.pickedUp)
            {
                Destroy(clone);

                item.transform.position = transform.GetChild(n).position;
                item.transform.rotation = transform.GetChild(n).rotation;

                item.GetComponent<Rigidbody>().isKinematic = true;
                item.GetComponent<Rigidbody>().detectCollisions = false;

                item.transform.SetParent(transform.GetChild(n));

                fries.Insert(n, item);

                interact = null;
                item = null;

                if(fries[4] != null)
                {
                    full = true;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Item") && !full)
        {
            if(other.GetComponent<Item>().item == 9)
            {
                for(int i = 0; i < fries.Count; i++)
                {
                    if(fries[i] == null)
                    {
                        item = other.gameObject;
                        interact = item.GetComponent<Item>();

                        clone = Instantiate(item, transform.GetChild(i).position, transform.GetChild(i).rotation);

                        clone.tag = "Untagged";
                        clone.GetComponent<Rigidbody>().isKinematic = true;
                        clone.GetComponent<Rigidbody>().detectCollisions = false;
                        clone.GetComponent<Renderer>().material = blue;

                        clone.transform.SetParent(transform.GetChild(i));

                        n = i;
                        return;
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Item") || other.CompareTag("Patty"))
        {
            if(item)
            {
                Destroy(clone);
                interact = null;
                item = null;
            }
        }
    }
}
