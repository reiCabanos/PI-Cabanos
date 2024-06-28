using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class CataventoComFade : MonoBehaviour
{
    [Header("Configura��es de Fade")]
    [Range(0f, 1f)] public float alphaInicial = 1f; // Alpha inicial (vis�vel)
    [Range(0f, 1f)] public float alphaFinal = 0f;   // Alpha final (transparente)
    public float duracaoFade = 1f;                 // Dura��o da anima��o de fade

    [Header("Configura��es de Rota��o")]
    public float velocidadeRotacao = 180f; // Graus por segundo
    private Tweener rotacaoTweener; // Refer�ncia para a anima��o de rota��o



    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = alphaInicial;

        // Inicia a anima��o de fade e chama PararRotacao ao completar
        canvasGroup.DOFade(alphaFinal, duracaoFade).SetEase(Ease.Linear)
            .OnComplete(PararRotacao);

        // Inicia a anima��o de rota��o
        rotacaoTweener = transform.DORotate(new Vector3(0, 0, 360), velocidadeRotacao, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental);

    }

    public void PararRotacao()
    {
        if (rotacaoTweener != null)
        {
            rotacaoTweener.Kill(); // Interrompe a anima��o
        }
    }
}
