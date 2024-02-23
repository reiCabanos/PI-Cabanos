using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityStandardAssets.Cameras;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float _moveX;
    [SerializeField] float _moveZ;
    [SerializeField] float _speed;
    [SerializeField] float _jumpForce;
    public Vector3 _playerVelocity;
    CharacterController _characterController;
    [SerializeField] bool _checkJump;
    [SerializeField] bool _checkGround;
    [SerializeField] float _gravityValue=-9.8f;
    float _timer;
    [SerializeField] float _timeValue;
    Animator _anim;
    float _speedAnimY;
    [SerializeField] float _girarSpeed;
    [SerializeField] float _rot;
    [SerializeField] float _velocidade;
    [SerializeField]bool _checkwalk;
    [SerializeField] Transform _orientation;



    [SerializeField] Transform _posRestatPlayer;
    [SerializeField] bool _checkMove;
    Vector3 _moveDir;
    float _currentvelocity;
    [SerializeField] float _smoothTime=0.0f;
    Vector3 _input;
    public Transform _moveCamera;
    [SerializeField] int _quantVida = 3;
    [SerializeField] PlayerControle _playerControle;
    [SerializeField] bool _autoCorrer=true;
    [SerializeField] int _mod;





    void Start()
    {
        _characterController=GetComponent<CharacterController>();
       

        _timer = _timeValue;
        _anim = GetComponent<Animator>();

        _checkMove = true;
        





    }
    void Update()
    {
        
            _checkGround = _characterController.isGrounded;
            if (_checkGround)
            {
                _playerVelocity.y = 0;

            }

            float tempSpeed = Mathf.Abs(_moveX) + Mathf.Abs(_moveZ);
            _anim.SetFloat("correndo", tempSpeed);
            _anim.SetBool("chekground", _characterController.isGrounded);

            if (_characterController.isGrounded == false)
            {
                Gravity();
            }
            _speedAnimY = _characterController.velocity.y;
            _anim.SetFloat("pulandoY", _speedAnimY);
            _anim.SetBool("IsRunning", _checkwalk);



            if (_checkMove)
            {
                Andar();
            }


        

        Jump();

            if (_checkJump)
            {
                _timer -= Time.deltaTime;
                if (_timer < 0)
                {
                    _checkJump = false;
                    _timer = _timeValue;

                }

            }
        if (_autoCorrer)
        {

            CorrerAuto();
            _anim.SetFloat("correndo", 6);
             tempSpeed = Mathf.Abs(_moveX) + Mathf.Abs(_moveZ);
           

        }
        Gravity();



        
    }
    void Andar()
    {
       

        //orientação do movimento
        _moveDir = (_orientation.forward * _moveZ + _orientation.right * _moveX) * _speed;

        //movimento
        _characterController.Move(new Vector3(_moveDir.x, _characterController.velocity.y, _moveDir.z) * Time.deltaTime);

        if (_checkwalk && _velocidade != 0)
        {
            _speed = 6f;
        }
        else
        {
            _speed = 2.57f;
        }

    }
    void RoationPlayer()
    {
        if (_input.sqrMagnitude==0)return;
            
       var tartAngle = Mathf.Atan2( _moveDir.x, _moveDir.z)*Mathf.Rad2Deg;
       var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, tartAngle, ref _currentvelocity, _smoothTime);
       transform.rotation=Quaternion.Euler(0,angle,0);
        

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
    
    void Jump()
    {
        if (_checkGround==true && _checkJump)
        {
            _checkGround = false;
            _playerVelocity.y = 0;
            _playerVelocity.y = Mathf.Sqrt(_jumpForce /5 * -3.0f *_gravityValue);
        }
    }
    void Gravity()
    {
      
        _playerVelocity.y += _gravityValue * Time.deltaTime;
        _characterController.Move( _playerVelocity *Time.deltaTime);
      
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("filho"))
        {
            Debug.Log("Leo maconha");
            _posRestatPlayer = other.GetComponent<Resetar>()._posRestat;
            StartCoroutine(Dano());
            _moveCamera.localEulerAngles = new Vector3(_moveCamera.localEulerAngles.x, -144.043f, _moveCamera.localEulerAngles.z);
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, -144.043f, transform.localEulerAngles.z);
            _quantVida--;
            _playerControle.CheckIcomVida(_quantVida);
        }
        if (other.gameObject.CompareTag("p"))
        {
            RotacaoDaCamera();
        }


    }
    public void RotacaoDaCamera()
    {
        _moveCamera.localEulerAngles = new Vector3(_moveCamera.localEulerAngles.x, -270, _moveCamera.localEulerAngles.z);

        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 117.454f, transform.localEulerAngles.z);

    }
    public void CorrerAuto()
    {
        _checkwalk = true;
        _speed = 6f;
    }
    IEnumerator Dano()
    {
        _checkMove = false;
        yield return new WaitForSeconds(2);
        transform.position = _posRestatPlayer.position;
        yield return new WaitForSeconds(1);
        _checkMove =true;
        Debug.Log("dano");
    }
}
