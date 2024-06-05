using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    private Vector2 CameraRotation;
    [SerializeField] private float Sensitivity;

    private void Awake()
    {
        CameraRotation = new Vector3(150, 10);
    }
    private void Update()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        CameraRotation.x += Input.GetAxis("Mouse X") * Sensitivity;
        CameraRotation.y -= Input.GetAxis("Mouse Y") * Sensitivity;

        CameraRotation.y = Mathf.Clamp(CameraRotation.y, -60, 60);

        transform.rotation = Quaternion.Euler(CameraRotation.y, CameraRotation.x,0);
    }
}
