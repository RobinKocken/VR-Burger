using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public Transform fallpoint;
    public Transform cam;
    public float speed;
    public bool quit;

    public ParticleSystem blood;

    public float timing;
    float waitTime;

    void Update()
    {
        Falling();
    }

    void Falling()
    {
        if(cam)
        {
            cam.position = Vector3.Lerp(cam.position, fallpoint.position, speed * Time.deltaTime);
            cam.rotation = Quaternion.Slerp(cam.rotation, fallpoint.rotation, speed * Time.deltaTime);

            if(Vector3.Distance(cam.position, fallpoint.position) < 0.1)
            {
                fallpoint.GetComponent<AudioSource>().Play();
                blood.Play();

                if(Time.time - waitTime > timing)
                {
                    if(!quit) MainLevel();
                    else if(quit) Quiting();
                }
            }
        }
    }

    void MainLevel()
    {
        SceneManager.LoadScene(1);
    }

    void Quiting()
    {
        Application.Quit(); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<Rigidbody>().isKinematic = true;
            cam = other.GetComponent<Movement>().camOffset;

            waitTime = Time.time;
        }
    }
}