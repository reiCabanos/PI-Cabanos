using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using SmallHedge.SomAmbiente;
using UnityEngine.SceneManagement;

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

    private GerenciadorMusicaAmbiente gerenciadorSom;
    private bool estaMutado = false;
    private bool efeitosMutados = false;

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
            SceneManager.LoadScene(cenaJogar);
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
            SceneManager.LoadScene(cenaMiniJogo);
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
            SceneManager.LoadScene(cenaCarregar);
        }
        else
        {
            Debug.LogWarning("Nome da cena para carregar está vazio!");
        }
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
        }
    }

    private void ToggleMuteEfeitos()
    {
        if (gerenciadorSom != null)
        {
            efeitosMutados = !efeitosMutados;
            gerenciadorSom.AlternarMuteEfeitos(efeitosMutados);
            AtualizarBotaoMuteEfeitos();
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
    }

    private void AjustarVolumeMusica(float volume)
    {
        gerenciadorSom.DefinirVolumeMusica(volume);
    }

    private void AjustarVolumeVoz(float volume)
    {
        gerenciadorSom.DefinirVolumeVoz(volume);
    }

    private void AjustarVolumeEfeitos(float volume)
    {
        if (gerenciadorSom != null)
        {
            gerenciadorSom.DefinirVolumeEfeitos(volume);
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
}
