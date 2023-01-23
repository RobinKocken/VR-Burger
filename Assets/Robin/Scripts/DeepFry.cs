using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeepFry : MonoBehaviour
{
    public List<GameObject> potatos;
    public List<float> timer;

    public GameObject fries;

    public GameObject button;

    public int degrees;
    public TMP_Text degreesText;

    public float fried;

    public int itemNumber;

    void Start()
    {
        
    }

    void Update()
    {
        Temperature();
        Button();
    }

    void Temperature()
    {
        if(degrees > 300 && degrees < 320)
        {
            for(int i = 0; i < timer.Count; i++)
            {
                if(Time.time - timer[i] > fried)
                {
                    Instantiate(fries, potatos[i].transform.position, potatos[i].transform.rotation);
                    Destroy(potatos[i]);
                    potatos.RemoveAt(i);
                    timer.RemoveAt(i);
                }
            }
        }
    }

    void Button()
    {
        float rot = button.transform.eulerAngles.z;

        degrees = (int)rot;

        if(degrees < 0)
        {
            degrees = 0;
        }
        else if(degrees > 340)
        {
            degrees = 340;
        }

        degreesText.text = degrees + "°C";
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Item"))
        {
            if(other.GetComponent<Item>() != null)
            {
                if(other.GetComponent<Item>().item == itemNumber)
                {
                    potatos.Add(other.gameObject);
                    timer.Add(Time.time);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        for(int i = 0; i < potatos.Count; i++)
        {
            if(other.gameObject == potatos[i])
            {
                potatos.RemoveAt(i);
                timer.RemoveAt(i);

                return;
            }
        }
    }
}
