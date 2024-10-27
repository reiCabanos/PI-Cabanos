using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class blocoNumero : MonoBehaviour
{
    public TextMeshPro _textBloco;
    public int _numeroBloco;
    void Start()
    {
        _textBloco.text =""+ _numeroBloco;
    }

   
}
