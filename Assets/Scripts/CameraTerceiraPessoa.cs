using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTerceiraPessoa : MonoBehaviour
{
    [Header("References")]
    public Transform _orientation;
    public Transform _player;
    public Transform _playerObj;
    public Rigidbody _rb;

    public float _rotationSpeed;

    
    public Vector3 _camRota;

    public enum CameraEstilo
    {
        Basic,
        Combat,
    }

    private void Start()
    {
        //Cursor ficar invisivel
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    private void Update()
    {


        //como conservar a rotação da camera, na troca de estilos de camera



        //Orientação da rotação
        if (_orientation != null)
        {
            RotCam();
        }
        //RotCam();



    }

    void RotCam()
    {
        //Orientação da rotação

        Vector3 _viewDir = (_player.position - new Vector3(transform.position.x, _player.position.y, transform.position.z))*0.5f;

        _orientation.forward = _viewDir.normalized;

        //Rotacionar o Objeto Player

        float _hInput = Input.GetAxisRaw("Horizontal");
        float _vInput = Input.GetAxisRaw("Vertical");
        Vector3 _InputDir = _orientation.forward * _vInput + _orientation.right * _hInput;

        if (_InputDir != Vector3.zero)
        {
            _playerObj.forward = Vector3.Slerp(_playerObj.forward, _InputDir.normalized, Time.deltaTime * _rotationSpeed*0.2f);
        }

    }



}
