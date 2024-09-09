using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Menu Control Settings")]
    [SerializeField] private List<MenuControl> _MenuControls; // Lista de menus

    [Header("Bot�es")]
    [SerializeField] private Button botaoIniciar; // Refer�ncia ao bot�o iniciar
    [SerializeField] private Button botaoSair;    // Refer�ncia ao bot�o para sair do jogo

    [Header("Scene Settings")]
    [SerializeField] private string nomeDaCena; // Nome da cena a ser carregada (definido pelo Inspetor)

    [Header("Tela de Carregamento")]
    [SerializeField] private GameObject telaDeCarregamentoPrefab; // Refer�ncia ao prefab da tela de carregamento

    private MenuControl _currentMenu;

    void Start()
    {
        InitializeMenus(); // Inicializa os menus corretamente

        // Certifique-se de que os bot�es est�o atribu�dos corretamente no Inspetor
        botaoIniciar.onClick.AddListener(IniciarJogo); // Liga o bot�o "Iniciar" para carregar a cena
        botaoSair.onClick.AddListener(SairDoJogo);  // Liga o bot�o "Sair" para sair ou voltar ao menu principal

        // Certifique-se de que a tela de carregamento est� desativada inicialmente
        if (telaDeCarregamentoPrefab != null)
        {
            telaDeCarregamentoPrefab.SetActive(false);
        }
    }

    // Inicializa os menus, deixando apenas o primeiro ativo
    private void InitializeMenus()
    {
        foreach (var menu in _MenuControls)
        {
            menu.transform.localScale = Vector3.zero;
            menu.gameObject.SetActive(false);
        }

        if (_MenuControls.Count > 0)
        {
            _MenuControls[0].gameObject.SetActive(true);
            _MenuControls[0].MenuOFF();
            _MenuControls[0].transform.DOScale(1f, 0.5f);
            _MenuControls[0].ChamaMenu();
            _currentMenu = _MenuControls[0];
        }
    }

    // Fun��o para carregar a cena de jogo quando o bot�o iniciar for clicado
    public void IniciarJogo()
    {
        // Verifica se o nome da cena foi configurado no Inspetor
        if (!string.IsNullOrEmpty(nomeDaCena))
        {
            Debug.Log("Carregando a cena: " + nomeDaCena); // Log para verificar se a fun��o foi chamada

            // Inicia a corrotina para exibir a tela de carregamento e carregar a cena
            StartCoroutine(ExibirTelaDeCarregamentoECarregarCena());
        }
        else
        {
            Debug.LogError("Nome da cena n�o est� definido no Inspetor!");
        }
    }

    // Corrotina que garante que a tela de carregamento seja exibida antes do carregamento da cena
    private IEnumerator ExibirTelaDeCarregamentoECarregarCena()
    {
        // Ativa o prefab da tela de carregamento
        if (telaDeCarregamentoPrefab != null)
        {
            telaDeCarregamentoPrefab.SetActive(true);
        }

        // Espera um frame para garantir que a tela de carregamento seja renderizada
        yield return null;

        // Inicia o carregamento ass�ncrono da cena
        AsyncOperation operacao = SceneManager.LoadSceneAsync(nomeDaCena);

        // Enquanto a cena est� carregando
        while (!operacao.isDone)
        {
            // Aqui voc� pode adicionar uma l�gica para atualizar a UI de carregamento
            // Se houver um slider ou barra de progresso, voc� pode atualizar aqui:
            // Exemplo:
            // slider.value = Mathf.Clamp01(operacao.progress / 0.9f);

            yield return null;  // Espera at� o pr�ximo frame
        }
    }

    // Fun��o para sair do jogo ou voltar ao menu principal
    public void SairDoJogo()
    {
        SceneManager.LoadScene("Menu"); // Exemplo de como voltar ao menu principal
    }
}
