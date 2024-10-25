using UnityEngine;
using UnityEngine.InputSystem;

public class Moveplayergame : MonoBehaviour
{
    public Transform _orientation;               // Orientação do movimento
    public Transform playerCamera;               // Referência para a câmera do jogador
    public float _moveSpeed = 2.35f;             // Velocidade de movimentação
    [SerializeField] float _jumpForce = 5f;      // Força de pulo
    [SerializeField] float _gravityValue = -9.81f; // Gravidade
    [SerializeField] CharacterController _controller;
    [SerializeField] public Animator _anim;      // Animator para animações do personagem

    private Vector3 _moveDir;
    private Vector3 _playerVelocity;
    private bool _checkGround;
    private bool _checkJump = false;
    private Vector3 moveVector;
    private float rotationSpeed = 100f;

    void Start()
    {
        _controller = GetComponent<CharacterController>();

        // Certifique-se de que o Animator está atribuído
        if (_anim == null)
        {
            _anim = GetComponent<Animator>();
        }
    }

    void Update()
    {
        // Verifica se o jogador está no chão para resetar a velocidade vertical
        _checkGround = _controller.isGrounded;
        if (_checkGround && _playerVelocity.y < 0)
        {
            _playerVelocity.y = 0f;
        }

        // Movimenta o jogador na direção de orientação
        Vector3 move = _orientation.forward * _moveDir.z + _orientation.right * _moveDir.x;
        _controller.Move(move * _moveSpeed * Time.deltaTime);

        // Aplica a gravidade
        _playerVelocity.y += _gravityValue * Time.deltaTime;
        _controller.Move(_playerVelocity * Time.deltaTime);

        // Atualiza o Animator com base no movimento
        UpdateAnimator(move.magnitude);

        // Rotaciona o jogador com base no input de rotação
        transform.Rotate(Vector3.up, moveVector.x * rotationSpeed * Time.deltaTime);
    }

    // Método para capturar movimento de entrada
    public void SetMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        _moveDir = new Vector3(input.x, 0, input.y); // Captura o movimento nas direções X e Z
    }

    // Método para capturar rotação da câmera
    public void SetLookMira(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector2>();
    }

    // Método para capturar o input de pulo
    public void SetJump(InputAction.CallbackContext context)
    {
        if (_checkGround && context.performed)
        {
            _playerVelocity.y += Mathf.Sqrt(_jumpForce * -2f * _gravityValue);
            _checkJump = true;
        }
    }

    // Atualiza as variáveis do Animator com base no movimento e estado do jogador
    private void UpdateAnimator(float speed)
    {
        _anim.SetFloat("correndo", speed);
        _anim.SetBool("chekground", _checkGround);
        _anim.SetBool("IsRunning", speed > 0);
        _anim.SetFloat("pulandoY", _playerVelocity.y);
    }
}
