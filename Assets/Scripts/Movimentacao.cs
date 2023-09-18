using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimentacao : MonoBehaviour
{
    [SerializeField] CharacterController _controller;
    [SerializeField] float _velocidade;
    [SerializeField] float _moveH;
    [SerializeField] float _moveZ;
    [SerializeField] float _jumpForce;
    [SerializeField] float _rot;
    [SerializeField] float _girarSpeed;
    Vector3 _playerVelocity;
    [SerializeField] float _forceGravity = -9.81f;
    Animator _anim;
    [SerializeField]float _speed;
    float _speedAnimY;
    [SerializeField]Rigidbody _rid;
    [SerializeField] bool isGround;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _anim = GetComponent<Animator>();
    }

    void RoationPlayer()
    {
       
        _rot -= Input.GetAxis("Horizontal") * _girarSpeed;
        transform.localEulerAngles = new Vector3(0.0f, -_rot, 0.0f);

       

    }
    void GravityMode()
    {
       
        _playerVelocity.y += _forceGravity * Time.deltaTime;
        _controller.Move(_playerVelocity * Time.deltaTime);

    }
    void Jump()
    {
        if ((Input.GetAxisRaw("Jump") > 0 && _controller.isGrounded == true))
        {
            _playerVelocity.y += Mathf.Sqrt(_jumpForce * -3.0f * _forceGravity);
           

        }
    }
    void Andar()
    {
        isGround = _controller.isGrounded;
        _moveZ = Input.GetAxisRaw("Vertical");

        _controller.Move(transform.forward * _moveZ * _velocidade * Time.deltaTime);
       
       
       
        _anim.SetFloat("correndo", _speed);



    }
   


    void Update()
    {
        _anim.SetBool("chekground", _controller.isGrounded);
        if (_controller.isGrounded == false)
        {
            GravityMode();
        }
        _speedAnimY = _controller.velocity.y;
        _anim.SetFloat("pulandoY", _speedAnimY);


        Jump();
        Andar();
        
        GravityMode();
        RoationPlayer();

    }
}

