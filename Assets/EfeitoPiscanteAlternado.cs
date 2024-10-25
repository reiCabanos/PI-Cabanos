using UnityEngine;
using DG.Tweening; // Certifique-se de importar a biblioteca DOTween
using TMPro; // Certifique-se de importar a biblioteca TextMeshPro
using System.Collections; // Necess�rio para usar IEnumerator

public class EfeitoPiscanteAlternado : MonoBehaviour
{
    public Transform firstTransform; // Primeiro objeto Transform
    public Transform secondTransform; // Segundo objeto Transform
    public float blinkDuration = 0.5f; // Dura��o de cada piscada
    public float alternateDelay = 1.0f; // Tempo de espera entre a altern�ncia

    private CanvasGroup firstCanvasGroup;
    private CanvasGroup secondCanvasGroup;

    void Start()
    {
        // Verifica se os Transforms foram atribu�dos e configura os CanvasGroups
        if (firstTransform != null && secondTransform != null)
        {
            firstCanvasGroup = firstTransform.gameObject.AddComponent<CanvasGroup>();
            secondCanvasGroup = secondTransform.gameObject.AddComponent<CanvasGroup>();

            // Inicia a altern�ncia entre os dois Transforms
            StartCoroutine(AlternateBlinking());
        }
        else
        {
            Debug.LogError("Um ou ambos os Transforms n�o foram atribu�dos!");
        }
    }

    private IEnumerator AlternateBlinking()
    {
        while (true) // Loop infinito para alternar continuamente
        {
            // Pisca o primeiro Transform
            yield return BlinkTransform(firstCanvasGroup);

            // Ativa o segundo Transform
            firstCanvasGroup.alpha = 0; // Primeiro invis�vel
            secondCanvasGroup.alpha = 1; // Segundo vis�vel
            yield return new WaitForSeconds(alternateDelay); // Espera antes de alternar

            // Pisca o segundo Transform
            yield return BlinkTransform(secondCanvasGroup);

            // Ativa o primeiro Transform
            secondCanvasGroup.alpha = 0; // Segundo invis�vel
            firstCanvasGroup.alpha = 1; // Primeiro vis�vel
            yield return new WaitForSeconds(alternateDelay); // Espera antes de alternar novamente
        }
    }

    private IEnumerator BlinkTransform(CanvasGroup canvasGroup)
    {
        // Anima a opacidade do Transform com efeito de piscar
        canvasGroup.DOFade(0, blinkDuration).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(blinkDuration);
        canvasGroup.DOFade(1, blinkDuration).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(blinkDuration);
    }
}
