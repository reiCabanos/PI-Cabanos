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
    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    void RoationPlayer()
    {
       
        _rot -= Input.GetAxis("Horizontal") * _girarSpeed;
        transform.localEulerAngles = new Vector3(0.0f, _rot, 0.0f);
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
        
        _moveZ = Input.GetAxisRaw("Vertical");

        _controller.Move(transform.forward * _moveZ * _velocidade * Time.deltaTime);
        
    }


    void Update()
    {
        if (_controller.isGrounded == false)
        {
            GravityMode();
        }

        Jump();
        Andar();
        GravityMode();
        RoationPlayer();

    }
}

