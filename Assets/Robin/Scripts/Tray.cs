using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tray : MonoBehaviour
{
    public List<GameObject> items;
    public List<GameObject> burger;
    public bool full;

    public void TrayGo()
    {
        for(int i = 0; i < items.Count; i++)
        {
            if(items[i].GetComponent<BurgerMaker>())
            {
                burger = items[i].GetComponent<BurgerMaker>().burger;
            }
            else if(items[i].GetComponent<FryMaker>())
            {
                full = items[i].GetComponent<FryMaker>().full;
            }

            items[i].GetComponent<Rigidbody>().isKinematic = true;
            
            items[i].transform.SetParent(gameObject.transform);

            Debug.Log(items[i].name);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Item") || collision.transform.CompareTag("Patty"))
        {
            items.Add(collision.gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.transform.CompareTag("Item") || collision.transform.CompareTag("Patty"))
        {
            for(int i = 0; i < items.Count; i++)
            {
                if(items[i] == collision.gameObject)
                {
                    items.RemoveAt(i);
                }
            }
        }
    }
}
