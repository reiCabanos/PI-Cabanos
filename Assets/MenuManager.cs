using UnityEngine;
using UnityEngine.UI;
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
    public Button botaoVoltar; // Botão de Voltar no painel de configurações

    [Header("Configurações de Som")]
    public Slider sliderVolumeGeral;
    public Slider sliderVolumeMusica;
    public Slider sliderVolumeVoz;
    public Slider sliderVolumeEfeitos;  // Novo slider para o volume dos efeitos sonoros
    public Button botaoMute;
    public Button botaoMuteEfeitos; // Novo botão para mutar os efeitos sonoros

    [Header("Configurações das Cenas")]
    public string cenaJogar;       // Nome da cena principal para jogar
    public string cenaMiniJogo;    // Nome da cena do Mini Jogo
    public string cenaCarregar;    // Nome da cena para carregar

    private GerenciadorMusicaAmbiente gerenciadorSom;
    private bool estaMutado = false;
    private bool efeitosMutados = false; // Estado de mute para efeitos sonoros

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
        botaoVoltar.onClick.AddListener(VoltarMenuPrincipal); // Botão Voltar

        botaoMute.onClick.AddListener(ToggleMute);
        botaoMuteEfeitos.onClick.AddListener(ToggleMuteEfeitos); // Botão de mute para efeitos sonoros

        // Ajustar sliders para volume
        sliderVolumeGeral.onValueChanged.AddListener(AjustarVolumeGeral);
        sliderVolumeMusica.onValueChanged.AddListener(AjustarVolumeMusica);
        sliderVolumeVoz.onValueChanged.AddListener(AjustarVolumeVoz);
        sliderVolumeEfeitos.onValueChanged.AddListener(AjustarVolumeEfeitos); // Slider de volume de efeitos

        // Configurar animação inicial dos menus
        painelMenuPrincipal.transform.localScale = Vector3.zero;
        painelConfiguracoes.SetActive(false);

        // Animação de entrada do menu principal
        painelMenuPrincipal.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
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
        });
    }

    private void SairJogo()
    {
        Application.Quit();
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
            gerenciadorSom.AlternarMuteEfeitos(efeitosMutados); // Chama a função para mutar os efeitos no Gerenciador de Som
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
            gerenciadorSom.DefinirVolumeEfeitos(volume); // Chama a função para ajustar o volume de efeitos no Gerenciador de Som
        }
    }

    public void VoltarMenuPrincipal()
    {
        painelConfiguracoes.transform.DOScale(0, 0.3f).OnComplete(() =>
        {
            painelConfiguracoes.SetActive(false);
            painelMenuPrincipal.SetActive(true);
            painelMenuPrincipal.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
        });
    }
}
