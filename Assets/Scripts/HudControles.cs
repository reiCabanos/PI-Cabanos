using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Button = UnityEngine.UI.Button;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using DG.Tweening;
/*using UnityEditor.ShaderGraph;*/

public class HudControles : MonoBehaviour
{
    public Transform _telaIniciar;
    public Transform _telaHuds;
    public Transform _telaCelular;
    public Transform _telaInventario;
    public bool _ativar;
    public Button _sect;
    public bool _ativarInventario;
    public bool _ativarCelular;
    public bool _sair;
    public Transform _painelControles;
    public Transform _painelBluer;
    public Transform _painelConfig;
    public bool _ativaConfig;
   




    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetComecar(InputAction.CallbackContext value)
    {
        if (_ativar = value.performed)
        {

            _telaIniciar.DOScale(0, 0.2f);
            _painelBluer.DOScale(0, 0.2f);
            _telaHuds.DOScale(1, 1f);
            Debug.Log("controles");
        }


    }
    public void SetInventario(InputAction.CallbackContext value)
    {
        if (_ativarInventario = value.performed)
        {

            _telaHuds.DOScale(0, 0.2f);
            _telaCelular.DOScale(0, 0.2f);
            _telaInventario.DOScale(1, 1f);
            _painelBluer.DOScale(1, 1f);
            _painelConfig.DOScale(0, 0.2f);
            Debug.Log("inventario");
        }





    }

    public void SetCelular(InputAction.CallbackContext value)
    {
        if (_ativarCelular = value.performed)
        {
            _telaHuds.DOScale(0, 0.2f);
            _telaInventario.DOScale(0, 0.2f);
            _telaCelular.DOScale(1, 1f);
            _painelBluer.DOScale(1, 1f);
            _painelConfig.DOScale(0, 0.2f);
            Debug.Log("Celular");
        }





    }

    public void SetSair(InputAction.CallbackContext value)
    {
        if (_sair = value.performed)
        {
            _telaHuds.DOScale(1, 1f);
            _telaCelular.DOScale(0, 0.2f);
            _telaInventario.DOScale(0, 0.2f);
            _painelBluer.DOScale(0, 0.2f);
            _painelConfig.DOScale(0, 0.2f);
            Debug.Log("sair");
        }





    }
    public void SetConfiguracao(InputAction.CallbackContext value)
    {
        if (_ativaConfig = value.performed)
        {
            _telaHuds.DOScale(0, 0.2f);
            _telaCelular.DOScale(0, 0.2f);
            _telaInventario.DOScale(0, 0.2f);
            _telaIniciar.DOScale(0, 0.2f);
            _painelBluer.DOScale(1, 1f);
            _painelConfig.DOScale(1, 1f);

            Debug.Log("sss");
        }
        


        
    }


    IEnumerator PainelControles()
    {
        _painelControles.DOScale(1, 1f);
        yield return new WaitForSeconds(1.7f);
        _painelControles.DOScale(0, 0.2f);

    }
}
