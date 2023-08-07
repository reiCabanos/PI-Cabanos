using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float _moveX;
    [SerializeField] float _moveZ;
    [SerializeField] float _speed;
    [SerializeField] float _jumpForce;
    Vector3 _playerVelocity;
    CharacterController _characterController;
    [SerializeField]bool _checkJump;
    [SerializeField] bool _checkGround;
    float _gravityValue=-9.8f;
    float _timer;
    [SerializeField] float _timeValue;
    Animator _anim;
    float _speedAnimY;


    void Start()
    {
        _characterController=GetComponent<CharacterController>();
        _timer = _timeValue;
        _anim = GetComponent<Animator>();
    }
    void Update()
    {

        _anim.SetBool("chekground", _characterController.isGrounded);
        _speed = _moveZ;
        _anim.SetFloat("correndo", _speed);

        if (_characterController.isGrounded == false)
        {
            Gravity();
        }
        _speedAnimY = _characterController.velocity.y;
        _anim.SetFloat("pulandoY", _speedAnimY);

        _checkGround = _characterController.isGrounded;
        _characterController.Move(new Vector3(_moveX*_speed,_characterController.velocity.y,_moveZ*_speed)*Time.deltaTime);
        
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
    public void SetMove(InputAction.CallbackContext value)
    {
        Vector3 m = value.ReadValue<Vector3>();
        _moveX = m.x;
        _moveZ = m.y;
       

    }
    public void SetJump(InputAction.CallbackContext value)
    {
        _checkJump = true;
      

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
}
