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

    [Header("Scene Settings")]
    [SerializeField] private string nomeDaCena; // Nome da cena a ser carregada (definido pelo Inspetor)
    [SerializeField] private float transitionTime = 10f; // Tempo ajustável para a transição de cena

    [Header("Tela de Carregamento")]
    [SerializeField] private GameObject telaDeCarregamentoPrefab; // Referência ao prefab da tela de carregamento
    [SerializeField] private RectTransform fader; // Fader de transição (adicionado para a transição)

    [Header("Slider de Carregamento")]
    [SerializeField] private Slider sliderDeCarregamento; // Referência ao slider de carregamento
    [SerializeField] private TextMeshProUGUI porcentagemDeCarregamento; // Exibir porcentagem de carregamento

    private MenuControl _currentMenu;
    private bool isTransitioning = false; // Variável para controlar transições

    void Start()
    {
        InitializeMenus(); // Inicializa os menus corretamente

        // Certifique-se de que os botões estão atribuídos corretamente no Inspetor
        botaoIniciar.onClick.AddListener(IniciarJogo); // Liga o botão "Iniciar" para carregar a cena
        botaoSair.onClick.AddListener(SairDoJogo);  // Liga o botão "Sair" para sair ou voltar ao menu principal

        // Certifique-se de que a tela de carregamento está desativada inicialmente
        if (telaDeCarregamentoPrefab != null)
        {
            telaDeCarregamentoPrefab.SetActive(false);
        }

        // Certifique-se de que o fader está desativado inicialmente
        if (fader != null)
        {
            fader.gameObject.SetActive(false);
        }

        // Certifique-se de que o slider e a porcentagem estão ocultos inicialmente
        if (sliderDeCarregamento != null)
        {
            sliderDeCarregamento.gameObject.SetActive(false);
        }
        if (porcentagemDeCarregamento != null)
        {
            porcentagemDeCarregamento.gameObject.SetActive(false);
        }
    }

    // Inicializa os menus, deixando apenas o primeiro ativo
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

    // Função para carregar a cena de jogo quando o botão iniciar for clicado
    public void IniciarJogo()
    {
        // Verifica se o nome da cena foi configurado no Inspetor
        if (!string.IsNullOrEmpty(nomeDaCena) && !isTransitioning)
        {
            Debug.Log("Carregando a cena: " + nomeDaCena); // Log para verificar se a função foi chamada
            isTransitioning = true; // Marca que a transição começou

            // Ativar o fader antes da transição
            if (fader != null)
            {
                fader.gameObject.SetActive(true);
                LeanTween.scale(fader, Vector3.one, 0.5f).setEase(LeanTweenType.easeInOutQuad);
            }

            // Inicia a corrotina para exibir a tela de carregamento e carregar a cena com atraso
            StartCoroutine(ExibirTelaDeCarregamentoECarregarCena());
        }
        else
        {
            Debug.LogError("Nome da cena não está definido no Inspetor ou transição já em andamento!");
        }
    }

    // Corrotina que garante que a tela de carregamento seja exibida antes do carregamento da cena
    private IEnumerator ExibirTelaDeCarregamentoECarregarCena()
    {
        // Ativa o prefab da tela de carregamento
        if (telaDeCarregamentoPrefab != null)
        {
            telaDeCarregamentoPrefab.SetActive(true);
        }

        // Ativa o slider e a porcentagem
        if (sliderDeCarregamento != null)
        {
            sliderDeCarregamento.gameObject.SetActive(true);
            sliderDeCarregamento.maxValue = 100;
            sliderDeCarregamento.value = 100; // Inicia no valor máximo
        }
        if (porcentagemDeCarregamento != null)
        {
            porcentagemDeCarregamento.gameObject.SetActive(true);
            porcentagemDeCarregamento.text = "100%"; // Exibe o valor inicial
        }

        float currentTime = 0f;

        // Durante o tempo de transição, diminui o valor do slider
        while (currentTime < transitionTime)
        {
            currentTime += Time.deltaTime;
            float progress = Mathf.Clamp01(currentTime / transitionTime);
            float sliderValue = Mathf.Lerp(100, 0, progress);

            // Atualiza o valor do slider e do texto
            if (sliderDeCarregamento != null)
            {
                sliderDeCarregamento.value = sliderValue;
            }
            if (porcentagemDeCarregamento != null)
            {
                porcentagemDeCarregamento.text = Mathf.RoundToInt(sliderValue).ToString() + "%";
            }

            yield return null; // Espera até o próximo frame
        }

        // Inicia o carregamento assíncrono da cena
        AsyncOperation operacao = SceneManager.LoadSceneAsync(nomeDaCena);
        operacao.allowSceneActivation = false; // Bloqueia a ativação da cena até o final da transição

        // Enquanto a cena está carregando
        while (!operacao.isDone)
        {
            // Quando o carregamento estiver quase completo (90%)
            if (operacao.progress >= 0.9f)
            {
                // Pequeno atraso antes de ativar a cena
                yield return new WaitForSeconds(0.5f);
                operacao.allowSceneActivation = true; // Ativar a cena
            }
            yield return null;  // Espera até o próximo frame
        }

        // Após a cena estar completamente carregada, finalizar a transição
        if (fader != null)
        {
            LeanTween.scale(fader, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => {
                fader.gameObject.SetActive(false); // Desativa o fader após a transição
                isTransitioning = false; // Marca que a transição terminou
            });
        }

        // Desativa o slider e a porcentagem após o carregamento completo
        if (sliderDeCarregamento != null)
        {
            sliderDeCarregamento.gameObject.SetActive(false);
        }
        if (porcentagemDeCarregamento != null)
        {
            porcentagemDeCarregamento.gameObject.SetActive(false);
        }
    }

    // Função para sair do jogo ou voltar ao menu principal
    public void SairDoJogo()
    {
        SceneManager.LoadScene("Menu"); // Exemplo de como voltar ao menu principal
    }
}
