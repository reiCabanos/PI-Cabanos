using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{

    Animator _anin;
    public bool _isHit = false;
    GameObject _mae;
    Animator _aninMae;
   


    // Start is called before the first frame update

    void Start()
    {
        _anin = GetComponent<Animator>();        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
       if (other.gameObject.name== "mae")
        {
              _isHit = true;
            Morte();


        }
    }

    void Morte()
    {
        _mae.SetActive(false);

    }
}

