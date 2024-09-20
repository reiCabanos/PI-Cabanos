using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.EventSystems;

public class HudControles : MonoBehaviour
{
    public Transform _telaIniciar;
    public Transform _telaHuds;
    public Transform _telaCelular;
    public Transform _telaInventario;
    public Transform _painelBluer;
    public Transform _painelConfig;

    private Transform _painelAtivo;
    private bool painelBloqueando = false; // Variável para controlar o bloqueio
    private float moveDistance = 35f; // Distância do movimento lateral
    private float moveDuration = 4f;  // Duração de cada movimento
    private Tweener moveTween;
    public bool blockMovement = true; // Variável para controlar o bloqueio do movimento
    public GameObject _panelSliderMenu;


    public Transform MiniMap;
    public Transform _painelNovo; // Novo painel a ser exibido h
    public float tempoExibicaoPainel = 5f; // Tempo de exibição do painel (ajustável no Inspetor)
    private bool painelNovoExibido = false; // Controla se o painel já foi exibido
    public float tempoExibicaoPainelNovo = 10f; // Tempo em segundos para ocultar o painel (ajuste no Inspetor)

    public Button botaoCelular; // Variável pública para seta o botão do painel _telaCelular 
    public SideMenuController _siderMenu;
    public Button botaoFecharPainelNovo;
    //



    void Awake()
    {
        blockMovement = true; // Define blockMovement como true no Awake
                              // ... (resto do seu código Start) ...
    }


    void Start()
    {
        _painelAtivo = _telaIniciar;
        _telaIniciar.gameObject.SetActive(true);
        _telaHuds.gameObject.SetActive(false);
        _telaCelular.gameObject.SetActive(false);
        _telaInventario.gameObject.SetActive(false);
        _painelConfig.gameObject.SetActive(false);
        // Adiciona a animação DOTween ao _telaIniciar
        moveTween = _telaIniciar.DOLocalMoveX(moveDistance, moveDuration)
                              .SetEase(Ease.InOutSine)
                              .SetLoops(-1, LoopType.Yoyo); // Repete infinitamente, indo e voltando
                                                            // Associa o botão fechar com o método FecharPainel
        if (botaoFecharPainelNovo != null)
        {
            botaoFecharPainelNovo.onClick.AddListener(() => FecharPainel(_painelNovo));
        }
    }

    void Update()
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector2(Screen.width / 2f, Screen.height / 2f);
    }

    public void SetComecar(InputAction.CallbackContext value)
    {
        if (value.performed && _painelAtivo == _telaIniciar)
        {
            FecharPainel(_telaIniciar);
            AbrirPainel(_telaHuds);
            blockMovement = false;
            _panelSliderMenu.gameObject.SetActive(true);
            // Inicia a corrotina para exibir o painel novo
            StartCoroutine(ExibirPainelNovo());
        }
    }

    private IEnumerator ExibirPainelNovo()
    {
        yield return new WaitForSeconds(tempoExibicaoPainel); // Aguarda o tempo definido


        if (!painelNovoExibido) // Verifica se o painel ainda não foi exibido
        {
            AbrirPainel(_painelNovo);
            painelNovoExibido = true; // Marca o painel como exibido

            // Inicia a corrotina para ocultar o painel após o tempo definido
            StartCoroutine(OcultarPainelNovo());
        }
    }

    private IEnumerator OcultarPainelNovo()
    {
        yield return new WaitForSeconds(tempoExibicaoPainelNovo); // Aguarda o tempo de exibição

        if (painelNovoExibido) // Verifica se o painel está sendo exibido
        {
            FecharPainel(_painelNovo);
            painelNovoExibido = false;
        }
    }

    public void SetInventario(InputAction.CallbackContext value)
    {
        if (value.performed) AlternarPainel(_telaInventario);
        _siderMenu.MenuShow();



    }

    public void SetCelular(InputAction.CallbackContext value)
    {
        if (value.performed) AlternarPainel(_telaCelular);
    }

    public void SetSair(InputAction.CallbackContext value)
    {
        if (value.performed)
            if (_painelAtivo == _painelNovo)
            {
                // Fecha o painel novo
                FecharPainel(_painelNovo);
            }
        {
            if (_painelAtivo == _telaCelular)
            {
                FecharPainel(_painelAtivo);
                AbrirPainel(_telaHuds);
                

            }
            else if (_painelAtivo != _telaHuds)
            {
                FecharPainel(_painelAtivo);
                AbrirPainel(_telaHuds);
            }
            else
            {
                // Lógica para sair do jogo (por exemplo, SceneManager.LoadScene("MenuPrincipal");)
            }
        }
    }

    // ... (Outros métodos para lidar com outras ações de input)

    private void AbrirPainel(Transform painel)
    {
        painel.gameObject.SetActive(true);

        // Lógica específica para o paneltutorial
        if (painel == _painelNovo)
        {
            painel.DOScale(1, 1.1f); // Aparece com escala 1.1
            MiniMap.DOScale(0, 0.0f); // Esconde o MiniMap
        }
        else
        {
            painel.DOScale(1, 1f); // Outros painéis abrem com escala 1
        }

        _painelAtivo = painel;

        if (painel != _telaIniciar && painel != _telaHuds)
        {
            _painelBluer.gameObject.SetActive(true);
            _painelBluer.DOScale(1, 0f);
            painelBloqueando = true; // Bloqueia outros painéis
        }

        if (painel == _telaIniciar || painel == _telaCelular || painel == _telaInventario || painel == _painelConfig || painel  == _painelNovo)
        {
            blockMovement = true; // Define blockMovement como true aqui
        }
        else
        {
            _painelBluer.DOScale(0, 0.0f).OnComplete(() => _painelBluer.gameObject.SetActive(false));
            painelBloqueando = false; // Desbloqueia outros painéis ao abrir _telaHuds
        }

        if (painel == _telaCelular)
        {
            // Se botaoCelular estiver definido, seleciona ele; caso contrário, seleciona o primeiro botão encontrado
            EventSystem.current.SetSelectedGameObject(botaoCelular != null ? botaoCelular.gameObject : _telaCelular.GetComponentInChildren<Button>()?.gameObject);
        }
    }

    private void FecharPainel(Transform painel)
    {
        // Lógica específica para o paneltutorial
        if (painel == _painelNovo)
        {
            painel.DOScale(0, 0.0f).OnComplete(() => painel.gameObject.SetActive(false)); // Desaparece com escala 0
            MiniMap.DOScale(1, 1.1f); // Mostra o MiniMap novamente
            _painelBluer.DOScale(0, 0.0f);

        }
        else
        {
            painel.DOScale(0, 0.2f).OnComplete(() => painel.gameObject.SetActive(false)); // Outros painéis fecham com escala 0
        }

        if (_telaHuds.gameObject.activeSelf == false &&
            _telaCelular.gameObject.activeSelf == false &&
            _telaInventario.gameObject.activeSelf == false &&
            _painelConfig.gameObject.activeSelf == false)
        {
            _painelBluer.DOScale(0, 0.0f).OnComplete(() => _painelBluer.gameObject.SetActive(false));
        }

        blockMovement = false; // Desbloqueia o movimento ao fechar qualquer painel

        // Reinicia o controle se o painel paneltutorial for fechado
        if (painel == _painelNovo)
        {
            painelNovoExibido = false;

        }

    }


    public void AlternarPainel(Transform painel)
    {
        if (painelBloqueando && painel != _telaHuds)
        {
            //sreturn; // Impede a abertura de outros painéis se um estiver bloqueando
        }

        if (_painelAtivo == painel)
        {
            FecharPainel(painel);
            AbrirPainel(_telaHuds);
            painelBloqueando = false; // Desbloqueia ao fechar o painel atual
        }
        else
        {
            FecharPainel(_painelAtivo);
            AbrirPainel(painel);

            // Define se o painel bloqueia outros (exceto _telaHuds)
            painelBloqueando = (painel == _telaCelular || painel == _telaInventario || painel == _painelConfig);
        }
    }
}
