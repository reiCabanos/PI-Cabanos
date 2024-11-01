using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BotoesFimJogo : MonoBehaviour
{
    public GameObject painel; // O painel ao qual este script pertence
    public GameObject botaoInicial; // O botão que deve ser selecionado inicialmente
    public GameObject[] botoes; // Lista de botões para navegação
    private int indexAtual = 0;

    private void OnEnable()
    {
        // Inicializa o botão inicial como selecionado
        if (painel.activeInHierarchy)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(botaoInicial);
            indexAtual = 0;
        }
    }

    private void Update()
    {
        // Verifica se o painel está ativo antes de permitir a navegação
        if (!painel.activeInHierarchy) return;

        // Navegação com as setas ou o analógico
        float movimento = Input.GetAxis("Horizontal"); // Ou "Vertical" para navegação vertical

        if (movimento > 0.1f)
        {
            indexAtual = (indexAtual + 1) % botoes.Length;
            SelecionarBotao(botoes[indexAtual]);
        }
        else if (movimento < -0.1f)
        {
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
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(botao);
    }
}
