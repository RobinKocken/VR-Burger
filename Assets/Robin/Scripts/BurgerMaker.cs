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

    void Start()
    {
        burger.Add(bottomBox);
    }

    void Update()
    {
        if(clone)
        {
            if(!interact.pickedUp)
            {
                Destroy(clone);

                item.transform.position = burger.Last().transform.GetChild(0).position;
                item.transform.rotation = burger.Last().transform.GetChild(0).rotation;

                item.GetComponent<Rigidbody>().isKinematic = true;
                item.GetComponent<Rigidbody>().detectCollisions = false;

                if(!once)
                {
                    burger[0] = item;
                    once = true;
                }
                

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
                Debug.Log("Enter");

                item = other.gameObject;
                interact = item.GetComponent<Item>();

                clone = Instantiate(item, burger.Last().transform.GetChild(0).position, burger.Last().transform.GetChild(0).rotation);

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
                Debug.Log("Exit");

                Destroy(clone);
                interact = null;
                item = null;
            }
        }
    }
}
