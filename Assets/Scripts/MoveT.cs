using NaughtyCharacter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public enum ERotationBehavior
{
    OrientRotationToMovement,
    UseControlRotation
}
[System.Serializable]
public class RotationSettings
{
    [Header("Control Rotation")]
    public float MinPitchAngle = -45.0f;
    public float MaxPitchAngle = 75.0f;

    [Header("Character Orientation")]
    public ERotationBehavior RotationBehavior = ERotationBehavior.OrientRotationToMovement;
    public float MinRotationSpeed = 600.0f; // The turn speed when the player is at max speed (in degrees/second)
    public float MaxRotationSpeed = 1200.0f; // The turn speed when the player is stationary (in degrees/second)
}
[System.Serializable]
public class GroundSettings
{
    public LayerMask GroundLayers; // Which layers are considered as ground
    public float SphereCastRadius = 0.35f; // The radius of the sphere cast for the grounded check
    public float SphereCastDistance = 0.15f; // The distance below the character's capsule used for the sphere cast grounded check
}
public class MoveT : MonoBehaviour
{
    public Controller Controller;
    public RotationSettings RotationSettings;
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
    [SerializeField] float _velocidade;
    [SerializeField] Transform _orientation;
    private Vector2 _controlRotation;
    public GroundSettings GroundSettings;
    public Vector2 CameraInput { get; private set; }

    void Start()
    {
       Controller.Init();
      // Controller.MoveT = this;
        _anim = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
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
        Gravidade();




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
    private void Gravidade()
    {
        _playerVelocity.y = _playerVelocity.y + _gravityValue * Time.deltaTime;
        _controller.Move(_playerVelocity * Time.deltaTime);
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
    public Vector2 GetControlRotation()
    {
        return _controlRotation;
    }

    public void SetControlRotation(Vector2 controlRotation)
    {
        // Adjust the pitch angle (X Rotation)
        float pitchAngle = controlRotation.x;
        pitchAngle %= 360.0f;
        pitchAngle = Mathf.Clamp(pitchAngle, RotationSettings.MinPitchAngle, RotationSettings.MaxPitchAngle);

        // Adjust the yaw angle (Y Rotation)
        float yawAngle = controlRotation.y;
        yawAngle %= 360.0f;

        _controlRotation = new Vector2(pitchAngle, yawAngle);
    }
    private bool CheckGrounded()
    {
        Vector3 spherePosition = transform.position;
        spherePosition.y = transform.position.y + GroundSettings.SphereCastRadius - GroundSettings.SphereCastDistance;
        bool isGrounded = Physics.CheckSphere(spherePosition, GroundSettings.SphereCastRadius, GroundSettings.GroundLayers, QueryTriggerInteraction.Ignore);

        return isGrounded;
    }
    private void OrientToTargetRotation(Vector3 horizontalMovement, float deltaTime)
    {
        if (RotationSettings.RotationBehavior == ERotationBehavior.OrientRotationToMovement && horizontalMovement.sqrMagnitude > 0.0f)
        {
            float rotationSpeed = Mathf.Lerp(
                RotationSettings.MaxRotationSpeed, RotationSettings.MinRotationSpeed, _moveDir.x / _controller.velocity.y);

            Quaternion targetRotation = Quaternion.LookRotation(horizontalMovement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * deltaTime);
        }
        else if (RotationSettings.RotationBehavior == ERotationBehavior.UseControlRotation)
        {
            Quaternion targetRotation = Quaternion.Euler(0.0f, _controlRotation.y, 0.0f);
            transform.rotation = targetRotation;
        }
    }
    
   
}
