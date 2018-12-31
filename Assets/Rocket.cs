using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    // Start is called before the first frame update

    Rigidbody rigidbody;
    AudioSource audioSource;
   // Rigidbody[] thrusters;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        
       // thrusters = GetComponentsInChildren<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
      ProcessInput();
    }

    private void ProcessInput()
    {
       if(Input.GetKey(KeyCode.Space))
        {


            rigidbody.AddRelativeForce(Vector3.up*100*Time.deltaTime);
            if (!audioSource.isPlaying)
                audioSource.Play();
           // thrusters[0].AddRelativeForce(Vector3.up*100*Time.deltaTime);

        } 
       else
        {
            audioSource.Stop();
        }

       if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward);
        }
       else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward);
        }
    }
}
