using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
   
    Camera mainCamera;
    void Start()
    {
        mainCamera = gameObject.GetComponent<Camera>();
        StartCoroutine(ZoomCameraIn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ZoomCameraIn( )
    {
        Vector3 target = new Vector3(0, 0, 0);
        float distance = Vector3.Distance(transform.position, target);
        while (distance > 50)
        {
        mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, target, 0.1f);
        distance = Vector3.Distance(transform.position, target);
        yield return new WaitForSeconds(0.03f);

        }
    }
}
