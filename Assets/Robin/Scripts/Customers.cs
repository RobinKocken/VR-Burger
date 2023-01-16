using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using TMPro;

public class Customers : MonoBehaviour
{
    public Food[] food;

    public Tray trayScript;
    public Pin pinScript;

    public GameObject[] customer;
    public GameObject pin;
    public GameObject card;
    public GameObject tray;

    GameObject spawnedCustomer;
    GameObject currentCustomer;
    GameObject goingCustomer;

    GameObject cloneCard;
    GameObject cloneTray;

    public TMP_Text burgerText;

    public List<GameObject> waypoint;

    public bool rightOrder;

    public bool spawned;
    public bool activeCostumer;
    public bool reveived;
    public bool pinned;

    public int spawnCount = 0;
    public int num = 3;
    public int orderNumber;

    public Vector3 trayPos;
    public Vector3 cardPos;

    void Start()
    {
        pinScript = pin.GetComponent<Pin>();
        trayScript = tray.GetComponent<Tray>();

        cloneTray = Instantiate(tray, trayPos, Quaternion.Euler(0, -90, 0));

        for(int i = 0; i < transform.childCount; i++)
        {
            waypoint.Add(transform.GetChild(i).gameObject);
        }        
    }
    
    void Update()
    {
        Customer();
    }

    void Customer()
    {
        //Spawn and going to first waypoint and waiting until next is free
        if(!spawned)
        {
            if(spawnCount == 0)
            {
                int c = Random.Range(0, 2);
                spawnedCustomer = Instantiate(customer[c], waypoint[0].transform.position, waypoint[0].transform.rotation);
                spawnCount++;
            }

            spawnedCustomer.transform.position = Vector3.Lerp(spawnedCustomer.transform.position, waypoint[1].transform.position, 2 * Time.deltaTime);
            spawnedCustomer.transform.rotation = Quaternion.Lerp(spawnedCustomer.transform.rotation, waypoint[1].transform.rotation, 1 * Time.deltaTime);

            if(Vector3.Distance(spawnedCustomer.transform.position, waypoint[1].transform.position) < 0.1 && !currentCustomer)
            {
                currentCustomer = spawnedCustomer;
                spawnedCustomer = null;
                spawnCount = 0;

                spawned = true;
            }
        }
         //Going to next Waypoint
        if(!activeCostumer && spawned)
        {
            currentCustomer.transform.position = Vector3.Lerp(currentCustomer.transform.position, waypoint[2].transform.position, 1 * Time.deltaTime);
            currentCustomer.transform.rotation = Quaternion.Lerp(currentCustomer.transform.rotation, waypoint[2].transform.rotation, 1 * Time.deltaTime);

            if(Vector3.Distance(currentCustomer.transform.position, waypoint[2].transform.position) < 0.3)
            {
                cloneCard = Instantiate(card, currentCustomer.transform.GetChild(2).position, Quaternion.identity);
                cloneCard.GetComponent<Rigidbody>().isKinematic = true;

                orderNumber = Random.Range(0, 5);

                activeCostumer = true;
                spawned = false;
                pinned = true;
            }
        }

        // Waiting for order and checking if its correct
        if(activeCostumer)
        {
            burgerText.text = food[orderNumber].burgerName;

            if(pinned)
            {
                cloneCard.transform.position = Vector3.Lerp(cloneCard.transform.position, cardPos, 3 * Time.deltaTime);

                if(Vector3.Distance(cloneCard.transform.position, cardPos) < 0.1)
                {
                    cloneCard.GetComponent<Rigidbody>().isKinematic = false;
                    pinned = false;
                }
            }

            if(pinScript.paid)
            {
                Debug.Log("Paid");

                cloneTray.GetComponent<Tray>().TrayGo();
                CheckOrder();

                if(rightOrder)
                {
                    Debug.Log("RightOrder = True");
                }
                else if(!rightOrder)
                {
                    Debug.Log("RightOrder = False");
                }

                cloneTray.transform.SetParent(currentCustomer.transform);
                cloneTray.transform.position = Vector3.Lerp(cloneTray.transform.position, currentCustomer.transform.GetChild(2).position, 3 * Time.deltaTime);

                if(Vector3.Distance(cloneTray.transform.position, currentCustomer.transform.GetChild(2).position) < 0.2)
                {
                    cloneTray = Instantiate(tray, trayPos, Quaternion.Euler(0, -90, 0));
                    Destroy(cloneCard);

                    goingCustomer = currentCustomer;
                    currentCustomer = null;

                    activeCostumer = false;
                    pinScript.paid = false;
                    reveived = true;
                }
            }
        }

        //Going after received the order
        if(reveived)
        {
            goingCustomer.transform.position = Vector3.Lerp(goingCustomer.transform.position, waypoint[num].transform.position, 1 * Time.deltaTime);
            goingCustomer.transform.rotation = Quaternion.Lerp(goingCustomer.transform.rotation, waypoint[num].transform.rotation, 1 * Time.deltaTime);

            if(Vector3.Distance(goingCustomer.transform.position, waypoint[num].transform.position) < 0.5)
            {
                if(num == 3)
                {
                    num = 4;
                }
                else if(num == 4)
                {
                    num = 5;
                }
                else if(num == 5)
                {
                    Destroy(goingCustomer.gameObject);
                    num = 3;

                    reveived = false;
                }
            }
        }
    }

    void CheckOrder()
    {
        if(trayScript.burger.Count > 0)
        {
            if(trayScript.burger.Count == food[orderNumber].burger.Count)
            {
                for(int i = 0; i < trayScript.burger.Count; i++)
                {
                    if(trayScript.burger[i].GetComponent<Item>().item == food[orderNumber].burger[i].GetComponent<Item>().item)
                    {
                        rightOrder = true;
                    }
                    else if(trayScript.burger[i].GetComponent<Item>().item != food[orderNumber].burger[i].GetComponent<Item>().item)
                    {
                        rightOrder = false;
                        return;
                    }
                }
            }
            else
            {
                rightOrder = false;
            }
        }
        else
        {
            rightOrder = false;
        }
    }

}

[System.Serializable]
public class Food
{
    public string burgerName;
    public List<GameObject> burger;

}
