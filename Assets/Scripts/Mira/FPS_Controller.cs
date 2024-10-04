using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
            rotationX += -Input.GetAxis("Mouse Y") * (lookSpeed);
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

            // Girando o objeto pai junto com a câmera
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            objectToMove.rotation = Quaternion.Euler(rotationX, objectToMove.rotation.y, 0);
           


        }
        #endregion
    }
}