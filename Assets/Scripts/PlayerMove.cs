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

   
     [SerializeField]FreeLookCam _cam;

    [SerializeField] Transform _posRestatPlayer;
    [SerializeField] bool _checkMove;
    Vector3 _vMovimento;
    float _currentvelocity;
    [SerializeField] float _smoothTime=0.0f;
    Vector3 _input;
    


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
        


        if (_checkMove) { 
        Andar();
        }

        


        Jump();

        if (_checkJump)
        {
            _timer-=Time.deltaTime;
            if(_timer<0)
            {
                _checkJump = false;
                _timer = _timeValue;

            }
        }  
        Gravity();
      
       

    } 
    void Andar()
    {
        RoationPlayer();
        _characterController.Move(_vMovimento * _speed * Time.deltaTime);
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
            
       var tartAngle = Mathf.Atan2(_vMovimento.x,_vMovimento.z)*Mathf.Rad2Deg;
       var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, tartAngle, ref _currentvelocity, _smoothTime);
       transform.rotation=Quaternion.Euler(0,angle,0);
        

    }
    public void SetMove(InputAction.CallbackContext value)
    {
        _input= value.ReadValue<Vector3>();
        _moveX = _input.x;
        _moveZ = _input.y;
        _vMovimento=new Vector3(_moveX, _characterController.velocity.y, _moveZ);
       

    }
    public void SetJump(InputAction.CallbackContext value)
    {
        _checkJump = true;
      

    }
    public void SetMoveWalk(InputAction.CallbackContext value)
    {
        _checkwalk = value.performed;
    }
    public void SetCamera(InputAction.CallbackContext value)
    {
        _cam.posCam = value.ReadValue<Vector3>();
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
      //  _characterController.Move(new Vector3(_characterController.velocity.x, 0, _characterController.velocity.z));
        _playerVelocity.y += _gravityValue * Time.deltaTime;
      //  _playerVelocity.y = Mathf.Sqrt(_jumpForce / 5 * -3.0f * _gravityValue);
        _characterController.Move( _playerVelocity *Time.deltaTime);
      
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("filho"))
        {
            _posRestatPlayer = other.GetComponent<Resetar>()._posRestat;
            StartCoroutine(Dano());
        }

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
