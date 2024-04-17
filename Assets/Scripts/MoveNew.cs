using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
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
    [SerializeField] Animator _anim;
    [SerializeField] Transform _posRestatPlayer;
    [SerializeField] Transform _mira;
    public bool _mira1;
    [SerializeField] Transform _miraFinal;
    public ProjectileThrow _project;
    public float _falt = 10f;
    PlayerPontos _playerPontos;


    [SerializeField] int _quantVida = 3;

    [SerializeField] PlayerControle _playerControle;
    public Transform _ativar;
    public GameController _gameController;
    Vector3 _input;
    [SerializeField] float _smoothTime = 0.0f;
    public float _currentvelocity;
    



    public GameObject _troca;

    public Vector3 miraV;
    public Transform _miraL;


    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _playerPontos=Camera.main.GetComponent<PlayerPontos>();
        _gameController = Camera.main.GetComponent<GameController>();
       _project=GetComponent<ProjectileThrow>();


    }

    // Update is called once per frame
    void Update()
    {
        if (_gameController._gamerOver == false)
        {
            

            _checkGround = _controller.isGrounded;
            //     _orientation.eu = Vector3.zero;
            if (_checkGround)
            {
                _playerVelocity.y = 0;

            }

            float tempSpeed = Mathf.Abs(_moveX) + Mathf.Abs(_moveZ);
            _anim.SetFloat("correndo", tempSpeed);
            _anim.SetBool("chekground", _controller.isGrounded);

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

            if (_checkwalk && _moveSpeed != 0)
            {
                _moveSpeed = 6f;
            }
            else
            {
                _moveSpeed = 2.57f;
            }
            Gravidade();
        }
        else
        {
            _anim.SetFloat("correndo", 0);
            _anim.SetFloat("pulandoY", 0);
            _anim.SetBool("IsRunning", false);
        }
      



    }

    void MovimentoPlayer()
    {
        //orientação do movimento
        _moveDir = (_orientation.forward * _moveZ + _orientation.right * _moveX) * _moveSpeed;

        //movimento
        _controller.Move(new Vector3(_moveDir.x, _controller.velocity.y, _moveDir.z) * Time.deltaTime);
    }
    void Jump()
    {
        if (_checkGround == true && _checkJump && _checkMove)
        {
            _checkGround = false;
            _playerVelocity.y = 0;
            _playerVelocity.y = Mathf.Sqrt(_jumpForce / 5 * -3.0f * _gravityValue);
        }
    }

    public void SetMove(InputAction.CallbackContext value)
    {
        if (_checkMove)
        {
            Vector3 m = value.ReadValue<Vector3>();
            _moveX = m.x;
            _moveZ = m.y;
            var tartAngle = Mathf.Atan2(m.x, m.z) * Mathf.Rad2Deg;
            var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, tartAngle, ref _currentvelocity, _smoothTime );
            transform.rotation = Quaternion.Euler(0, angle, 0);
            
        }
        

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
        _playerVelocity.y = _playerVelocity.y + _gravityValue * Time.deltaTime;
        _controller.Move(_playerVelocity * Time.deltaTime);
    }
    

    private void OnTriggerEnter(Collider other)
    {
        
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
    }

  
    public void SetMira(InputAction.CallbackContext callbackContext)
    {
       
        _mira1 = callbackContext.performed;
        _mira.gameObject.SetActive(_mira1);
        _miraFinal.gameObject.SetActive(_mira1);
        _checkMove = !_mira1;
       



    }
    public void SetAtirar(InputAction.CallbackContext callbackContext)
    {
        
        if (_mira1 && _project.objectToThrow) { 
            _anim.SetBool("atirar", true);
             Invoke("MiraFalse", 0.5f);
            Invoke("DesativarSandalia", 5f);
        }
       
    }

    public void DesativarSandalia()
    {
        _project.objectToThrow.gameObject.SetActive(false);
        _project._sandaliaOn = true;
    }
    void MiraFalse()
    {
        _anim.SetBool("atirar", false);
    }
    public void TrocaScene()
    {
        SceneManager.LoadScene("MiniGame1");
    }


    public void SetLookMira(InputAction.CallbackContext callbackContext)
    {

        miraV = callbackContext.ReadValue<Vector3>();
        _miraL.localEulerAngles = new Vector3(_miraL.localEulerAngles.x+(- miraV.y*2), 0,0);

    }

}
