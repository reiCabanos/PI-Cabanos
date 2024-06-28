using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Telacarregamento : MonoBehaviour
{
    // Configura��o da transi��o
    [Header("Configura��es de Fade")]
    public ImageFadeAnimation fadeImage; // Refer�ncia ao script que controla a anima��o de fade da imagem
    public float tempoTransicao = 1f;    // Dura��o da transi��o (deve ser igual � dura��o do fade)

    // Verifica se o bot�o do mouse foi pressionado
    void Update()
    {
        // Input.GetMouseButtonDown(0) detecta o clique inicial do bot�o esquerdo do mouse
        if (Input.GetMouseButtonDown(0))
        {
            IniciarTransicao(); // Inicia o processo de transi��o de cena
        }
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
        // Aguarda o tempo definido na vari�vel 'tempoTransicao' (que deve ser igual � dura��o da anima��o)
        yield return new WaitForSeconds(tempoTransicao);

        // Chama a fun��o para carregar a pr�xima cena ap�s o t�rmino da anima��o
        Loadnextlevel();
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
