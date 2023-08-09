using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Inimigo : MonoBehaviour
{
    [SerializeField] NavMeshAgent _agent;
    [SerializeField] Transform _posionPlayer;
    [SerializeField] float _speedGet;
    [SerializeField] bool _stop;
    void Start()
    {
        _agent = gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        _agent.destination = _posionPlayer.position;
        _speedGet = Mathf.Abs(_agent.velocity.x + _agent.velocity.z);
    }
}
