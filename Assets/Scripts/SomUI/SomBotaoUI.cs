using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Certifique-se de importar esse namespace
using SmallHedge.SomAmbiente;

[RequireComponent(typeof(Button))]
public class SomBotaoUI : MonoBehaviour
{
    public TipoSomAmbiente somHover = TipoSomAmbiente.BotaoHover;
    public TipoSomAmbiente somClick = TipoSomAmbiente.BotaoClick;
    private GerenciadorMusicaAmbiente gerenciadorSomUI;

    private void Start()
    {
        gerenciadorSomUI = FindObjectOfType<GerenciadorMusicaAmbiente>();  // Encontra o GerenciadorMusicaAmbiente na cena

        Button button = GetComponent<Button>();
        button.onClick.AddListener(TocarSomClick);  // Som ao clicar

        // Adiciona o EventTrigger para som de hover se ainda não estiver no GameObject
        EventTrigger trigger = GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = gameObject.AddComponent<EventTrigger>();
        }

        // Configura o evento de PointerEnter para tocar o som de hover
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        entry.callback.AddListener((data) => { TocarSomHover(); });
        trigger.triggers.Add(entry);
    }

    private void TocarSomHover()
    {
        gerenciadorSomUI.TocarSomUI(somHover);
    }

    private void TocarSomClick()
    {
        gerenciadorSomUI.TocarSomUI(somClick);
    }
}
