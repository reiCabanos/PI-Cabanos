using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SeguirPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    NavMeshAgent _agent;
    public Transform _alvo;
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        _agent.SetDestination(_alvo.position);
    }
}
