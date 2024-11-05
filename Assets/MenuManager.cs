using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using SmallHedge.SomAmbiente;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Header("Paineis")]
    public GameObject painelMenuPrincipal;
    public GameObject painelConfiguracoes;

    [Header("Botões")]
    public Button botaoJogar;
    public Button botaoMiniJogo;
    public Button botaoCarregar;
    public Button botaoConfiguracoes;
    public Button botaoSair;
    public Button botaoVoltar;

    [Header("Botões Padrão")]
    public Button botaoPadraoMenu;            // Botão inicial do Menu Principal
    public Button botaoPadraoConfiguracoes;   // Botão inicial das Configurações

    [Header("Configurações de Som")]
    public Slider sliderVolumeGeral;
    public Slider sliderVolumeMusica;
    public Slider sliderVolumeVoz;
    public Slider sliderVolumeEfeitos;
    public Button botaoMute;
    public Button botaoMuteEfeitos;

    [Header("Configurações das Cenas")]
    public string cenaJogar;
    public string cenaMiniJogo;
    public string cenaCarregar;

    [Header("Transição de Cena")]
    [SerializeField] RectTransform fader;
    [SerializeField] float transitionTime = 10f;
    [SerializeField] Slider transitionSlider;
    [SerializeField] TextMeshProUGUI sliderText;

    private GerenciadorMusicaAmbiente gerenciadorSom;
    private bool estaMutado = false;
    private bool efeitosMutados = false;
    private bool isTransitioning = false;

    private void Start()
    {
        // Obter referência ao Gerenciador de Som
        gerenciadorSom = FindObjectOfType<GerenciadorMusicaAmbiente>();

        // Inicializar os botões com suas funções correspondentes
        botaoJogar.onClick.AddListener(IniciarJogo);
        botaoMiniJogo.onClick.AddListener(CarregarMiniJogo);
        botaoCarregar.onClick.AddListener(CarregarCena);
        botaoConfiguracoes.onClick.AddListener(AbrirConfiguracoes);
        botaoSair.onClick.AddListener(SairJogo);
        botaoVoltar.onClick.AddListener(VoltarMenuPrincipal);

        botaoMute.onClick.AddListener(ToggleMute);
        botaoMuteEfeitos.onClick.AddListener(ToggleMuteEfeitos);

        // Ajustar sliders para volume
        sliderVolumeGeral.onValueChanged.AddListener(AjustarVolumeGeral);
        sliderVolumeMusica.onValueChanged.AddListener(AjustarVolumeMusica);
        sliderVolumeVoz.onValueChanged.AddListener(AjustarVolumeVoz);
        sliderVolumeEfeitos.onValueChanged.AddListener(AjustarVolumeEfeitos);

        // Configurar animação inicial do menu principal e definir o botão selecionado
        painelMenuPrincipal.transform.localScale = Vector3.zero;
        painelConfiguracoes.SetActive(false);
        painelMenuPrincipal.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
        SetarBotaoSelecionado(botaoPadraoMenu.gameObject); // Define o botão inicial do menu principal

        // Carregar preferências salvas
        CarregarPreferencias();

        // Configurar slider de transição
        transitionSlider.maxValue = 100;
        transitionSlider.value = 0;
        UpdateSliderText(0);
    }

    private void Update()
    {
        // Verifica se o EventSystem perdeu o foco e restaura o botão padrão do painel ativo
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            if (painelMenuPrincipal.activeSelf)
            {
                SetarBotaoSelecionado(botaoPadraoMenu.gameObject);
            }
            else if (painelConfiguracoes.activeSelf)
            {
                SetarBotaoSelecionado(botaoPadraoConfiguracoes.gameObject);
            }
        }
    }

    private void IniciarJogo()
    {
        if (!string.IsNullOrEmpty(cenaJogar))
        {
            OpenScene(cenaJogar, transitionTime);
        }
        else
        {
            Debug.LogWarning("Nome da cena para jogar está vazio!");
        }
    }

    private void CarregarMiniJogo()
    {
        if (!string.IsNullOrEmpty(cenaMiniJogo))
        {
            OpenScene(cenaMiniJogo, transitionTime);
        }
        else
        {
            Debug.LogWarning("Nome da cena do Mini Jogo está vazio!");
        }
    }

    private void CarregarCena()
    {
        if (!string.IsNullOrEmpty(cenaCarregar))
        {
            OpenScene(cenaCarregar, transitionTime);
        }
        else
        {
            Debug.LogWarning("Nome da cena para carregar está vazio!");
        }
    }

    private void OpenScene(string sceneName, float delay)
    {
        if (isTransitioning) return; // Evitar múltiplas ativações

        isTransitioning = true;
        fader.gameObject.SetActive(true);
        transitionSlider.gameObject.SetActive(true);

        StartCoroutine(UpdateSliderAndFader(delay));
        StartCoroutine(LoadSceneAsync(sceneName, delay));
    }

    private IEnumerator UpdateSliderAndFader(float delay)
    {
        float currentTime = 0f;

        while (currentTime < delay)
        {
            currentTime += Time.deltaTime;
            float progress = Mathf.Lerp(0, 100, currentTime / delay);
            transitionSlider.value = progress;
            UpdateSliderText(Mathf.CeilToInt(progress));
            yield return null;
        }

        transitionSlider.value = 0;
        UpdateSliderText(0);
        transitionSlider.gameObject.SetActive(false);
    }

    private void UpdateSliderText(int value)
    {
        if (sliderText != null)
        {
            sliderText.text = value.ToString();
        }
    }

    private IEnumerator LoadSceneAsync(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);

        // Carregar a cena de forma assíncrona
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        // Esperar até a cena estar completamente carregada
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                // Pequeno atraso antes de ativar a cena
                yield return new WaitForSeconds(1f);
                asyncLoad.allowSceneActivation = true; // Ativar a cena
            }
            yield return null;
        }

        // Animação de fechamento do fader
        yield return new WaitForSeconds(0.5f);
        LeanTween.scale(fader, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
        {
            fader.gameObject.SetActive(false);
            isTransitioning = false;
        });
    }

    private void AbrirConfiguracoes()
    {
        painelMenuPrincipal.transform.DOScale(0, 0.3f).OnComplete(() =>
        {
            painelMenuPrincipal.SetActive(false);
            painelConfiguracoes.SetActive(true);
            painelConfiguracoes.transform.localScale = Vector3.zero;
            painelConfiguracoes.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);

            // Define o botão inicial do painel de configurações
            SetarBotaoSelecionado(botaoPadraoConfiguracoes.gameObject);
        });
    }

    private void SairJogo()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
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
        gerenciadorSom.DefinirVolumeGeral(volume);
        SalvarPreferencias();
    }

    private void AjustarVolumeMusica(float volume)
    {
        gerenciadorSom.DefinirVolumeMusica(volume);
        SalvarPreferencias();
    }

    private void AjustarVolumeVoz(float volume)
    {
        gerenciadorSom.DefinirVolumeVoz(volume);
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

    public void VoltarMenuPrincipal()
    {
        painelConfiguracoes.transform.DOScale(0, 0.3f).OnComplete(() =>
        {
            painelConfiguracoes.SetActive(false);
            painelMenuPrincipal.SetActive(true);
            painelMenuPrincipal.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);

            // Define o botão inicial do menu principal
            SetarBotaoSelecionado(botaoPadraoMenu.gameObject);
        });
    }

    private void SetarBotaoSelecionado(GameObject botao)
    {
        EventSystem.current.SetSelectedGameObject(null); // Limpa o botão selecionado atual
        EventSystem.current.SetSelectedGameObject(botao); // Define o botão desejado como selecionado
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
}

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
