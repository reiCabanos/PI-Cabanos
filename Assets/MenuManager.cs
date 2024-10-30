using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Header("Menu Control Settings")]
    [SerializeField] private List<MenuControl> _MenuControls; // Lista de menus

    [Header("Botões")]
    [SerializeField] private Button botaoIniciar; // Referência ao botão iniciar
    [SerializeField] private Button botaoSair;    // Referência ao botão para sair do jogo
    [SerializeField] private Button botaoCarregarSegundaCena; // Referência ao botão para carregar a segunda cena

    [Header("Scene Settings")]
    [SerializeField] private string nomeDaCena; // Nome da primeira cena a ser carregada
    [SerializeField] private string nomeDaSegundaCena; // Nome da segunda cena a ser carregada
    [SerializeField] private float transitionTime = 10f; // Tempo ajustável para a transição de cena

    [Header("Tela de Carregamento")]
    [SerializeField] private GameObject telaDeCarregamentoPrefab; // Referência ao prefab da tela de carregamento
    [SerializeField] private RectTransform fader; // Fader de transição

    [Header("Slider de Carregamento")]
    [SerializeField] private Slider sliderDeCarregamento; // Referência ao slider de carregamento
    [SerializeField] private TextMeshProUGUI porcentagemDeCarregamento; // Exibir porcentagem de carregamento

    private MenuControl _currentMenu;
    private bool isTransitioning = false; // Variável para controlar transições

    void Start()
    {
        InitializeMenus();

        // Liga os botões aos métodos correspondentes
        botaoIniciar.onClick.AddListener(() => IniciarJogo(nomeDaCena));
        botaoCarregarSegundaCena.onClick.AddListener(() => IniciarJogo(nomeDaSegundaCena));
        botaoSair.onClick.AddListener(SairDoJogo);

        // Inicializa a tela de carregamento como inativa
        if (telaDeCarregamentoPrefab != null) telaDeCarregamentoPrefab.SetActive(false);
        if (fader != null) fader.gameObject.SetActive(false);
        if (sliderDeCarregamento != null) sliderDeCarregamento.gameObject.SetActive(false);
        if (porcentagemDeCarregamento != null) porcentagemDeCarregamento.gameObject.SetActive(false);
    }

    private void InitializeMenus()
    {
        foreach (var menu in _MenuControls)
        {
            menu.transform.localScale = Vector3.zero;
            menu.gameObject.SetActive(false);
        }

        if (_MenuControls.Count > 0)
        {
            _MenuControls[0].gameObject.SetActive(true);
            _MenuControls[0].MenuOFF();
            _MenuControls[0].transform.DOScale(1f, 0.5f);
            _MenuControls[0].ChamaMenu();
            _currentMenu = _MenuControls[0];
        }
    }

    // Função para iniciar o carregamento da cena especificada
    public void IniciarJogo(string nomeDaCena)
    {
        if (!string.IsNullOrEmpty(nomeDaCena) && !isTransitioning)
        {
            Debug.Log("Carregando a cena: " + nomeDaCena);
            isTransitioning = true;
            if (fader != null)
            {
                fader.gameObject.SetActive(true);
                LeanTween.scale(fader, Vector3.one, 0.5f).setEase(LeanTweenType.easeInOutQuad);
            }
            StartCoroutine(ExibirTelaDeCarregamentoECarregarCena(nomeDaCena));
        }
        else
        {
            Debug.LogError("Nome da cena não está definido ou transição já em andamento!");
        }
    }

    // Corrotina para exibir a tela de carregamento e carregar a cena especificada
    private IEnumerator ExibirTelaDeCarregamentoECarregarCena(string cenaParaCarregar)
    {
        if (telaDeCarregamentoPrefab != null) telaDeCarregamentoPrefab.SetActive(true);
        if (sliderDeCarregamento != null) sliderDeCarregamento.gameObject.SetActive(true);
        if (porcentagemDeCarregamento != null) porcentagemDeCarregamento.gameObject.SetActive(true);

        float currentTime = 0f;
        while (currentTime < transitionTime)
        {
            currentTime += Time.deltaTime;
            float progress = Mathf.Clamp01(currentTime / transitionTime);
            float sliderValue = Mathf.Lerp(0, 100, progress);

            if (sliderDeCarregamento != null) sliderDeCarregamento.value = sliderValue;
            if (porcentagemDeCarregamento != null) porcentagemDeCarregamento.text = Mathf.RoundToInt(sliderValue).ToString() + "%";

            yield return null;
        }

        AsyncOperation operacao = SceneManager.LoadSceneAsync(cenaParaCarregar);
        operacao.allowSceneActivation = false;

        while (!operacao.isDone)
        {
            if (operacao.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0.5f);
                operacao.allowSceneActivation = true;
            }
            yield return null;
        }

        if (fader != null)
        {
            LeanTween.scale(fader, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => fader.gameObject.SetActive(false));
        }

        isTransitioning = false;
    }

    // Função para sair do jogo ou voltar ao menu principal
    public void SairDoJogo()
    {
        SceneManager.LoadScene("Menu");
    }
}
