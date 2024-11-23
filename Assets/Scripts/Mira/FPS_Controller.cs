using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPS_Controller : MonoBehaviour
{
    public Camera playerCamera;
    [HideInInspector] public Vector3 playerCameraPos;
    [HideInInspector] public Quaternion playerCameraRot;
    public float lookSpeed = 3f;
   
    [HideInInspector] public float startLookSpeed;
    public float lookXLimit = 90f;
    float rotationX = 0;

    private bool canMove = true;
    CharacterController characterController;
    public float smoothingFactor = 0.1f;
    private Vector3 smoothInput;
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
            // Atualizando a rotação no eixo X
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            Vector3 mouseInput = new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
            // Girando a câmera no eixo X
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            smoothInput = Vector3.Lerp(smoothInput, mouseInput, smoothingFactor);

            // Mantendo o valor original do eixo Y de objectToMove
            float currentYRotation = objectToMove.rotation.eulerAngles.y;

            // Aplicando a nova rotação somente nos eixos X e Z
            objectToMove.rotation = Quaternion.Euler(rotationX, currentYRotation, 0);



        }
        
    }

    public void SetMouse(InputAction.CallbackContext value)
    {
        // Se canMove for verdadeiro, aplicamos o movimento da câmera
        if (canMove && _move._agoraMira==true)
        {
            Vector3 mouseDelta = value.ReadValue<Vector3>();
            Vector3 mouseInput = new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
            // Ajustando os valores de rotação com base na sensibilidade (lookSpeed)
            rotationX += -mouseDelta.y * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

            // Girando a câmera no eixo X
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            smoothInput = Vector3.Lerp(smoothInput, mouseInput, smoothingFactor);


            // Mantendo a rotação atual no eixo Y do objeto (apenas movimentando no eixo X)
            float currentYRotation = objectToMove.rotation.eulerAngles.y;

            // Aplicando a nova rotação no objeto
            objectToMove.rotation = Quaternion.Euler(rotationX, currentYRotation, 0);
        }
    }

}