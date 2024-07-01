using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Telacarregamento : MonoBehaviour
{ 
    
    [Header("Configurações de Fade")]
    public ImageFadeAnimation fadeImage;
    public float tempoTransicao = 1f;

    [Header("Configurações de Carregamento")]
    public float tempoMinimoExibicao = 2f; // Tempo mínimo de exibição

    [Header("Sprite de Carregamento")]
    public GameObject spriteCarregamento;

    public PlayerInput playerInput;

    void Start()
    {

        SceneManager.sceneLoaded += OnSceneLoaded;
        if (spriteCarregamento != null)
        {
            spriteCarregamento.SetActive(false);
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Inicia a animação de fade out na imagem
        fadeImage.IniciarAnimacao();

        // Inicia uma corrotina para aguardar o término da animação antes de carregar a próxima cena
        StartCoroutine(CarregarProximaCena());
    }

    // Função para iniciar a transição de cena
    public void IniciarTransicao()
    {
        // Inicia a animação de fade out na imagem (a imagem vai se tornando transparente)
        fadeImage.IniciarAnimacao();

        // Inicia uma corrotina para aguardar o término da animação antes de carregar a próxima cena
        StartCoroutine(CarregarProximaCena());
    }

    // Corrotina para aguardar o término da animação antes de carregar a próxima cena
    IEnumerator CarregarProximaCena()
    {
        // Ativa o sprite de carregamento
        spriteCarregamento.SetActive(true);

        // Desativa o input
        playerInput.enabled = false;

        yield return new WaitForSeconds(tempoTransicao);

        int proximaCena = SceneManager.GetActiveScene().buildIndex + 1;

        // Lógica para lidar com a última cena (opcional)
        if (proximaCena >= SceneManager.sceneCountInBuildSettings)
        {
            proximaCena = 0;
        }

        // Carrega a próxima cena de forma assíncrona
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(proximaCena);

        // Aguarda até que a cena termine de carregar
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Aguarda o tempo mínimo de exibição (se necessário)
        float tempoRestante = tempoMinimoExibicao - tempoTransicao;
        if (tempoRestante > 0)
        {
            yield return new WaitForSeconds(tempoRestante);
        }

        // Desativa o sprite de carregamento após o término do carregamento
        spriteCarregamento.SetActive(false);

        // Reabilita o input
        playerInput.enabled = true;
    }

    // Função para carregar a próxima cena
    public void Loadnextlevel()
    {
        // Obtém o índice da cena atual e adiciona 1 para obter o índice da próxima cena
        int proximaCena = SceneManager.GetActiveScene().buildIndex + 1;

        // Verifica se o índice da próxima cena é válido (se existe uma próxima cena na build)
        if (proximaCena < SceneManager.sceneCountInBuildSettings)
        {
            // Carrega a próxima cena
            SceneManager.LoadScene(proximaCena);
        }
        else
        {
            // Exibe um aviso no console se não houver próxima cena para carregar
            Debug.LogWarning("Não há próxima cena para carregar!");
        }
    }
}
