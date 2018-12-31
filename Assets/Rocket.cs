using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    // Start is called before the first frame update

    Rigidbody rigidbody;
    AudioSource audioSource;
    [SerializeField] float thrustSpeed = 100; 
    [SerializeField] float rotationSpeed = 100; 
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
        Thrust();
        Rotate();
     
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            float thrustThisFrame = thrustSpeed * Time.deltaTime * 10;
            rigidbody.AddRelativeForce(Vector3.up * thrustThisFrame);
            if (!audioSource.isPlaying)
                audioSource.Play();
           

        }
        else
        {
            audioSource.Stop();
        }
    }

    private void Rotate()
    {

        rigidbody.freezeRotation = true;
        float rotationThisFrame = rotationSpeed * Time.deltaTime ;
       if (Input.GetKey(KeyCode.LeftArrow))
        { 
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
       else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        rigidbody.freezeRotation = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("OK");
                break;
            case "Fuel":
                Debug.Log("Fuel");
                break;
            default:
                Debug.Log("Dead");
                break;

        }
    }
}
