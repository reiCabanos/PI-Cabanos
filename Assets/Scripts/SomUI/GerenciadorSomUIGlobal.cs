using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SmallHedge.SomAmbiente;
using System.Collections.Generic;

public class GerenciadorSomUIGlobal : MonoBehaviour
{
    public TipoSomAmbiente somHover = TipoSomAmbiente.BotaoHover;
    public TipoSomAmbiente somClick = TipoSomAmbiente.BotaoClick;
    public TipoSomAmbiente somSelected = TipoSomAmbiente.BotaoSelected; // Novo som para Selected
    private GerenciadorMusicaAmbiente gerenciadorSomUI;
    private HashSet<Button> botoesComSom = new HashSet<Button>(); // Usa HashSet para evitar duplicidade

    private void Start()
    {
        gerenciadorSomUI = FindObjectOfType<GerenciadorMusicaAmbiente>();  // Encontra o GerenciadorMusicaAmbiente na cena
        AdicionarSonsAosBotoesIniciais();  // Aplica os sons aos botões que já estão ativos na cena
    }

    // Função para adicionar sons aos botões que estão ativos ao iniciar a cena
    private void AdicionarSonsAosBotoesIniciais()
    {
        Button[] botoes = FindObjectsOfType<Button>();  // Encontra todos os botões na cena
        foreach (Button botao in botoes)
        {
            if (botao.gameObject.activeInHierarchy)  // Apenas botões ativos
            {
                AdicionarEventoDeSom(botao);
            }
        }
    }

    // Função chamada automaticamente quando este objeto é ativado na cena
    private void OnEnable()
    {
        Button[] botoes = FindObjectsOfType<Button>();  // Encontra todos os botões na cena

        foreach (Button botao in botoes)
        {
            // Detecta e configura sons apenas para botões ativados recentemente
            if (botao.gameObject.activeInHierarchy && !botoesComSom.Contains(botao))
            {
                AdicionarEventoDeSom(botao);
            }
        }
    }

    // Função pública que adiciona eventos de som a um botão específico
    private void AdicionarEventoDeSom(Button botao)
    {
        if (botoesComSom.Contains(botao)) return;

        botao.onClick.AddListener(() => TocarSomClick());

        // Adiciona EventTrigger para hover e selected, se não estiver presente
        EventTrigger trigger = botao.GetComponent<EventTrigger>() ?? botao.gameObject.AddComponent<EventTrigger>();

        // Configura o evento de PointerEnter para tocar o som de hover
        EventTrigger.Entry hoverEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        hoverEntry.callback.AddListener((data) => TocarSomHover());
        trigger.triggers.Add(hoverEntry);

        // Configura o evento de Select para tocar o som de selected
        EventTrigger.Entry selectedEntry = new EventTrigger.Entry { eventID = EventTriggerType.Select };
        selectedEntry.callback.AddListener((data) => TocarSomSelected());
        trigger.triggers.Add(selectedEntry);

        botoesComSom.Add(botao); // Registra o botão para evitar duplicidade
    }

    private void TocarSomHover()
    {
        gerenciadorSomUI.TocarSomUI(somHover);
    }

    private void TocarSomClick()
    {
        gerenciadorSomUI.TocarSomUI(somClick);
    }

    private void TocarSomSelected()
    {
        gerenciadorSomUI.TocarSomUI(somSelected);
    }
}
