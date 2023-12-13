using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

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


    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        _checkGround = _controller.isGrounded;
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
          //  _moveX = 0;
           // _moveZ = 0;
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

    void MovimentoPlayer()
    {
        //orienta��o do movimento
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
        if (other.gameObject.CompareTag("filho"))
        {
            _posRestatPlayer = other.GetComponent<Resetar>()._posRestat;
            StartCoroutine(Dano());
        }
        if (other.gameObject.CompareTag("item"))
        {
            Debug.Log("ff");
        }

    }
    IEnumerator Dano()
    {
        _checkMove = false;
        yield return new WaitForSeconds(2);
        transform.position = _posRestatPlayer.position;
        yield return new WaitForSeconds(1);
        _checkMove = true;
        Debug.Log("dano");
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
        if (_mira1 && _project._sandaliaOn) { 
            _anim.SetBool("atirar", true);
        Invoke("MiraFalse", 0.5f);
            Invoke("DesativarSandalia", 5f);
        }

    }

    public void DesativarSandalia()
    {
        _project._sandalia.gameObject.SetActive(false);
        _project._sandaliaOn = true;
    }
    void MiraFalse()
    {
        _anim.SetBool("atirar", false);
    }
    
    

}
