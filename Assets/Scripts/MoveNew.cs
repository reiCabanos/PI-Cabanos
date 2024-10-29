using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class MoveNew : MonoBehaviour
{
    public Transform _orientation;

    float _moveX, _moveZ;
    [SerializeField] float _jumpForce;

    Vector3 _moveDir;
    [SerializeField] CharacterController _controller;
    [SerializeField] float _moveSpeed = 2.35f, _gravityValue = -9.81f;

    Vector3 _playerVelocity;
    float _timer;
    [SerializeField] float _timeValue;

    bool _checkJump;

    float _speedAnimY;
    bool _checkGround;
    [SerializeField] bool _checkMove;
    [SerializeField] bool _checkwalk;
    [SerializeField] public Animator _anim;
    [SerializeField] Transform _posRestatPlayer;
    [SerializeField] Transform _mira;
    public bool _mira1;
    [SerializeField] Transform _miraFinal;
    public ProjectileThrow _project;
    public float _falt = 10f;
    PlayerPontos _playerPontos;

    [SerializeField] PlayerControle _playerControle;
    public Transform _ativar;
    public GameController _gameController;
    Vector3 _input;
    public float _currentvelocity;
    bool test = true;

    public GameObject _troca;

    public Vector3 miraV;
    public Transform _miraL;
    [SerializeField] public string _tagCheckPoint;
    public GameManager _manager;
    public Vector3 _posSalvar;
    public Transform _juF;
    public Vector3 moveVector;
    public float rotationSpeed = 100f;

    InventarioControl _control;
    public ItemDados _itemDados;
    public SpriteRenderer _spriteRenderer;

    [Header("Stamina")]
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaDrainRate = 10f;
    public float staminaRecoveryRate = 5f;
    public UnityEngine.UI.Slider staminaSlider;
    public HudControles hudControles; // Referência ao script HudControles
    public Transform playerCamera;
    public GameObject pontoTroca;
    public TutoriasJogo _tutoriasJ;
    public TutoriasJogo2 _tutoriasJ2;
    public TutorialVendedor _tutoriasV;
    public bool podeAvancarTutorial = false; // Flag para verificar se o jogador pode avançar
    public bool podeAvancarTutorial2 = false; // Flag para verificar se o jogador pode avançar
    public bool podeAvancarTutorial3 = false;
    public bool podeAvancarManga = false;
    public TutorialManga _manga;
    public GameObject _tutorManga;




    void Start()
    {


        _controller = GetComponent<CharacterController>();
        _control = Camera.main.GetComponent<InventarioControl>();
        _playerPontos = Camera.main.GetComponent<PlayerPontos>();
        _gameController = Camera.main.GetComponent<GameController>();
        _project = GetComponent<ProjectileThrow>();
        _manager = Camera.main.GetComponent<GameManager>();
        Salves();
        pontoTroca.SetActive(false);

        currentStamina = maxStamina;
        if (staminaSlider != null)
        {
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = maxStamina;
        }
        else
        {
            Debug.LogError("Slider staminaSlider não atribuído no Inspector!");
        }
    }

    void Update()
    {
        transform.Rotate(Vector3.up, moveVector.x * rotationSpeed * Time.deltaTime);


        if (_gameController._gamerOver == false && !hudControles.blockMovement)
        {
            _checkGround = _controller.isGrounded;

            if (_checkGround)
            {
                _playerVelocity.y = 0;
            }

            float tempSpeed = Mathf.Abs(_moveX) + Mathf.Abs(_moveZ);
            _anim.SetFloat("correndo", tempSpeed);
            _anim.SetBool("chekground", _controller.isGrounded);
            _anim.SetBool("mirando", _mira1);

            if (_controller.isGrounded == false)
            {
                Gravidade();
            }

            _speedAnimY = _controller.velocity.y;
            _anim.SetFloat("pulandoY", _speedAnimY);
            _anim.SetBool("IsRunning", _checkwalk);

            Jump();

            if (_checkMove)
            {
                MovimentoPlayer();
            }
            else
            {
                _anim.SetFloat("correndo", 0);
                _anim.SetFloat("pulandoY", 0);
                _anim.SetBool("IsRunning", false);
            }

            if (_checkJump)
            {
                _timer -= Time.deltaTime;
                if (_timer < 0)
                {
                    _checkJump = false;
                    _timer = _timeValue;
                }
            }

            _moveSpeed = _checkwalk && _moveSpeed != 0 ? 6f : 2.57f;
            Gravidade();
        }
        else
        {
            _anim.SetFloat("correndo", 0);
            _anim.SetFloat("pulandoY", 0);
            _anim.SetBool("IsRunning", false);
            _moveZ = 0; // Zera o movimento para garantir que o personagem pare
        }

        if (_checkwalk && _moveSpeed != 0) // Correndo ou pulando
        {
            currentStamina -= staminaDrainRate * Time.deltaTime;
        }
        else // Parado ou andando
        {
            currentStamina += staminaRecoveryRate * Time.deltaTime;
        }

        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        UpdateStaminaSlider();

        if (currentStamina <= 0)
        {
            _checkwalk = false; // Impede que continue correndo sem estamina
        }
        else if (currentStamina > 20f) // Valor arbitrário, ajuste conforme necessário
        {
            _checkMove = true; // Restaura a habilidade de se mover quando a estamina se recuperar
        }
    }

    void UpdateStaminaSlider()
    {
        if (staminaSlider != null)
        {
            staminaSlider.value = currentStamina;
            staminaSlider.DOValue(currentStamina, 0.2f); // Animação suave (opcional)
        }
    }

    void MovimentoPlayer()
    {
        //orientação do movimento
        _moveDir = (_juF.forward * _moveZ) * _moveSpeed;

        //movimento
        _controller.Move(new Vector3(_moveDir.x, _controller.velocity.y, _moveDir.z) * Time.deltaTime);
    }

    void Jump()
    {
        if (_checkGround && _checkJump && _checkMove)
        {
            _checkGround = false;
            _playerVelocity.y = Mathf.Sqrt(_jumpForce / 5 * -3.0f * _gravityValue);
        }
    }

    public void SetMove(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector3>();
        _moveZ = moveVector.y;
    }

    public void SetJump(InputAction.CallbackContext value)
    {
        _checkJump = true;
    }

    public void SetMoveWalk(InputAction.CallbackContext value)
    {
        _checkwalk = value.performed;
    }

    private void Gravidade()
    {
        _playerVelocity.y += _gravityValue * Time.deltaTime;
        _controller.Move(_playerVelocity * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        /* if (other.gameObject.CompareTag("testes"))
         {
             playerCamera.transform.localRotation = Quaternion.Euler(22.787f, -0.928f, 0);
         }*/
        if (other.gameObject.CompareTag("item"))
        {
            _playerPontos.SomarPontos(1);
            other.GetComponent<ColetarItens>().DestroyItens();
        }
        if (other.gameObject.CompareTag("T"))
        {
            TrocaScene();
            _troca.SetActive(false);
        }
        if (other.gameObject.CompareTag("PontoTroca"))
        {


            _tutoriasV._conText = 0;
            _tutoriasV._fimTutor = false;
            _gameController._gamerOver = true;

            podeAvancarTutorial3 = true; // Habilita o input
            _tutoriasV.TempoTutorOff();
            Debug.Log("_tutoriasJ");
        }
        if (other.gameObject.CompareTag(_tagCheckPoint))
        {
            Debug.Log(other.gameObject.name);
            Debug.Log(other.transform.localPosition);
            _manager.Salvar();
            _manager.CheckPointSalvar(other.transform.localPosition);
        }

        if (other.gameObject.CompareTag("Tutor1"))
        {
            _tutoriasJ._conText = 0;
            _tutoriasJ._fimTutor = false;
            _gameController._gamerOver = true;

            podeAvancarTutorial = true; // Habilita o input
            _tutoriasJ.TempoTutorOff();
            Debug.Log("_tutoriasJ");




        }
        if (other.gameObject.CompareTag("Tutor2"))
        {
            _tutoriasJ2._conText = 0;
            _tutoriasJ2._fimTutor = false;
            _gameController._gamerOver = true;

            podeAvancarTutorial2 = true; // Habilita o input
            _tutoriasJ2.TempoTutorOff();
            Debug.Log("_tutoriasJ");




        }
        if (other.gameObject.CompareTag("Manga"))
        {
            _manga._conText = 0;
            _manga._fimTutor = false;
            _gameController._gamerOver = true;

            podeAvancarManga = true; // Habilita o input
            _manga.TempoTutorOff();
            Debug.Log("_tutoriasJ");




        }



    }
    private void OnTriggerExit(Collider other)
    {
        // Quando o jogador sair da área, desativa o botão de troca de cena
        if (other.gameObject.CompareTag("PontoTroca"))
        {
            //_troca.SetActive(false);
            podeAvancarTutorial3 = false;
        }
        if (other.CompareTag("Tutor1")) // Substitua "TutorialZone" pela tag desejada
        {
            podeAvancarTutorial = false; // Desabilita o input
            Debug.Log("Saiu da área do tutorial, input desabilitado.");
        }
        if (other.CompareTag("Tutor2")) // Substitua "TutorialZone" pela tag desejada
        {
            podeAvancarTutorial2 = false; // Desabilita o input
            Debug.Log("Saiu da área do tutorial, input desabilitado.");
        }
        if (other.CompareTag("Manga")) // Substitua "TutorialZone" pela tag desejada
        {
            podeAvancarManga = false; // Desabilita o input
            Debug.Log("Saiu da área do tutorial, input desabilitado.");
        }

    }

    public void SetMira(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            _mira1 = true;
            _mira.gameObject.SetActive(_mira1);
            _miraFinal.gameObject.SetActive(_mira1);
            _checkMove = _mira1;
            _moveSpeed = 0f;
            moveVector = Vector3.zero;
            _moveZ = 0;
        }
        else if (callbackContext.canceled)
        {
            _mira1 = false;
            _mira.gameObject.SetActive(_mira1);
            _miraFinal.gameObject.SetActive(_mira1);
            _checkMove = !_mira1;
            playerCamera.transform.localRotation = Quaternion.Euler(22.787f, -0.928f, 0);
            Debug.Log("A tecla de mira foi liberada.");
        }
    }
    public void SetAvanca(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            if (podeAvancarTutorial || podeAvancarTutorial2 || podeAvancarTutorial3 ||podeAvancarManga )
            {
                Debug.Log("Tecla de teste pressionada. Deve avançar o tutorial.");
                _tutoriasJ.TempoTutorOff();
                _tutoriasJ2.TempoTutorOff();
                _tutoriasV.TempoTutorOff();
                _manga.TempoTutorOff();
            }




        }
        else
        {
            Debug.Log("Não pode avançar o tutorial, pois o jogador não está na área correta.");
        }
    }
    public void TrocaScene()
    {
        SceneManager.LoadScene("MiniGame1");
    }

    public void SetLookMira(InputAction.CallbackContext callbackContext)
    {
        miraV = callbackContext.ReadValue<Vector2>();
    }

    void Salves()
    {
        // Adicionando uma chamada ao novo método para inicialização
        SalvarDados(); // Pode ser um novo método que você implementou
    }

    void SalvarDados()
    {
        // Implementação do método de salvar dados
        Debug.Log("Dados salvos com sucesso!");
    }
    public void AtivarPontoTroca()
    {
        pontoTroca.SetActive(true);
        Debug.Log("Ponto de troca de cena ativado!");
    }
    public void AtivarTutorManga()
    {
        _tutorManga.SetActive(true);
        Debug.Log("Ponto de troca de cena ativado!");
    }

}