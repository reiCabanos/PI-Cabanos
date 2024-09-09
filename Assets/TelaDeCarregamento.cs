using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class TelaDeCarregamento : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Slider sliderProgresso;  // Refer�ncia ao Slider na tela de carregamento
    [SerializeField] private TMP_Text textoProgresso; // Refer�ncia ao componente TextMeshPro para exibir a porcentagem de carregamento

    [Header("Imagens")]
    [SerializeField] private Image imgParaMudar; // Imagem que ser� mudada durante o carregamento
    [SerializeField] private Sprite[] imagens;  // Lista de sprites que podem ser usados

    private float progressoMaximo = 1f;  // O valor m�ximo do progresso da cena � 1 (100%)

    void Start()
    {
        // Inicializa o slider e a porcentagem de progresso
        sliderProgresso.value = 0;
        textoProgresso.text = "0%";
    }

    // Esta fun��o ser� chamada pelo MenuManager quando a cena come�ar a ser carregada
    public void IniciarCarregamentoCena(string nomeDaCena)
    {
        // Inicia a corrotina para carregar a cena de forma ass�ncrona e estilizada
        StartCoroutine(LoadSceneEstiloso(nomeDaCena));
    }

    // Corrotina para simular um carregamento com estilo
    private IEnumerator LoadSceneEstiloso(string nomeDaCena)
    {
        // Inicia o carregamento da cena de forma ass�ncrona
        AsyncOperation operacao = SceneManager.LoadSceneAsync(nomeDaCena);
        operacao.allowSceneActivation = false; // N�o ativa a cena at� que chegue a 100%

        float progressoSimulado = 0f;

        // Simula o progresso
        while (progressoSimulado < 100f)
        {
            // Simula o progresso incremental (entre 5% e 15% a cada 0.8s)
            progressoSimulado += Random.Range(5f, 15f);
            progressoSimulado = Mathf.Clamp(progressoSimulado, 0, 100); // Garante que o progresso n�o ultrapasse 100%

            // Atualiza o valor do slider com suaviza��o
            sliderProgresso.value = Mathf.Lerp(sliderProgresso.value, progressoSimulado, 0.5f);

            // Atualiza o texto de porcentagem
            textoProgresso.text = Mathf.RoundToInt(progressoSimulado) + "%";

            // Muda a imagem durante o carregamento
            MudarImagem();

            // Espera um pouco antes de continuar simulando o progresso
            yield return new WaitForSeconds(0.8f);
        }

        // Garanta que o progresso chegue a 100%
        sliderProgresso.value = 100f;
        textoProgresso.text = "100%";

        // Libera a ativa��o da cena quando o progresso estiver em 100%
        operacao.allowSceneActivation = true;

        yield return null;
    }

    // Fun��o para mudar a imagem aleatoriamente durante o carregamento
    private void MudarImagem()
    {
        if (imagens.Length > 0)
        {
            // Seleciona uma imagem aleatoriamente
            int rand = Random.Range(0, imagens.Length);
            imgParaMudar.sprite = imagens[rand];
        }
    }
}
