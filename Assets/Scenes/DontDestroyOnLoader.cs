using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoader : UnityEngine.MonoBehaviour
{
    public bool isSingleton = true;
    public static DontDestroyOnLoader thisSingleObject = null;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (isSingleton)
        {
            if (thisSingleObject == null)
            {
                thisSingleObject = this;
            }
            else if (thisSingleObject != this)
            {
                Destroy(this);
            }
        }
    }
}