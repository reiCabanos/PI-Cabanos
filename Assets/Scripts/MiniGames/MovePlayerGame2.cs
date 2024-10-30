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

    // Variáveis de controle
    private float _moveX;
    private float _moveZ;
    private bool _checkJump = false;

    [Header("Debug")]
    public bool showDebugLogs = true;
    public Vector3 _posicaoinicial;
    public Animator _anim;
    private float _speedAnimY;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        _anim = GetComponent<Animator>();
        if (controller == null)
        {
            Debug.LogError("CharacterController não encontrado!");
            enabled = false;
        }
        _posicaoinicial = transform.position;
    }

    void Update()
    {
        float tempSpeed = Mathf.Abs(_moveX) + Mathf.Abs(_moveZ);
        _speedAnimY = controller.velocity.y;
        _anim.SetFloat("correndo", tempSpeed);
        _anim.SetBool("ground", controller.isGrounded);
        if (controller.isGrounded == false)
        {
            HandleGravity();
        }

        jumpHeight = controller.velocity.y;
        _anim.SetFloat("pulandoY", _speedAnimY);
        _anim.SetBool("parado", true);
       

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

        // Invertemos o eixo Z aqui para corrigir a direção do movimento
        Vector3 moveDirection = new Vector3(_moveX, 0f, _moveZ).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {
            float currentSpeed = Keyboard.current.leftShiftKey.isPressed ? runSpeed : walkSpeed;

            // Também ajustamos o ângulo de rotação para corresponder à nova direção
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
            _moveX = moveInput.x;  // Mantemos o X normal
            _moveZ = moveInput.y;  // O Y do input será usado como Z no mundo

            if (showDebugLogs)
            {
                /*Debug.Log($"Move input received: X={_moveX}, Z={_moveZ}");*/
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