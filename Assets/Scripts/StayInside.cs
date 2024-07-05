using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayInside : MonoBehaviour
{

    public Transform _miniMapaCam;
    public float _minimapSize;
    Vector3 _tempV3;
    public GameController controller;
    
    void Start()
    {
        controller=Camera.main.GetComponent<GameController>();
        _miniMapaCam = controller._miniCam;
        
    }

    // Update is called once per frame
    void Update()
    {
        _tempV3 = transform.parent.transform.position;
        _tempV3.y = transform.position.y;
        transform.position = _tempV3;
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, _miniMapaCam.position.x-_minimapSize,_minimapSize+_miniMapaCam.position.x),transform.position.y,
            Mathf.Clamp(transform.position.z,_miniMapaCam.position.z-_minimapSize,_minimapSize+_miniMapaCam.position.z));
    }
}
