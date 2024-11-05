using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SmallHedge.SomAmbiente;
using System.Collections;
using System.Collections.Generic;

public class GerenciadorSomUIGlobal : MonoBehaviour
{
    public TipoSomAmbiente somHover = TipoSomAmbiente.BotaoHover;
    public TipoSomAmbiente somClick = TipoSomAmbiente.BotaoClick;
    public TipoSomAmbiente somSelected = TipoSomAmbiente.BotaoSelected;
    private GerenciadorMusicaAmbiente gerenciadorSomUI;
    private HashSet<Button> botoesComSom = new HashSet<Button>();
    private HashSet<Slider> slidersComSom = new HashSet<Slider>();

    private void Start()
    {
        gerenciadorSomUI = FindObjectOfType<GerenciadorMusicaAmbiente>();
        AdicionarSonsAosElementosIniciais();
        StartCoroutine(VerificarElementosAtivadosPeriodicamente());
    }

    private void AdicionarSonsAosElementosIniciais()
    {
        Button[] botoes = FindObjectsOfType<Button>();
        foreach (Button botao in botoes)
        {
            if (botao.gameObject.activeInHierarchy)
            {
                AdicionarEventoDeSom(botao);
            }
        }

        Slider[] sliders = FindObjectsOfType<Slider>();
        foreach (Slider slider in sliders)
        {
            if (slider.gameObject.activeInHierarchy)
            {
                AdicionarEventoDeSom(slider);
            }
        }
    }

    private IEnumerator VerificarElementosAtivadosPeriodicamente()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            VerificarNovosElementos();
        }
    }

    private void VerificarNovosElementos()
    {
        Button[] botoes = FindObjectsOfType<Button>();
        foreach (Button botao in botoes)
        {
            if (botao.gameObject.activeInHierarchy && !botoesComSom.Contains(botao))
            {
                AdicionarEventoDeSom(botao);
            }
        }

        Slider[] sliders = FindObjectsOfType<Slider>();
        foreach (Slider slider in sliders)
        {
            if (slider.gameObject.activeInHierarchy && !slidersComSom.Contains(slider))
            {
                AdicionarEventoDeSom(slider);
            }
        }
    }

    private void AdicionarEventoDeSom(Button botao)
    {
        if (botoesComSom.Contains(botao)) return;

        botao.onClick.AddListener(() => TocarSomClick());

        EventTrigger trigger = botao.GetComponent<EventTrigger>() ?? botao.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry hoverEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        hoverEntry.callback.AddListener((data) => TocarSomHover());
        trigger.triggers.Add(hoverEntry);

        EventTrigger.Entry selectedEntry = new EventTrigger.Entry { eventID = EventTriggerType.Select };
        selectedEntry.callback.AddListener((data) => TocarSomSelected());
        trigger.triggers.Add(selectedEntry);

        botoesComSom.Add(botao);
    }

    private void AdicionarEventoDeSom(Slider slider)
    {
        // Ignora sliders de carregamento, verificando o nome do objeto
        if (slidersComSom.Contains(slider) || slider.name.Contains("Loading"))
            return;

        slider.onValueChanged.AddListener((value) => TocarSomClick());

        EventTrigger trigger = slider.GetComponent<EventTrigger>() ?? slider.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry hoverEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        hoverEntry.callback.AddListener((data) => TocarSomHover());
        trigger.triggers.Add(hoverEntry);

        EventTrigger.Entry selectedEntry = new EventTrigger.Entry { eventID = EventTriggerType.Select };
        selectedEntry.callback.AddListener((data) => TocarSomHover());
        trigger.triggers.Add(selectedEntry);

        slidersComSom.Add(slider);
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
