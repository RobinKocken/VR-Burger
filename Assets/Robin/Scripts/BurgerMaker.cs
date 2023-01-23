using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BurgerMaker : MonoBehaviour
{
    public Item interact;

    GameObject item;
    public GameObject topBox;
    public GameObject bottomBox;

    public Material blue;

    public List<GameObject> burger;

    GameObject clone;

    bool once;

    void Update()
    {
        if(clone)
        {
            if(!interact.pickedUp)
            {
                Destroy(clone);

                if(!once)
                {
                    item.transform.position = bottomBox.transform.GetChild(0).position;
                    item.transform.rotation = bottomBox.transform.GetChild(0).rotation;

                    burger.Add(item);
                    once = true;
                }
                else
                {
                    item.transform.position = burger.Last().transform.GetChild(0).position;
                    item.transform.rotation = burger.Last().transform.GetChild(0).rotation;

                    burger.Add(item);
                }


                item.GetComponent<Rigidbody>().isKinematic = true;
                item.GetComponent<Rigidbody>().detectCollisions = false;              

                item.transform.SetParent(bottomBox.transform);

                interact = null;
                item = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Item") || other.CompareTag("Patty"))
        {
            if(other.GetComponent<Item>().item >= 0)
            {
                item = other.gameObject;
                interact = item.GetComponent<Item>();

                if(!once)
                {
                    clone = Instantiate(item, bottomBox.transform.GetChild(0).position, bottomBox.transform.GetChild(0).rotation);
                }
                else
                {
                    clone = Instantiate(item, burger.Last().transform.GetChild(0).position, burger.Last().transform.GetChild(0).rotation);
                }

                clone.tag = "Untagged";
                clone.GetComponent<Rigidbody>().isKinematic = true;
                clone.GetComponent<Rigidbody>().detectCollisions = false;
                clone.GetComponent<Renderer>().material = blue;

                clone.transform.SetParent(bottomBox.transform);
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
