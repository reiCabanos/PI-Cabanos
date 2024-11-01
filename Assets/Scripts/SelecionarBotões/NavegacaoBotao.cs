using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NavegacaoBotao : MonoBehaviour
{
    public GameObject botaoSair; // Refer�ncia ao bot�o "Sair"
    public GameObject botaoTentarNovamente; // Refer�ncia ao bot�o "Tentar Novamente"
    public GameObject[] botoes;
    private int indexAtual = 0;

    private void OnEnable()
    {
        // Inicializa os bot�es e garante que "Sair" seja selecionado
        botoes = new GameObject[] { botaoSair, botaoTentarNovamente };
        indexAtual = 0;
        SelecionarBotao(botaoSair);
    }

    private void Update()
    {
        // Verifica input para navega��o (exemplo com teclas de seta ou o anal�gico)
        float movimento = Input.GetAxis("Horizontal"); // Ou "Vertical" para navega��o vertical

        if (movimento > 0.1f)
        {
            // Move para o pr�ximo bot�o
            indexAtual = (indexAtual + 1) % botoes.Length;
            SelecionarBotao(botoes[indexAtual]);
        }
        else if (movimento < -0.1f)
        {
            // Move para o bot�o anterior
            indexAtual = (indexAtual - 1 + botoes.Length) % botoes.Length;
            SelecionarBotao(botoes[indexAtual]);
        }

        // Garante que um bot�o esteja sempre selecionado
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            SelecionarBotao(botoes[indexAtual]);
        }
    }

    private void SelecionarBotao(GameObject botao)
    {
        // Limpa a sele��o atual e seleciona o novo bot�o
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(botao);
    }
}
