using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Telacarregamento : MonoBehaviour
{ 
    
    [Header("Configura��es de Fade")]
    public ImageFadeAnimation fadeImage;
    public float tempoTransicao = 1f;

    [Header("Configura��es de Carregamento")]
    public float tempoMinimoExibicao = 2f; // Tempo m�nimo de exibi��o

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
        // Inicia a anima��o de fade out na imagem
        fadeImage.IniciarAnimacao();

        // Inicia uma corrotina para aguardar o t�rmino da anima��o antes de carregar a pr�xima cena
        StartCoroutine(CarregarProximaCena());
    }

    // Fun��o para iniciar a transi��o de cena
    public void IniciarTransicao()
    {
        // Inicia a anima��o de fade out na imagem (a imagem vai se tornando transparente)
        fadeImage.IniciarAnimacao();

        // Inicia uma corrotina para aguardar o t�rmino da anima��o antes de carregar a pr�xima cena
        StartCoroutine(CarregarProximaCena());
    }

    // Corrotina para aguardar o t�rmino da anima��o antes de carregar a pr�xima cena
    IEnumerator CarregarProximaCena()
    {
        // Ativa o sprite de carregamento
        spriteCarregamento.SetActive(true);

        // Desativa o input
        playerInput.enabled = false;

        yield return new WaitForSeconds(tempoTransicao);

        int proximaCena = SceneManager.GetActiveScene().buildIndex + 1;

        // L�gica para lidar com a �ltima cena (opcional)
        if (proximaCena >= SceneManager.sceneCountInBuildSettings)
        {
            proximaCena = 0;
        }

        // Carrega a pr�xima cena de forma ass�ncrona
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(proximaCena);

        // Aguarda at� que a cena termine de carregar
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Aguarda o tempo m�nimo de exibi��o (se necess�rio)
        float tempoRestante = tempoMinimoExibicao - tempoTransicao;
        if (tempoRestante > 0)
        {
            yield return new WaitForSeconds(tempoRestante);
        }

        // Desativa o sprite de carregamento ap�s o t�rmino do carregamento
        spriteCarregamento.SetActive(false);

        // Reabilita o input
        playerInput.enabled = true;
    }

    // Fun��o para carregar a pr�xima cena
    public void Loadnextlevel()
    {
        // Obt�m o �ndice da cena atual e adiciona 1 para obter o �ndice da pr�xima cena
        int proximaCena = SceneManager.GetActiveScene().buildIndex + 1;

        // Verifica se o �ndice da pr�xima cena � v�lido (se existe uma pr�xima cena na build)
        if (proximaCena < SceneManager.sceneCountInBuildSettings)
        {
            // Carrega a pr�xima cena
            SceneManager.LoadScene(proximaCena);
        }
        else
        {
            // Exibe um aviso no console se n�o houver pr�xima cena para carregar
            Debug.LogWarning("N�o h� pr�xima cena para carregar!");
        }
    }
}
