using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SmallHedge.SomAmbiente;
using DG.Tweening;

public class MenuConfigPrefab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [System.Serializable]
    public class Preferencias
    {
        public float volumeGeral;
        public float volumeMusica;
        public float volumeVoz;
        public float volumeEfeitos;
        public bool estaMutado;
        public bool efeitosMutados;
    }

    [Header("Configurações fehca")]
    public Button botaoFecharConfiguracoes; // Botão para fechar o painel de configurações
    private HudControles hudControles; // Referência ao script HudControles para controlar o painel

    [Header("Configurações de Animação")]
    [SerializeField] private float hoverScale = 1.1f;
    [SerializeField] private float hoverDuration = 0.2f;
    [SerializeField] private float clickScale = 0.95f;
    [SerializeField] private float clickDuration = 0.1f;

    [Header("Painel de Pré-Menu")]
    public GameObject painelPreMenu;
    public Button botaoVoltarMenuPrincipal;
    public Button botaoSairJogo;
    public Button botaoConfiguracoes;
    public Button botaoTutorial;
    public Button botaoSalvarJogo;
    public Button botaoFocoPreMenu; // Botão para focar ao abrir o Pré-Menu

    [Header("Painel de Configuração")]
    public GameObject painelConfiguracoes;
    public Button botaoVoltarConfiguracoes;
    public Button botaoFocoConfiguracoes; // Botão para focar ao abrir o Painel de Configurações

    [Header("Botões de Configuração")]
    public Button botaoMute;
    public Button botaoMuteEfeitos;

    [Header("Sliders de Volume")]
    public Slider sliderVolumeGeral;
    public Slider sliderVolumeMusica;
    public Slider sliderVolumeVoz;
    public Slider sliderVolumeEfeitos;

    [Header("Painel de Tutorial")]
    public GameObject painelTutorial;
    public Button botaoFecharTutorial;
    public Button botaoFocoTutorial; // Botão para focar ao abrir o Painel de Tutorial

    private GerenciadorMusicaAmbiente gerenciadorSom;
    private bool estaMutado = false;
    private bool efeitosMutados = false;

    [ Header("Slider") ]
    public GameObject _slider;
    public RectTransform _painelSlider;

    private void Start()
    {
        // Buscar o GameObject do Canvas e localizar o componente HudControles
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            hudControles = canvas.GetComponent<HudControles>();
        }

        // Configurar o botão para fechar o painel de configurações usando a lógica do HudControles
        if (botaoFecharConfiguracoes != null && hudControles != null)
        {
            botaoFecharConfiguracoes.onClick.AddListener(FecharPainelConfig);  
        }

        ConfigurarBotoes();

        // Obter referência ao Gerenciador de Som
        gerenciadorSom = FindObjectOfType<GerenciadorMusicaAmbiente>();

        // Carregar preferências salvas
        CarregarPreferencias();

        // Desativar slider 
        

    }

    private void AbrirPainel(GameObject painel)
    {
        painelPreMenu.transform.DOScale(0, 0.3f).OnComplete(() =>
        {
            painelPreMenu.SetActive(false);
            painel.SetActive(true);
            painel.transform.localScale = Vector3.zero;
            painel.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);

            // Define o foco no botão configurado para o painel que foi aberto
            if (painel == painelPreMenu && botaoFocoPreMenu != null)
            {
                EventSystem.current.SetSelectedGameObject(botaoFocoPreMenu.gameObject);
            }
            else if (painel == painelConfiguracoes && botaoFocoConfiguracoes != null)
            {
                EventSystem.current.SetSelectedGameObject(botaoFocoConfiguracoes.gameObject);

                // Atualizar o painel ativo e _painelBluer no HudControles
                if (hudControles != null)
                {
                    hudControles._painelAtivo = painelConfiguracoes.transform;
                    hudControles.painelBloqueando = true;
                    hudControles._painelBluer.gameObject.SetActive(true);
                    hudControles._painelBluer.DOScale(1, 0f);
                }
            }
            else if (painel == painelTutorial && botaoFocoTutorial != null)
            {
                EventSystem.current.SetSelectedGameObject(botaoFocoTutorial.gameObject);

                // Atualizar o painel ativo e _painelBluer no HudControles
                if (hudControles != null)
                {
                    hudControles._painelAtivo = painelTutorial.transform;
                    hudControles.painelBloqueando = true;
                    hudControles._painelBluer.gameObject.SetActive(true);
                    hudControles._painelBluer.DOScale(1, 0f);
                }
            }
            // Desativar slider 
            _slider.SetActive(false);
        });
    }

    private void FecharPainel(GameObject painel)
    {
        painel.transform.DOScale(0, 0.3f).OnComplete(() =>
        {
            painel.SetActive(false);
            painelPreMenu.SetActive(true);
            painelPreMenu.transform.localScale = Vector3.zero;
            painelPreMenu.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);

            // Foca no botão configurado ao retornar para o painel pré-menu
            if (botaoFocoPreMenu != null)
            {
                EventSystem.current.SetSelectedGameObject(botaoFocoPreMenu.gameObject);
            }

            // Limpar o painel ativo e _painelBluer no HudControles
            if (hudControles != null && hudControles._painelAtivo == painel.transform)
            {
                hudControles._painelAtivo = null;
                hudControles.painelBloqueando = false;

                // Desativar _painelBluer quando todos os painéis estão fechados
                if (!painelPreMenu.activeSelf && !painelConfiguracoes.activeSelf && !painelTutorial.activeSelf)
                {
                    hudControles._painelBluer.DOScale(0, 0.0f).OnComplete(() => hudControles._painelBluer.gameObject.SetActive(false));
                }
            }
        });
    }

    private void FecharPainelConfig()
    {
        if (hudControles != null)
        {
            // Fecha o painel de configurações usando a lógica do HudControles
            hudControles.FecharPainel(hudControles._painelConfig);

            // Reseta o estado do painel para permitir a abertura de outros painéis
            hudControles._painelAtivo = null;
            hudControles.painelBloqueando = false;

            // Prepara o painel de configurações para ser reaberto corretamente
            hudControles._painelConfig.localScale = Vector3.one;
            hudControles._painelConfig.gameObject.SetActive(false);

            // Desativar o _painelBluer ao fechar o painel de configurações
            hudControles._painelBluer.DOScale(0, 0.0f).OnComplete(() => hudControles._painelBluer.gameObject.SetActive(false));
            // Ativar slider
            _slider.SetActive(true);
            _painelSlider.DOAnchorPos(new Vector2(-295f, _painelSlider.anchoredPosition.y), 0.5f).SetEase(Ease.InOutCubic);
        }
    }

    private void ConfigurarBotoes()
    {
        ConfigurarBotao(botaoVoltarMenuPrincipal, VoltarMenuPrincipal);
        ConfigurarBotao(botaoSairJogo, ConfirmarSairJogo);
        ConfigurarBotao(botaoConfiguracoes, () => AbrirPainel(painelConfiguracoes));
        ConfigurarBotao(botaoTutorial, () => AbrirPainel(painelTutorial));
        ConfigurarBotao(botaoSalvarJogo, SalvarJogo);
        ConfigurarBotao(botaoVoltarConfiguracoes, () => FecharPainel(painelConfiguracoes));
        ConfigurarBotao(botaoFecharTutorial, () => FecharPainel(painelTutorial));
        ConfigurarBotao(botaoMute, ToggleMute);
        ConfigurarBotao(botaoMuteEfeitos, ToggleMuteEfeitos);

        ConfigurarSlider(sliderVolumeGeral, AjustarVolumeGeral);
        ConfigurarSlider(sliderVolumeMusica, AjustarVolumeMusica);
        ConfigurarSlider(sliderVolumeVoz, AjustarVolumeVoz);
        ConfigurarSlider(sliderVolumeEfeitos, AjustarVolumeEfeitos);
    }

    private void ConfigurarBotao(Button botao, UnityEngine.Events.UnityAction acao)
    {
        if (botao != null)
        {
            botao.onClick.RemoveAllListeners();
            botao.onClick.AddListener(acao);

            EventTrigger trigger = botao.gameObject.GetComponent<EventTrigger>();
            if (trigger == null)
                trigger = botao.gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry entryEnter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
            entryEnter.callback.AddListener((data) => { OnPointerEnter(botao); });
            trigger.triggers.Add(entryEnter);

            EventTrigger.Entry entryExit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
            entryExit.callback.AddListener((data) => { OnPointerExit(botao); });
            trigger.triggers.Add(entryExit);

            EventTrigger.Entry entryDown = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
            entryDown.callback.AddListener((data) => { OnPointerDown(botao); });
            trigger.triggers.Add(entryDown);

            EventTrigger.Entry entryUp = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
            entryUp.callback.AddListener((data) => { OnPointerUp(botao); });
            trigger.triggers.Add(entryUp);
        }
    }

    private void ConfigurarSlider(Slider slider, UnityEngine.Events.UnityAction<float> acao)
    {
        if (slider != null)
        {
            slider.onValueChanged.RemoveAllListeners();
            slider.onValueChanged.AddListener(acao);
        }
    }

    private void VoltarMenuPrincipal()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    private void ConfirmarSairJogo()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void SalvarJogo()
    {
        Debug.Log("Funcionalidade de salvar jogo ainda não implementada.");
    }

    private void ToggleMute()
    {
        if (gerenciadorSom != null)
        {
            gerenciadorSom.AlternarMute();
            estaMutado = !estaMutado;
            AtualizarBotaoMute();
            SalvarPreferencias();
        }
    }

    private void ToggleMuteEfeitos()
    {
        if (gerenciadorSom != null)
        {
            efeitosMutados = !efeitosMutados;
            gerenciadorSom.AlternarMuteEfeitos(efeitosMutados);
            AtualizarBotaoMuteEfeitos();
            SalvarPreferencias();
        }
    }

    private void AtualizarBotaoMute()
    {
        botaoMute.GetComponentInChildren<Text>().text = estaMutado ? "Desmutar" : "Mutar";
    }

    private void AtualizarBotaoMuteEfeitos()
    {
        botaoMuteEfeitos.GetComponentInChildren<Text>().text = efeitosMutados ? "Desmutar Efeitos" : "Mutar Efeitos";
    }

    private void AjustarVolumeGeral(float volume)
    {
        if (gerenciadorSom != null)
        {
            gerenciadorSom.DefinirVolumeGeral(volume);
        }
        SalvarPreferencias();
    }

    private void AjustarVolumeMusica(float volume)
    {
        if (gerenciadorSom != null)
        {
            gerenciadorSom.DefinirVolumeMusica(volume);
        }
        SalvarPreferencias();
    }

    private void AjustarVolumeVoz(float volume)
    {
        if (gerenciadorSom != null)
        {
            gerenciadorSom.DefinirVolumeVoz(volume);
        }
        SalvarPreferencias();
    }

    private void AjustarVolumeEfeitos(float volume)
    {
        if (gerenciadorSom != null)
        {
            gerenciadorSom.DefinirVolumeEfeitos(volume);
            SalvarPreferencias();
        }
    }

    private void SalvarPreferencias()
    {
        Preferencias preferencias = new Preferencias()
        {
            volumeGeral = sliderVolumeGeral.value,
            volumeMusica = sliderVolumeMusica.value,
            volumeVoz = sliderVolumeVoz.value,
            volumeEfeitos = sliderVolumeEfeitos.value,
            estaMutado = estaMutado,
            efeitosMutados = efeitosMutados
        };

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/preferencias.dat");
        bf.Serialize(file, preferencias);
        file.Close();
    }

    private void CarregarPreferencias()
    {
        string path = Application.persistentDataPath + "/preferencias.dat";
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            Preferencias preferencias = (Preferencias)bf.Deserialize(file);
            file.Close();

            sliderVolumeGeral.value = preferencias.volumeGeral;
            sliderVolumeMusica.value = preferencias.volumeMusica;
            sliderVolumeVoz.value = preferencias.volumeVoz;
            sliderVolumeEfeitos.value = preferencias.volumeEfeitos;
            estaMutado = preferencias.estaMutado;
            efeitosMutados = preferencias.efeitosMutados;

            AtualizarBotaoMute();
            AtualizarBotaoMuteEfeitos();
        }
    }

    public void OnPointerEnter(Component botao)
    {
        botao.transform.DOScale(hoverScale, hoverDuration).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(Component botao)
    {
        botao.transform.DOScale(1f, hoverDuration).SetEase(Ease.OutBack);
    }

    public void OnPointerDown(Component botao)
    {
        botao.transform.DOScale(clickScale, clickDuration).SetEase(Ease.OutBack);
    }

    public void OnPointerUp(Component botao)
    {
        botao.transform.DOScale(hoverScale, clickDuration).SetEase(Ease.OutBack);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.GetComponent<Button>() != null)
        {
            OnPointerEnter(eventData.pointerCurrentRaycast.gameObject.GetComponent<Button>());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.GetComponent<Button>() != null)
        {
            OnPointerExit(eventData.pointerCurrentRaycast.gameObject.GetComponent<Button>());
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.GetComponent<Button>() != null)
        {
            OnPointerDown(eventData.pointerCurrentRaycast.gameObject.GetComponent<Button>());
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.GetComponent<Button>() != null)
        {
            OnPointerUp(eventData.pointerCurrentRaycast.gameObject.GetComponent<Button>());
        }
    }
}
