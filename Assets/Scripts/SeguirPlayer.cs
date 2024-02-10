using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SeguirPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    NavMeshAgent _agent;
    public float _alvoDist;
    public Transform _alvo;
    public NpcsControlle _controlle;
    [SerializeField] Vector3 _speedAgente;
    [SerializeField] float _speedAnin;
    Animator _animator;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _controlle = Camera.main.GetComponent<NpcsControlle>();
        _animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        _agent.SetDestination(_alvo.position);
        _alvoDist = _agent.remainingDistance;

        if (_alvoDist < 5)
        {
            int r= Random.Range(0, _controlle._pos1.Count);
            _alvo=_controlle._pos1[r];
        }
        Animacao();
        _speedAgente = _agent.velocity;
    }
    void Animacao()
    {

        _speedAnin = Mathf.Abs(_speedAgente.x + _speedAgente.z);
        _animator.SetFloat("Speed", _speedAnin);
    }
}
