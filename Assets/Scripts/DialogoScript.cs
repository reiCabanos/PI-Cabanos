using System.Collections;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class DialogoScript : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public float textSpeed;
    private string currentText;
    private Coroutine typingCoroutine;
    private bool isTyping;

    // Variáveis para animação do sprite
    public GameObject spriteGameObject;

    void Start()
    {
        textComponent.text = string.Empty;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping) // Se ainda está digitando o texto, completa imediatamente
            {
                StopCoroutine(typingCoroutine); // Para a animação letra por letra
                textComponent.text = currentText; // Exibe o texto completo
                isTyping = false; // Define que o texto completo foi exibido
            }
        }
    }

    // Função para iniciar o diálogo
    public void IniciarDialogo(string texto)
    {
        currentText = texto;
        textComponent.text = string.Empty; // Limpa o texto
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine); // Garante que a última animação foi interrompida
        }
        typingCoroutine = StartCoroutine(TypeLine()); // Inicia a animação do texto
    }

    // Coroutine para exibir o texto letra por letra
    IEnumerator TypeLine()
    {
        isTyping = true; // Indica que a animação está em andamento
        foreach (char c in currentText.ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        isTyping = false; // Conclui a animação
    }

    // Função para animar o sprite
    public void AnimarSprite()
    {
        if (spriteGameObject != null)
        {
            Vector3 posicaoOriginal = spriteGameObject.transform.position;

            Sequence sequencia = DOTween.Sequence();

            // Aumentar a duração das animações para deixá-las mais suaves
            float duracaoMovimento = 2f; // Duração mais longa para suavizar os movimentos
            float duracaoRotacao = 2f;

            // Ajuste de amplitude do movimento
            float amplitudeMovimentoX = 0.1f; // Movimentos menores para suavidade
            float amplitudeMovimentoY = 0.05f;
            float rotacaoAmplitude = 3f; // Reduzir rotação para suavizar

            sequencia.Append(spriteGameObject.transform
                .DOMoveX(posicaoOriginal.x + amplitudeMovimentoX, duracaoMovimento)
                .SetEase(Ease.InOutSine)) // Interpolação suave
              .Join(spriteGameObject.transform
                .DORotate(new Vector3(0, 0, rotacaoAmplitude), duracaoRotacao)
                .SetEase(Ease.InOutSine)) // Suave
              .Append(spriteGameObject.transform
                .DOMoveY(posicaoOriginal.y + amplitudeMovimentoY, duracaoMovimento)
                .SetEase(Ease.InOutSine)) // Movimento suave no eixo Y
              .Join(spriteGameObject.transform
                .DORotate(new Vector3(0, 0, -rotacaoAmplitude), duracaoRotacao)
                .SetEase(Ease.InOutSine)) // Rotação suave
              .Append(spriteGameObject.transform
                .DOMoveX(posicaoOriginal.x - amplitudeMovimentoX, duracaoMovimento)
                .SetEase(Ease.InOutSine)) // Movimento de volta suave
              .Join(spriteGameObject.transform
                .DORotate(new Vector3(0, 0, 0), duracaoRotacao)
                .SetEase(Ease.InOutSine)) // Retorna à rotação original suavemente
              .Append(spriteGameObject.transform
                .DOMoveY(posicaoOriginal.y, duracaoMovimento)
                .SetEase(Ease.InOutSine)) // Retorna à posição original suave
              .SetLoops(-1, LoopType.Yoyo); // Repetição suave
        }
    }
}