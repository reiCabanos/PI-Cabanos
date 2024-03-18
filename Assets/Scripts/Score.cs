using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    private Camera _mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        _mainCamera=Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position+_mainCamera.transform.forward);
    }
    private void OnEnable()
    {
        Invoke("InActive", 0.5f);
    }
    private void InActive()
    {
        gameObject.SetActive(false);
    }
}
