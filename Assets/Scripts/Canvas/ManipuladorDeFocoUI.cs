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
        public bool isPainelFixo; // Define se este é o painel fixo
    }

    public List<ConfiguracaoDeFocoPainel> configuracoesDeFocoPainel;
    private Selectable elementoFocoAtual;
    private GameObject painelAtual;
    private bool focoFixado = false;
    private bool focoNoPainelFixoPermitido = true; // Controla se o painel fixo pode receber o foco

    private void Start()
    {
        VerificarPainelAtivo();
    }

    private void Update()
    {
        // Verifica se o foco foi perdido e se foco fixo está ativo
        if (focoFixado && elementoFocoAtual != null && EventSystem.current.currentSelectedGameObject == null)
        {
            StartCoroutine(DefinirFocoComAtraso(elementoFocoAtual));
        }
    }

    public void FixarFocoNoPainelAtivo()
    {
        focoFixado = true;
        focoNoPainelFixoPermitido = true; // Permite foco no painel fixo
        VerificarPainelAtivo();
    }

    public void LiberarFocoPainel()
    {
        focoFixado = false;
        focoNoPainelFixoPermitido = false; // Impede foco no painel fixo
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void VerificarPainelAtivo()
    {
        foreach (var config in configuracoesDeFocoPainel)
        {
            // Evita focar no painel fixo se o foco no painel fixo não for permitido
            if (config.painel.activeSelf && (painelAtual != config.painel) &&
               (!config.isPainelFixo || focoNoPainelFixoPermitido))
            {
                painelAtual = config.painel;
                elementoFocoAtual = config.elementoFocoInicial;

                // Move o painel para o topo da hierarquia para garantir que está à frente
                painelAtual.transform.SetAsLastSibling();

                // Define o foco no elemento inicial do painel ativado
                StartCoroutine(DefinirFocoComAtraso(elementoFocoAtual));
                break;
            }
        }
    }

    private IEnumerator DefinirFocoComAtraso(Selectable elemento)
    {
        yield return null;

        if (elemento != null)
        {
            EventSystem.current.SetSelectedGameObject(elemento.gameObject); // Foca diretamente no elemento inicial desejado
        }
    }
}
