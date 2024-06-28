using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Telacarregamento : MonoBehaviour
{
    // Configuração da transição
    [Header("Configurações de Fade")]
    public ImageFadeAnimation fadeImage; // Referência ao script que controla a animação de fade da imagem
    public float tempoTransicao = 1f;    // Duração da transição (deve ser igual à duração do fade)

    // Verifica se o botão do mouse foi pressionado
    void Update()
    {
        // Input.GetMouseButtonDown(0) detecta o clique inicial do botão esquerdo do mouse
        if (Input.GetMouseButtonDown(0))
        {
            IniciarTransicao(); // Inicia o processo de transição de cena
        }
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
        // Aguarda o tempo definido na variável 'tempoTransicao' (que deve ser igual à duração da animação)
        yield return new WaitForSeconds(tempoTransicao);

        // Chama a função para carregar a próxima cena após o término da animação
        Loadnextlevel();
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
