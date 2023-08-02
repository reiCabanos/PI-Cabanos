using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    void Start()
    {
        _characterController=GetComponent<CharacterController>();

    }
    void Update()
    {
        _checkGround = _characterController.isGrounded;
        Gravity();
    }
    void Gravity()
    {
        _playerVelocity.y += _gravityValue * Time.deltaTime;
        _characterController.Move( _playerVelocity *Time.deltaTime);
    }
}
