using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SmallHedge.SomAmbiente;

public class GerenciadorSomUIGlobal : MonoBehaviour
{
    public TipoSomAmbiente somHover = TipoSomAmbiente.BotaoHover;
    public TipoSomAmbiente somClick = TipoSomAmbiente.BotaoClick;
    public TipoSomAmbiente somSelected = TipoSomAmbiente.BotaoSelected; // Novo som para Selected
    private GerenciadorMusicaAmbiente gerenciadorSomUI;

    private void Start()
    {
        gerenciadorSomUI = FindObjectOfType<GerenciadorMusicaAmbiente>();  // Encontra o GerenciadorMusicaAmbiente na cena
        AdicionarSonsAosBotoes();  // Aplica os sons a todos os botões
    }

    // Função para adicionar sons de hover, clique e selected a todos os botões da cena
    private void AdicionarSonsAosBotoes()
    {
        Button[] botoes = FindObjectsOfType<Button>();  // Encontra todos os botões na cena

        foreach (Button botao in botoes)
        {
            // Adiciona evento de clique ao botão
            botao.onClick.AddListener(() => TocarSomClick());

            // Verifica e adiciona EventTrigger para hover e selected, se não estiver presente
            EventTrigger trigger = botao.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = botao.gameObject.AddComponent<EventTrigger>();
            }

            // Configura o evento de PointerEnter para tocar o som de hover
            EventTrigger.Entry hoverEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
            hoverEntry.callback.AddListener((data) => TocarSomHover());
            trigger.triggers.Add(hoverEntry);

            // Configura o evento de Selected para tocar o som de selected
            EventTrigger.Entry selectedEntry = new EventTrigger.Entry { eventID = EventTriggerType.Select };
            selectedEntry.callback.AddListener((data) => TocarSomSelected());
            trigger.triggers.Add(selectedEntry);
        }
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
