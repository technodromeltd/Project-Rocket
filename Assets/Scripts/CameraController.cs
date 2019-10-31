using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float minYDistance = 15f; // the smoothing for the camera's rotation
    [SerializeField] float minXDistance = 15f; // 
    [SerializeField] float m_MoveSpeed = 0.1f; // the smoothing for the camera's rotation
    [SerializeField] float zoomOffset = 15f; // the smoothing for the camera's rotation
    [SerializeField] Transform m_Target; // the smoothing for the camera's rotation
    Rigidbody rb;
    Camera mainCamera;
    float StarterZPos;
    Vector3 startingPosition;
    bool beginAnimationIsPlaying = false;
    void Start()
    {
        m_Target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        mainCamera = gameObject.GetComponent<Camera>();
        StarterZPos = transform.position.z;
        rb = m_Target.GetComponent<Rigidbody>();
         StartCoroutine(ZoomCameraIn());
        startingPosition = transform.position;
      
    }

    // Update is called once per frame
    void Update()
    {
        if (beginAnimationIsPlaying)
            return;
        float yDistance = transform.position.y - m_Target.position.y;
        float xDistance = transform.position.x - m_Target.position.x;

        float zPos;
   
        zPos = startingPosition.z - (rb.velocity.magnitude/2);
        Vector3 targetPosition = new Vector3(m_Target.position.x, m_Target.position.y, zPos);
        
        if (yDistance > minYDistance || yDistance < -minYDistance)
        {
           
            if (targetPosition.y > startingPosition.y) {
                 transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * m_MoveSpeed);
            }    
        }
        if (xDistance > minYDistance || xDistance < -minYDistance)
        {
           
            if (targetPosition.x > startingPosition.x) {
                 transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * m_MoveSpeed);
            }    
        }


       
    }

    IEnumerator ZoomCameraIn( )

    {
        beginAnimationIsPlaying = true;
        Vector3 target = transform.position;
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - zoomOffset);
        float distance = Vector3.Distance(transform.position, target);
        while (distance > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, 0.4f);
        distance = Vector3.Distance(transform.position, target);
        yield return new WaitForSeconds(0.03f);
        }
        //transform.position = Vector3.MoveTowards(transform.position, target, 0.1f);
        beginAnimationIsPlaying = false;
    
    }
}
