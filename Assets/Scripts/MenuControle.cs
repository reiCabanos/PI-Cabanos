using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class MenuControle : MonoBehaviour
{
    [SerializeField] private string _nomeDaScena;
    [SerializeField]GameObject _painelMenuInicial;
    [SerializeField]GameObject _painelOpcoes;
    public Transform _painelIniciar;
    public Transform _painelMenu;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void Jogar()
    {
        SceneManager.LoadScene(_nomeDaScena);

    }
    public void AbrirOpcoes()
    {
        _painelMenuInicial.SetActive(false);
        _painelOpcoes.SetActive(true);

    }
    public void FecharOpcoes()
    {
        _painelOpcoes.SetActive(false);
        _painelMenuInicial.SetActive(true);
    }
    public void SairDoJogo()
    {
        
        SceneManager.LoadScene("Menu");
    }


    
}
