using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Transform camera;

    private void Start()
    {
        Invoke("GetCamera", 1.5f);
        
    }

    public void GetCamera()
    {
        if (camera == null)
        {
            camera = Camera.current.transform;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (camera == null)
        {
            return;
        }
        transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
    }
}
