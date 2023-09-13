using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

using UnityEngine.AI;

public class InimigoMove : MonoBehaviour
{
    NavMeshAgent _agent;
    [SerializeField] Transform _alvo;
    [SerializeField] Transform[] _pos;
    float _distancia;
    [SerializeField] float _distanciaPatrulhar;
    int _numero;
    bool _seguirPlayer;
    [SerializeField] Transform _player;
    [SerializeField] float _distanciaPlayer;
    [SerializeField] Vector3 _speedAgente;
    [SerializeField] Hit Hit;
    [SerializeField] float _speedAnin;


    Animator _animator;
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        Hit = GetComponent<Hit>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
        Movimento();
        _speedAgente = _agent.velocity;
        Animacao();

    }


    void Animacao()
    {

        _speedAnin = math.abs(_speedAgente.x + _speedAgente.z);
        _animator.SetFloat("Speed", _speedAnin);
        _animator.SetBool("Hit", Hit._isHit);
    }

    void Patrulhar()
    {
        if (_distancia < _distanciaPatrulhar && _numero == 0)
        {
            _numero = 1;
        }
        else if (_distancia < _distanciaPatrulhar && _numero == 1)
        {
            _numero = 0;
        }

        _alvo = _pos[_numero];
    }
    void Movimento()
    {
        _distancia = _agent.remainingDistance;
        _distanciaPlayer = Vector3.Distance(transform.position, _player.position);
        if (_distanciaPlayer < 8)
        {
            _seguirPlayer = true;
        }
        else
        {
            _seguirPlayer = false;
        }


        if (!_seguirPlayer)
        {
            Patrulhar();
            _agent.SetDestination(_alvo.position);

        }
        else
        {

            _agent.SetDestination(_player.position);
        }


    }
}
