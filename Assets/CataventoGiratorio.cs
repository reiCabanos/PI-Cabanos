using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class CataventoComFade : MonoBehaviour
{
    [Header("Configurações de Fade")]
    [Range(0f, 1f)] public float alphaInicial = 1f; // Alpha inicial (visível)
    [Range(0f, 1f)] public float alphaFinal = 0f;   // Alpha final (transparente)
    public float duracaoFade = 1f;                 // Duração da animação de fade

    [Header("Configurações de Rotação")]
    public float velocidadeRotacao = 180f; // Graus por segundo
    private Tweener rotacaoTweener; // Referência para a animação de rotação



    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = alphaInicial;

        // Inicia a animação de fade e chama PararRotacao ao completar
        canvasGroup.DOFade(alphaFinal, duracaoFade).SetEase(Ease.Linear)
            .OnComplete(PararRotacao);

        // Inicia a animação de rotação
        rotacaoTweener = transform.DORotate(new Vector3(0, 0, 360), velocidadeRotacao, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental);

    }

    public void PararRotacao()
    {
        if (rotacaoTweener != null)
        {
            rotacaoTweener.Kill(); // Interrompe a animação
        }
    }
}
