using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;


public class Moveplayergame : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public float turnSpeed = 100.0f;
    public GameObject _painel;
    public GameObject arma;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    // Variáveis de controle
    private float _moveX;
    private float _moveZ;
    private bool _checkMove = true;
    private bool _checkJump = false;

    public TextMeshPro _textPlayer;
    public blocoNumero _blocoNumero;
    public Conta _conta;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void FixedUpdate()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Calcular a direção de movimento
        Vector3 move = transform.right * _moveX + transform.forward * _moveZ;

        // Definir velocidade de caminhada ou corrida
        float speed = Keyboard.current.leftShiftKey.isPressed ? runSpeed : walkSpeed;
        controller.Move(move * speed * Time.fixedDeltaTime);

        // Verificar se o pulo foi ativado e o personagem está no chão
        if (_checkJump && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            _checkJump = false; // Resetar pulo
        }

        // Aplicar gravidade
        velocity.y += gravity * Time.fixedDeltaTime;
        controller.Move(velocity * Time.fixedDeltaTime);

        // Rotação
        Rotate(_moveX);
    }

    private void Rotate(float input)
    {
        transform.Rotate(Vector3.up * input * turnSpeed * Time.fixedDeltaTime);
    }

    // Callback para o movimento
    public void SetMove(InputAction.CallbackContext value)
    {
        if (_checkMove && value.performed) // Verifique se a ação foi "performed"
        {
            Vector3 moveInput = value.ReadValue<Vector3>();
            _moveX = moveInput.x;
            _moveZ = moveInput.y;
        }
        else if (value.canceled) // Resetar quando a ação é cancelada
        {
            _moveX = 0;
            _moveZ = 0;
        }
    }

    // Callback para o pulo
    public void SetJump(InputAction.CallbackContext value)
    {
        if (value.performed && isGrounded) // Somente pular quando "performed" e no chão
        {
            _checkJump = true;
            Debug.Log("Jump triggered");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bloco"))
        {
            _blocoNumero = other.gameObject.GetComponent<blocoNumero>();
            _textPlayer.text = "" + _blocoNumero._numeroBloco;

        }
        if (other.gameObject.CompareTag("Conta"))
        {
            _conta = other.gameObject.GetComponent<Conta>();
            if (_conta._resp == _blocoNumero._numeroBloco)
            {

                _conta.ContaSet("" + _blocoNumero._numeroBloco);
            }
            else
            {
                Debug.Log("errou");
            }
        }
    }
}