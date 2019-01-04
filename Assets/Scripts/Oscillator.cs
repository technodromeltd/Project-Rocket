using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{

    [SerializeField] Vector3 movementVector = new Vector3(10f,10f,10f);
    [SerializeField] float period = 2f;

    // remove from inspector later
    float movementFactor; // betwen 0 not moved and 1 fully moved
    Vector3 startingPos;
    Vector3 offset;
    float speed;
    const float tau = Mathf.PI * 2;
    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
       
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon) { return; }
        //(protect against period =0;
        float cycles = Time.time / period;
       
        float rawSineWave = Mathf.Sin(cycles * tau);
        movementFactor = rawSineWave / 2f + 0.5f;
        offset = movementVector * movementFactor;

        transform.position = startingPos + offset;

    }
}
