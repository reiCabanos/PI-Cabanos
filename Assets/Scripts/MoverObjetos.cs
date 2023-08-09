using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverObjetos : MonoBehaviour
{

   [SerializeField] float _forca = 1;
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody _rig= hit.collider.attachedRigidbody;
        if(_rig != null)
        {
            Vector3 forceDirection = hit.gameObject.transform.position - transform.position;
            forceDirection.y = 0;
            forceDirection.Normalize();
            _rig.AddForceAtPosition(forceDirection*_forca , transform.position,ForceMode.Impulse);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
