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
                item.transform.eulerAngles = new Vector3(0, 0, 0);

                item.GetComponent<Rigidbody>().isKinematic = true;
                item.GetComponent<Rigidbody>().detectCollisions = false;

                burger.Add(item);

                interact = null;
                item = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Item") || other.CompareTag("Patty"))
        {
            item = other.gameObject;
            interact = item.GetComponent<Item>();

            clone = Instantiate(item, burger.Last().transform.GetChild(0).position, Quaternion.identity);

            clone.tag = "Untagged";
            clone.GetComponent<Rigidbody>().isKinematic = true;
            clone.GetComponent<Rigidbody>().detectCollisions = false;
            clone.GetComponent<Renderer>().material = blue;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(item)
        {
            Destroy(clone);
            interact = null;
            item = null;
        }
    }
}
