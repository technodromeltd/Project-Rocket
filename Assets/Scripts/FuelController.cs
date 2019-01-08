using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelController : MonoBehaviour
{

    [SerializeField] float startingFuel = 50f;
    [SerializeField] float burnSpeed = 25f;
    [SerializeField] float fuelCell = 25f;

    Text fuelText;
    Text fuelTextAdded;
    float currentFuel;
    AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponentInChildren<AudioSource>();
        Text[] textFields = GetComponentsInChildren<Text>();
        fuelText = textFields[0];
        fuelTextAdded = textFields[1];
        currentFuel = startingFuel;
        //fuelTextAdded.text = "";
    }

    public void BurnFuel()
    {
        float burnRate = burnSpeed * Time.deltaTime;
        currentFuel -= burnRate;

    }


    public void UpdateFuelText()
    {
        string currentFuelString = ((int)currentFuel).ToString();
        fuelText.text = currentFuelString;
    }

    IEnumerator IncreaseFuel(float increase)
    {
        for (int i = 1; i < increase; i++)
        {
        currentFuel += 1;
        yield return new WaitForSeconds(0.05f);

        }
    }

      public void CollectFuelCell()
    {
        // Increase currentFuel amount, remove object from scene
        
        audioSource.Play();
        fuelTextAdded.text = "+" + fuelCell.ToString();
        
        fuelTextAdded.GetComponent<Animator>().Play("FadeOut");
        StartCoroutine(IncreaseFuel(fuelCell));
        

    }

    void Update()
    {
        UpdateFuelText();
    }

    public float getCurrentFuel()
    {
        return currentFuel;
    }

     
}
