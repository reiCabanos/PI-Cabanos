using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BotoesFimJogo : MonoBehaviour
{
    public GameObject painel; // O painel ao qual este script pertence
    public GameObject botaoInicial; // O bot�o que deve ser selecionado inicialmente
    public GameObject[] botoes; // Lista de bot�es para navega��o
    private int indexAtual = 0;

    private void OnEnable()
    {
        // Inicializa o bot�o inicial como selecionado
        if (painel.activeInHierarchy)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(botaoInicial);
            indexAtual = 0;
        }
    }

    private void Update()
    {
        // Verifica se o painel est� ativo antes de permitir a navega��o
        if (!painel.activeInHierarchy) return;

        // Navega��o com as setas ou o anal�gico
        float movimento = Input.GetAxis("Horizontal"); // Ou "Vertical" para navega��o vertical

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

        // Garante que um bot�o esteja sempre selecionado
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
