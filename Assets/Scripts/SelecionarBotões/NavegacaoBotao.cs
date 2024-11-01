using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NavegacaoBotao : MonoBehaviour
{
    public GameObject botaoSair; // Referência ao botão "Sair"
    public GameObject botaoTentarNovamente; // Referência ao botão "Tentar Novamente"
    public GameObject[] botoes;
    private int indexAtual = 0;

    private void OnEnable()
    {
        // Inicializa os botões e garante que "Sair" seja selecionado
        botoes = new GameObject[] { botaoSair, botaoTentarNovamente };
        indexAtual = 0;
        SelecionarBotao(botaoSair);
    }

    private void Update()
    {
        // Verifica input para navegação (exemplo com teclas de seta ou o analógico)
        float movimento = Input.GetAxis("Horizontal"); // Ou "Vertical" para navegação vertical

        if (movimento > 0.1f)
        {
            // Move para o próximo botão
            indexAtual = (indexAtual + 1) % botoes.Length;
            SelecionarBotao(botoes[indexAtual]);
        }
        else if (movimento < -0.1f)
        {
            // Move para o botão anterior
            indexAtual = (indexAtual - 1 + botoes.Length) % botoes.Length;
            SelecionarBotao(botoes[indexAtual]);
        }

        // Garante que um botão esteja sempre selecionado
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            SelecionarBotao(botoes[indexAtual]);
        }
    }

    private void SelecionarBotao(GameObject botao)
    {
        // Limpa a seleção atual e seleciona o novo botão
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(botao);
    }
}
