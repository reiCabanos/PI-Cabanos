using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class ManipuladorDeFocoUI : MonoBehaviour
{
    [System.Serializable]
    public class ConfiguracaoDeFocoPainel
    {
        public GameObject painel; // Painel a ser monitorado
        public Selectable elementoFocoInicial; // Elemento inicial a ser focado (botão ou slider)
    }

    public List<ConfiguracaoDeFocoPainel> configuracoesDeFocoPainel; // Lista de painéis e elementos de foco inicial
    private Selectable elementoFocoAtual; // Armazena o elemento atualmente focado
    private GameObject painelAtual; // Armazena o painel que está atualmente ativo

    private void Start()
    {
        // Inicializa com o elemento de foco do painel ativo, se houver
        VerificarPainelAtivo();
    }

    private void Update()
    {
        // Verifica se o foco foi perdido apenas se não houver nenhum elemento selecionado
        if (elementoFocoAtual != null && EventSystem.current.currentSelectedGameObject == null)
        {
            StartCoroutine(DefinirFocoComAtraso(elementoFocoAtual));
        }

        // Verifica se houve mudança no painel ativo e ajusta o foco conforme necessário
        VerificarPainelAtivo();
    }

    private void VerificarPainelAtivo()
    {
        foreach (var config in configuracoesDeFocoPainel)
        {
            // Se o painel foi ativado e é diferente do painel atual, atualiza o foco inicial
            if (config.painel.activeSelf && painelAtual != config.painel)
            {
                painelAtual = config.painel;
                elementoFocoAtual = config.elementoFocoInicial;

                // Mover o painel para o topo da hierarquia para garantir que ele está à frente
                painelAtual.transform.SetAsLastSibling();

                // Define o foco apenas no elemento inicial do painel ativado
                StartCoroutine(DefinirFocoComAtraso(elementoFocoAtual));
                break;
            }
        }
    }

    private IEnumerator DefinirFocoComAtraso(Selectable elemento)
    {
        yield return null; // Aguarda um quadro para garantir que o foco seja aplicado após a ativação

        if (elemento != null)
        {
            EventSystem.current.SetSelectedGameObject(null); // Limpa o foco atual
            EventSystem.current.SetSelectedGameObject(elemento.gameObject); // Foca no elemento inicial desejado
        }
    }
}
