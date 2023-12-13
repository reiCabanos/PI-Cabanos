using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumirSandalia : MonoBehaviour
{
    public float _falt = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("item"))
        {

            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            Debug.Log("dd");
        }
    }
}
