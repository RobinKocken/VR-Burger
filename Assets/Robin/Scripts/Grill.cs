using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Grill : MonoBehaviour
{
    public List<GameObject> patties;
    public List<float> timer;

    public GameObject button1;
    public GameObject button2;

    public Material cooked;
    public Material overCooked;

    public TMP_Text degrees;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Temperature();
    }
    void Temperature()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Item"))
        {
            patties.Add(collision.gameObject);
            timer.Add(Time.time);
        }
    }
}
