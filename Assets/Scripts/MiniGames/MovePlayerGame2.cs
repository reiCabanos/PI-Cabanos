using UnityEngine;
using UnityEngine.InputSystem;

public class Moveplayergame2 : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public float rotationSpeed = 720f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    // Vari�veis de controle
    private float _moveX;
    private float _moveZ;
    private bool _checkJump = false;

    [Header("Debug")]
    public bool showDebugLogs = true;
    public Vector3 _posicaoinicial;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            Debug.LogError("CharacterController n�o encontrado!");
            enabled = false;
        }
        _posicaoinicial = transform.position;
    }

    void Update()
    {
        HandleMovement();
        HandleGravity();
    }

    void HandleMovement()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Invertemos o eixo Z aqui para corrigir a dire��o do movimento
        Vector3 moveDirection = new Vector3(_moveX, 0f, _moveZ).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {
            float currentSpeed = Keyboard.current.leftShiftKey.isPressed ? runSpeed : walkSpeed;

            // Tamb�m ajustamos o �ngulo de rota��o para corresponder � nova dire��o
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;

            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            controller.Move(moveDirection * currentSpeed * Time.deltaTime);

            if (showDebugLogs)
            {
              /*  Debug.Log($"Moving with speed: {currentSpeed}, Direction: {moveDirection}");*/
            }
        }

        if (_checkJump && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            _checkJump = false;

            if (showDebugLogs)
            {
            /*    Debug.Log("Jump executed!");*/
            }
        }
    }

    void HandleGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void SetMove(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            Vector2 moveInput = value.ReadValue<Vector2>();

            // Verifique se a entrada veio do teclado ou do joystick
            if (value.control.device is Keyboard)
            {
                _moveX = -moveInput.x;  // Inverte o X apenas para corrigir a dire��o horizontal
                _moveZ = moveInput.y;   // Mant�m o Y como est� para corrigir a dire��o vertical
            }
            else
            {
                _moveX = moveInput.x;
                _moveZ = moveInput.y;
            }

            if (showDebugLogs)
            {
                Debug.Log($"Move input received: X={_moveX}, Z={_moveZ}");
            }
        }
        else if (value.canceled)
        {
            _moveX = 0;
            _moveZ = 0;
        }
    }


    public void SetJump(InputAction.CallbackContext value)
    {
        if (value.performed && isGrounded)
        {
            _checkJump = true;

            if (showDebugLogs)
            {
                /*Debug.Log("Jump input received!");*/
            }
        }
    }
}