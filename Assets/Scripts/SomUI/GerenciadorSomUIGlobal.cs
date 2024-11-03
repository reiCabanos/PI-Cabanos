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
        AdicionarSonsAosBotoesIniciais();  // Aplica os sons aos bot�es que j� est�o ativos na cena
    }

    // Fun��o para adicionar sons aos bot�es que est�o ativos ao iniciar a cena
    private void AdicionarSonsAosBotoesIniciais()
    {
        Button[] botoes = FindObjectsOfType<Button>();  // Encontra todos os bot�es na cena
        foreach (Button botao in botoes)
        {
            if (botao.gameObject.activeInHierarchy)  // Apenas bot�es ativos
            {
                AdicionarEventoDeSom(botao);
            }
        }
    }

    // Fun��o chamada automaticamente quando este objeto � ativado na cena
    private void OnEnable()
    {
        Button[] botoes = FindObjectsOfType<Button>();  // Encontra todos os bot�es na cena

        foreach (Button botao in botoes)
        {
            // Detecta e configura sons apenas para bot�es ativados recentemente
            if (botao.gameObject.activeInHierarchy && !botoesComSom.Contains(botao))
            {
                AdicionarEventoDeSom(botao);
            }
        }
    }

    // Fun��o p�blica que adiciona eventos de som a um bot�o espec�fico
    private void AdicionarEventoDeSom(Button botao)
    {
        if (botoesComSom.Contains(botao)) return;

        botao.onClick.AddListener(() => TocarSomClick());

        // Adiciona EventTrigger para hover e selected, se n�o estiver presente
        EventTrigger trigger = botao.GetComponent<EventTrigger>() ?? botao.gameObject.AddComponent<EventTrigger>();

        // Configura o evento de PointerEnter para tocar o som de hover
        EventTrigger.Entry hoverEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        hoverEntry.callback.AddListener((data) => TocarSomHover());
        trigger.triggers.Add(hoverEntry);

        // Configura o evento de Select para tocar o som de selected
        EventTrigger.Entry selectedEntry = new EventTrigger.Entry { eventID = EventTriggerType.Select };
        selectedEntry.callback.AddListener((data) => TocarSomSelected());
        trigger.triggers.Add(selectedEntry);

        botoesComSom.Add(botao); // Registra o bot�o para evitar duplicidade
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
