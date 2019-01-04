using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinking : MonoBehaviour
{
    //[SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;

    // remove from inspector later
   
    float speed;
    const float tau = Mathf.PI * 2;
    float originalIntensity;
    // Start is called before the first frame update
    Light light;
    void Start()
    {
        light = GetComponent<Light>();
        originalIntensity = light.intensity;

    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon) { return; }
        //(protect against period =0;
        float cycles = Time.time / period;

        float rawSineWave = Mathf.Sin(cycles * tau);
        float dimmingValue = rawSineWave / 2f + 0.5f;
        light.intensity = originalIntensity * dimmingValue;


    }
}
