using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class AnimatioScript : MonoBehaviour
{
    public MoveNew _moveNew;
    // Start is called before the first frame update
    void Start()
    {
        _moveNew = transform.parent.GetComponent<MoveNew>();
    }

    // Update is called once per frame
    public void Atirar()
    {
      
       // _moveNew._project.Fire();
    }
}
