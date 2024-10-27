using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Conta : MonoBehaviour
{
    public int _numb1;
    public int _numb2;
    public int _resp;

    public string _conta;
    public TextMeshPro _textResp;
   ControleConta _contaControl;
    void Start()
    {
        
        _resp = _numb1 + _numb2;
        ContaSet("?");
      

        _contaControl = Camera.main.GetComponent<ControleConta>();

        float r = Random.Range(0.2f, 0.5f);
        Invoke("NunbEnviar", r);
    }

    public void ContaSet(string conta)
    {
        _conta = _numb1 + " + " + _numb2 + " = " + conta;
        _textResp.text = _conta;
    }

    void NunbEnviar() {
        _contaControl._respList.Add(_resp);
    }

}
