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

    private Transform _painelAtivo;
    private bool painelBloqueando = false;
    private float moveDistance = 35f;
    private float moveDuration = 4f;
    private Tweener moveTween;
    public bool blockMovement = true;
    public GameObject _panelSliderMenu;

    public Transform MiniMap;
    public Transform _painelNovo;
    public float tempoExibicaoPainel = 5f;
    private bool painelNovoExibido = false;
    public float tempoExibicaoPainelNovo = 10f;

    public Button botaoCelular;
    public SideMenuController sideMenuController;
    public Button botaoFecharPainelNovo;
    public bool _telaDiaIni;

    void Awake()
    {
        blockMovement = true;
    }

    void Start()
    {
        _painelAtivo = _telaIniciar;
        _telaIniciar.gameObject.SetActive(true);
        _telaHuds.gameObject.SetActive(false);
        _telaCelular.gameObject.SetActive(false);
        _telaInventario.gameObject.SetActive(false);
        _painelConfig.gameObject.SetActive(false);
       // _painelDialogo.gameObject.SetActive(false); // Inicia o painel de diálogo como inativo

        moveTween = _telaIniciar.DOLocalMoveX(moveDistance, moveDuration)
                              .SetEase(Ease.InOutSine)
                              .SetLoops(-1, LoopType.Yoyo);

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
           // _panelSliderMenu.gameObject.SetActive(true);
            StartCoroutine(ExibirPainelNovo());
        }
    }

    private IEnumerator ExibirPainelNovo()
    {
        yield return new WaitForSeconds(tempoExibicaoPainel);
        if (!painelNovoExibido)
        {
            AbrirPainel(_painelNovo);
            painelNovoExibido = true;
            StartCoroutine(OcultarPainelNovo());
        }
    }

    private IEnumerator OcultarPainelNovo()
    {
        yield return new WaitForSeconds(tempoExibicaoPainelNovo);
        if (painelNovoExibido)
        {
            FecharPainel(_painelNovo);
            painelNovoExibido = false;
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
        _telaDiaIni = true;
        if (value.performed)
        {
            _telaDiaIni = true;
            if (_painelAtivo == _painelNovo) FecharPainel(_painelNovo);
            else if (_painelAtivo == _telaCelular || _painelAtivo ) // Adicionado o painel de diálogo à condição
            {
                FecharPainel(_painelAtivo);
                AbrirPainel(_telaHuds);
            }
            else if (_painelAtivo != _telaHuds)
            {
                FecharPainel(_painelAtivo);
                AbrirPainel(_telaHuds);
            }
        }
    }

    private void AbrirPainel(Transform painel)
    {
        painel.gameObject.SetActive(true);

        if (painel == _painelNovo || painel ) // Inclui a lógica para o novo painel
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

        if (painel == _telaIniciar || painel == _telaCelular || painel == _telaInventario || painel == _painelConfig || painel == _painelNovo || painel )
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
    }

    private void FecharPainel(Transform painel)
    {
        if (painel == _painelNovo || painel ) // Inclui a lógica de fechamento para o novo painel
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

        if (painel == _painelNovo || painel )
        {
            painelNovoExibido = false;
        }
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
}
