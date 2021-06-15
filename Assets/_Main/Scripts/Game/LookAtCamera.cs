using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Transform currentCamera;

    private void Start()
    {
        Invoke("GetCamera", 1.5f);
        
    }

    public void GetCamera()
    {
        if (currentCamera == null)
        {   
            if(Camera.current != null)
            {
                currentCamera = Camera.current.transform;
            }
            
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (currentCamera == null)
        {
            GetCamera();
            return;
        }
        transform.LookAt(transform.position + currentCamera.transform.rotation * Vector3.forward, currentCamera.transform.rotation * Vector3.up);
    }
}
