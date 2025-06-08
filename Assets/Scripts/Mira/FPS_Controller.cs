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
    
    //physics push
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody
        if (body == null || body.isKinematic)
            return;

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3f)
            return;

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);


        // Apply the push
        body.velocity = pushDir * 1;
    }

    void Start()
    {
        startLookSpeed = lookSpeed;
        characterController = GetComponent<CharacterController>();
       
    }
    public void ApplyKickback(float kickbackStrength)
    {
        // Adjust xRotation to simulate kickback
        rotationX -= kickbackStrength;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);
    }

    private Vector3 moveVelocity = Vector3.zero; // Current movement velocity
    public float acceleration = 5f; // How quickly the player reaches full speed
    public float deceleration = 5f; // How quickly the player slows down

    void Update()
    {

        #region Handles Movement
        Vector3 targetVelocity = Vector3.zero; // The velocity the player is trying to reach

        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);


        speed = isRunning ? runSpeed : walkSpeed;

        // Check for WASD keys and set target velocity
        if (canMove)
        {
            if (Input.GetKey(KeyCode.W))
            {
                targetVelocity += transform.forward * speed;
            }
            if (Input.GetKey(KeyCode.S))
            {
                targetVelocity -= transform.forward * speed;
            }
            if (Input.GetKey(KeyCode.A))
            {
                targetVelocity -= transform.right * speed;
            }
            if (Input.GetKey(KeyCode.D))
            {
                targetVelocity += transform.right * speed;
            }
        }

        // Smoothly interpolate current velocity towards the target velocity
        moveVelocity = Vector3.Lerp(moveVelocity, targetVelocity, (targetVelocity.magnitude > 0 ? acceleration : deceleration) * Time.deltaTime);

        #endregion

        #region Handles Jumping
        if (Input.GetKey(KeyCode.Space) && canMove && characterController.isGrounded)
        {
            moveVelocity.y = jumpPower;
        }
        else if (!characterController.isGrounded)
        {
            moveVelocity.y -= gravity * Time.deltaTime;
        }
        #endregion

        #region Handles Rotation and Movement
        characterController.Move(moveVelocity * Time.deltaTime); // Use the smoothly updated velocity

       

        if (canMove)
        {
            // Atualizando a rotação no eixo X
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

            // Girando a câmera no eixo X
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

            // Mantendo o valor original do eixo Y de objectToMove
            float currentYRotation = objectToMove.rotation.eulerAngles.y;

            // Aplicando a nova rotação somente nos eixos X e Z
            objectToMove.rotation = Quaternion.Euler(rotationX, currentYRotation, 0);



        }
        #endregion
    }

    public void SetMouse(InputAction.CallbackContext value)
    {
        // Se canMove for verdadeiro, aplicamos o movimento da câmera
        if (canMove && _move._agoraMira==true)
        {// Pegando o valor do movimento do mouse
            Vector3 mouseDelta = value.ReadValue<Vector3>();

            // Atualizando a rotação no eixo X (movimento vertical do mouse)
            rotationX += -mouseDelta.y * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

            // Girando a câmera no eixo X
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

            // Mantendo a rotação atual no eixo Y do objeto (apenas movimentando no eixo X)
            float currentYRotation = objectToMove.rotation.eulerAngles.y;

            // Aplicando a nova rotação no objeto
            objectToMove.rotation = Quaternion.Euler(rotationX, currentYRotation, 0);
        }
    }

}