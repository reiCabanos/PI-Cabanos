using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralaxmove : MonoBehaviour
{
    private float cameraMoveSpeed = 3f;
    private Transform cameraTransform;

    
        void Start()
    {
        cameraTransform = Camera.main.transform;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 NewPosition = cameraTransform.position;
        NewPosition.x += cameraMoveSpeed * Time.deltaTime;
        cameraTransform.position = NewPosition; 
    }
}
