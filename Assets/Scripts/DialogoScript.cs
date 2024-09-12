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

    // Vari�veis para anima��o do sprite
    public GameObject spriteGameObject;

    void Start()
    {
        textComponent.text = string.Empty;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping) // Se ainda est� digitando o texto, completa imediatamente
            {
                StopCoroutine(typingCoroutine); // Para a anima��o letra por letra
                textComponent.text = currentText; // Exibe o texto completo
                isTyping = false; // Define que o texto completo foi exibido
            }
        }
    }

    // Fun��o para iniciar o di�logo
    public void IniciarDialogo(string texto)
    {
        currentText = texto;
        textComponent.text = string.Empty; // Limpa o texto
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine); // Garante que a �ltima anima��o foi interrompida
        }
        typingCoroutine = StartCoroutine(TypeLine()); // Inicia a anima��o do texto
    }

    // Coroutine para exibir o texto letra por letra
    IEnumerator TypeLine()
    {
        isTyping = true; // Indica que a anima��o est� em andamento
        foreach (char c in currentText.ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        isTyping = false; // Conclui a anima��o
    }

    // Fun��o para animar o sprite
    public void AnimarSprite()
    {
        if (spriteGameObject != null)
        {
            Vector3 posicaoOriginal = spriteGameObject.transform.position;

            Sequence sequencia = DOTween.Sequence();

            // Aumentar a dura��o das anima��es para deix�-las mais suaves
            float duracaoMovimento = 2f; // Dura��o mais longa para suavizar os movimentos
            float duracaoRotacao = 2f;

            // Ajuste de amplitude do movimento
            float amplitudeMovimentoX = 0.1f; // Movimentos menores para suavidade
            float amplitudeMovimentoY = 0.05f;
            float rotacaoAmplitude = 3f; // Reduzir rota��o para suavizar

            sequencia.Append(spriteGameObject.transform
                .DOMoveX(posicaoOriginal.x + amplitudeMovimentoX, duracaoMovimento)
                .SetEase(Ease.InOutSine)) // Interpola��o suave
              .Join(spriteGameObject.transform
                .DORotate(new Vector3(0, 0, rotacaoAmplitude), duracaoRotacao)
                .SetEase(Ease.InOutSine)) // Suave
              .Append(spriteGameObject.transform
                .DOMoveY(posicaoOriginal.y + amplitudeMovimentoY, duracaoMovimento)
                .SetEase(Ease.InOutSine)) // Movimento suave no eixo Y
              .Join(spriteGameObject.transform
                .DORotate(new Vector3(0, 0, -rotacaoAmplitude), duracaoRotacao)
                .SetEase(Ease.InOutSine)) // Rota��o suave
              .Append(spriteGameObject.transform
                .DOMoveX(posicaoOriginal.x - amplitudeMovimentoX, duracaoMovimento)
                .SetEase(Ease.InOutSine)) // Movimento de volta suave
              .Join(spriteGameObject.transform
                .DORotate(new Vector3(0, 0, 0), duracaoRotacao)
                .SetEase(Ease.InOutSine)) // Retorna � rota��o original suavemente
              .Append(spriteGameObject.transform
                .DOMoveY(posicaoOriginal.y, duracaoMovimento)
                .SetEase(Ease.InOutSine)) // Retorna � posi��o original suave
              .SetLoops(-1, LoopType.Yoyo); // Repeti��o suave
        }
    }
}