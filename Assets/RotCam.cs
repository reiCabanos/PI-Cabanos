using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotCam : MonoBehaviour
{
    // Start is called before the first frame update
    Transform cam;
    public Transform _transform;
    void Start()
    {
        cam = Camera.main.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.eulerAngles= new Vector3(-cam.transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
    }
}
