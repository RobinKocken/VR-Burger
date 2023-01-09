using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Grill : MonoBehaviour
{
    public List<GameObject> patties;
    public List<float> timer;
    public List<int> num;

    public GameObject button;

    public Material cooked;
    public Material overCooked;

    public float cookedTime;
    public float overCookedTimer;

    public int degrees;
    public TMP_Text degreesText;

    void Update()
    {
        Temperature();
        Button();
    }
    void Temperature()
    {
        if(degrees > 200 && degrees < 240)
        {
            for(int i = 0; i < timer.Count; i++)
            {
                if(num[i] == 1)
                {
                    if(Time.time - timer[i] > overCookedTimer)
                    {
                        patties[i].GetComponent<Renderer>().material = overCooked;
                        num[i] = 2;

                        patties[i].GetComponent<Item>().stage = 2;
                    }
                }
                else if(num[i] == 0)
                {
                    if(Time.time - timer[i] > cookedTime)
                    {
                        patties[i].GetComponent<Renderer>().material = cooked;
                        num[i] = 1;

                        patties[i].GetComponent<Item>().stage = 1;
                    }
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
        else if(degrees > 300)
        {
            degrees = 300;
        }

        degreesText.text = degrees + "°C";
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Patty"))
        {
            patties.Add(collision.gameObject);
            timer.Add(Time.time);
            num.Add(0);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        for(int i = 0; i < patties.Count; i++)
        {
            if(collision.gameObject == patties[i])
            {
                patties.RemoveAt(i);
                timer.RemoveAt(i);

                return;
            }
        }
    }
}
