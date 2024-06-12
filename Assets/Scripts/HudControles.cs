using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using DG.Tweening;

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

    void Start()
    {
        _painelAtivo = _telaIniciar;
        _telaIniciar.gameObject.SetActive(true);

        _telaHuds.gameObject.SetActive(false);
        _telaCelular.gameObject.SetActive(false);
        _telaInventario.gameObject.SetActive(false);
        _painelConfig.gameObject.SetActive(false);
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
        }
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
        if (value.performed)
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
        painel.DOScale(1, 1f);
        _painelAtivo = painel;

        if (painel != _telaIniciar && painel != _telaHuds)
        {
            _painelBluer.gameObject.SetActive(true);
            _painelBluer.DOScale(1, 1f);
            painelBloqueando = true; // Bloqueia outros painéis
        }
        else
        {
            _painelBluer.DOScale(0, 0.2f).OnComplete(() => _painelBluer.gameObject.SetActive(false));
            painelBloqueando = false; // Desbloqueia outros painéis ao abrir _telaHuds
        }
    }

    private void FecharPainel(Transform painel)
    {
        painel.DOScale(0, 0.2f).OnComplete(() => painel.gameObject.SetActive(false));

        if (_telaHuds.gameObject.activeSelf == false &&
            _telaCelular.gameObject.activeSelf == false &&
            _telaInventario.gameObject.activeSelf == false &&
            _painelConfig.gameObject.activeSelf == false)
        {
            _painelBluer.DOScale(0, 0.2f).OnComplete(() => _painelBluer.gameObject.SetActive(false));
        }
    }

    private void AlternarPainel(Transform painel)
    {
        if (painelBloqueando && painel != _telaHuds)
        {
            return; // Impede a abertura de outros painéis se um estiver bloqueando
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
