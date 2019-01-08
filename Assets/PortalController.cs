using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PortalController : MonoBehaviour
{
    [SerializeField] GameObject targetPortal;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject getTargetPortal()
    {
        return targetPortal;
    }
}
