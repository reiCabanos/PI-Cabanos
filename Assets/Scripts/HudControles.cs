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
    public Transform _painelDialogo; // Novo painel de diálogo

    public Transform _painelAtivo;
    public bool painelBloqueando = false;
    private float moveDistance = 35f;
    private float moveDuration = 4f;
    private Tweener moveTween;
    public bool blockMovement = true;
    public GameObject _panelSliderMenu;

    public Transform MiniMap;
    public float tempoExibicaoPainel = 5f;

    public Button botaoCelular;
    public bool _telaDiaIni;

    // Variáveis para controle do painel lateral (animação)
    public RectTransform PanelMenu;  // O painel lateral que será movido
    private bool isMenuVisible = true;  // Controla se o menu está visível ou não
    private float sideMenuMoveDuration = 0.5f;  // Duração da animação
    private float sideMenuMoveDistance = 295f;

    // Variáveis para o botão que alterna um painel específico
    public Button meuBotao;  // O botão que seleciona o painel
    public Transform painelQueSeraSelecionado;  // O painel a ser selecionado
    [SerializeField] private ManipuladorDeFocoUI manipuladorFoco;


    void Awake()
    {
        blockMovement = true;
    }

    void Start()
    {
        _telaDiaIni = true;
        _painelAtivo = _telaIniciar;
        
        _telaIniciar.gameObject.SetActive(true);
        PanelMenu.gameObject.SetActive(false);
        _telaHuds.gameObject.SetActive(false);
        _telaCelular.gameObject.SetActive(false);
        _telaInventario.gameObject.SetActive(false);
        _painelConfig.gameObject.SetActive(false);
        _painelDialogo.gameObject.SetActive(false); // Inicia o painel de diálogo como inativo


        moveTween = _telaIniciar.DOLocalMoveX(moveDistance, moveDuration)
                              .SetEase(Ease.InOutSine)
                              .SetLoops(-1, LoopType.Yoyo);

        // Vincular o botao para alternar o painel
        if (meuBotao != null)
        {
            meuBotao.onClick.AddListener(() => AlternarPainel(painelQueSeraSelecionado));
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
        }
    }

    public void SetDialogo(InputAction.CallbackContext value)
    {
        if (value.performed) AlternarPainel(_painelDialogo); // Método para abrir o novo painel de diálogo
    }

    public void SetInventario(InputAction.CallbackContext value)
    {
        if (value.performed) AlternarPainel(_telaInventario);
    }

    public void SetCelular(InputAction.CallbackContext value)
    {
        if (value.performed) AlternarPainel(_telaCelular);
    }

    public void SetSair(InputAction.CallbackContext value)
    {
       // _telaDiaIni = true;
        if (value.performed)
        {
            _telaDiaIni = true;
            // Se um dos painéis estiver ativo, fecha e volta para a HUD
            if (_painelAtivo == _telaCelular || _painelAtivo == _painelDialogo ||
                _painelAtivo == _telaInventario || _painelAtivo == _painelConfig)
            {
                FecharPainel(_painelAtivo);
                AbrirPainel(_telaHuds);
                MostrarMenuLateral();

            }
            else if (_painelAtivo == _telaHuds)
            {
                // Se já está na HUD, abre o painel de configurações
                AbrirPainel(_painelConfig);
               _panelSliderMenu.gameObject.SetActive(false);
            }
            else
            {
                // Se qualquer outro estiver ativo, volta para a HUD
                FecharPainel(_painelAtivo);
                AbrirPainel(_telaHuds);
                
            }
        }
    }

    private void AbrirPainel(Transform painel)
    {
        painel.gameObject.SetActive(true);
        PanelMenu.gameObject.SetActive(true);

        if (painel == _painelDialogo) // Inclui a lógica para o novo painel de diálogo
        {
            painel.DOScale(1, 1.1f);
            MiniMap.DOScale(0, 0.0f);
        }
        else
        {
            painel.DOScale(1, 1f);
        }

        _painelAtivo = painel;

        if (painel != _telaIniciar && painel != _telaHuds)
        {
            _painelBluer.gameObject.SetActive(true);
            _painelBluer.DOScale(1, 0f);
            painelBloqueando = true;
        }

        if (painel == _telaIniciar || painel == _telaCelular || painel == _telaInventario || painel == _painelConfig || painel == _painelDialogo)
        {
            blockMovement = true;
        }
        else
        {
            _painelBluer.DOScale(0, 0.0f).OnComplete(() => _painelBluer.gameObject.SetActive(false));
            painelBloqueando = false;
        }

        if (painel == _telaCelular)
        {
            EventSystem.current.SetSelectedGameObject(botaoCelular != null ? botaoCelular.gameObject : _telaCelular.GetComponentInChildren<Button>()?.gameObject);
        }

        // Oculte o menu lateral se outro painel for aberto
        if (painel == _telaCelular || painel == _telaInventario || painel == _painelConfig || painel == _painelDialogo)
        {
            OcultarMenuLateral();
        }
        manipuladorFoco.VerificarPainelAtivo();
    }

    public void FecharPainel(Transform painel)
    {
        if (painel == _painelDialogo) // Inclui a lógica de fechamento para o painel de diálogo
        {
            painel.DOScale(0, 0.0f).OnComplete(() => painel.gameObject.SetActive(false));
            MiniMap.DOScale(1, 1.1f);
            _painelBluer.DOScale(0, 0.0f);
        }
        else
        {
            painel.DOScale(0, 0.2f).OnComplete(() => painel.gameObject.SetActive(false));
        }

        if (_telaHuds.gameObject.activeSelf == false &&
            _telaCelular.gameObject.activeSelf == false &&
            _telaInventario.gameObject.activeSelf == false &&
            _painelConfig.gameObject.activeSelf == false)
        {
            _painelBluer.DOScale(0, 0.0f).OnComplete(() => _painelBluer.gameObject.SetActive(false));
        }

        blockMovement = false;
    }

    public void AlternarPainel(Transform painel)
    {
        if (painelBloqueando && painel != _telaHuds) return;

        if (_painelAtivo == painel)
        {
            FecharPainel(painel);
            AbrirPainel(_telaHuds);
            painelBloqueando = false;
        }
        else
        {
            FecharPainel(_painelAtivo);
            AbrirPainel(painel);

            painelBloqueando = (painel == _telaCelular || painel == _telaInventario || painel == _painelConfig);
        }
    }

    public void AbrirPainelComBluer(Transform painel)
{
    _painelBluer.gameObject.SetActive(true);
    _painelBluer.DOScale(1, 0f);
    _painelAtivo = painel;
    painelBloqueando = true;
}

public void FecharPainelComBluer(Transform painel)
{
    if (_painelAtivo == painel)
    {
        _painelAtivo = null;
        painelBloqueando = false;

        // Desativa o _painelBluer se não houver nenhum painel ativo
        if (_painelConfig == null || (!_painelConfig.gameObject.activeSelf && !_painelAtivo))
        {
            _painelBluer.DOScale(0, 0f).OnComplete(() => _painelBluer.gameObject.SetActive(false));
        }
    }
}


    // Funções para controlar o menu lateral
    public void OcultarMenuLateral()
    {
        if (PanelMenu != null && isMenuVisible)
        {
            PanelMenu.DOAnchorPosX(sideMenuMoveDistance, sideMenuMoveDuration)
                     .SetEase(Ease.InOutSine)
                     .OnComplete(() => Debug.Log("Menu lateral ocultado."));

            isMenuVisible = false;

            
            

            
        }
    }

    public void MostrarMenuLateral()
    {
        if (!isMenuVisible)
        {
            PanelMenu.DOAnchorPosX(-295f, sideMenuMoveDuration);
            isMenuVisible = true;

           
        }
    }


    public void SetLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            MostrarMenuLateral();
            Debug.Log("Menu lateral exibido.");
        }
    }

    public void SetRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OcultarMenuLateral();
            Debug.Log("Menu lateral ocultado.");
        }
    }

    public void AbrirPainelEOcultarMenuLateral(Transform painel)
    {
        Debug.Log("Chamando AlternarPainel...");

        // Exibe o painel desejado
        AlternarPainel(painel);

        // Verifica se o menu lateral está visível antes de tentar ocultá-lo
        if (isMenuVisible)
        {
            Debug.Log("Menu lateral visível. Chamando OcultarMenuLateral...");
            OcultarMenuLateral();
        }
        else
        {
            Debug.Log("Menu lateral já está oculto.");
        }
    }

}
