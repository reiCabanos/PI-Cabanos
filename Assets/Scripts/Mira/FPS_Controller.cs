using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPS_Controller : MonoBehaviour
{
    public Camera playerCamera;
    [HideInInspector] public Vector3 playerCameraPos;
    [HideInInspector] public Quaternion playerCameraRot;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;
    public float lookSpeed = 2f;

    [HideInInspector] public float startLookSpeed;
    public float lookXLimit = 90f;
    float rotationX = 0;

    private bool canMove = true;
    CharacterController characterController;

    private float speed;
    public Transform objectToMove;
    public MoveNew _move;
    
   
   

    void Start()
    {
        startLookSpeed = lookSpeed;
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    void Update()
    {


        if (canMove)
        {
            // Atualizando a rota��o no eixo X
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

            // Girando a c�mera no eixo X
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

            // Mantendo o valor original do eixo Y de objectToMove
            float currentYRotation = objectToMove.rotation.eulerAngles.y;

            // Aplicando a nova rota��o somente nos eixos X e Z
            objectToMove.rotation = Quaternion.Euler(rotationX, currentYRotation, 0);



        }
        
    }

    public void SetMouse(InputAction.CallbackContext value)
    {
        // Se canMove for verdadeiro, aplicamos o movimento da c�mera
        if (canMove && _move._agoraMira==true)
        {// Pegando o valor do movimento do mouse
            Vector3 mouseDelta = value.ReadValue<Vector3>();

            // Atualizando a rota��o no eixo X (movimento vertical do mouse)
            rotationX += -mouseDelta.y * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

            // Girando a c�mera no eixo X
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

            // Mantendo a rota��o atual no eixo Y do objeto (apenas movimentando no eixo X)
            float currentYRotation = objectToMove.rotation.eulerAngles.y;

            // Aplicando a nova rota��o no objeto
            objectToMove.rotation = Quaternion.Euler(rotationX, currentYRotation, 0);
        }
    }

}