using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaioObjeto : MonoBehaviour
{
    
    // Start is called before the first frame update
    [SerializeField] private float _radius;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        FindThePlayer();
    }
    private void FindThePlayer()
    {
        Collider[] _raios = Physics.OverlapSphere(transform.position, _radius);
        foreach (var c in _raios)
        {
            if (c.CompareTag("J"))
            {
                transform.position = Vector3.MoveTowards(transform.position, c.transform.position
                        + new Vector3(0f, 0.5f, 0f), Time.deltaTime * 50f);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, _radius);
    }

}
