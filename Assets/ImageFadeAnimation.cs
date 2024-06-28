using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class ImageFadeAnimation : MonoBehaviour
{
    [Header("Configura��es de Fade")]
    [Range(0f, 1f)] public float alphaInicial = 1f; // Alpha inicial (vis�vel)
    [Range(0f, 1f)] public float alphaFinal = 0f;   // Alpha final (transparente)
    public float duracao = 1f;                     // Dura��o da anima��o em segundos

    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = alphaInicial; // Garante que a imagem come�a com o alpha correto

        IniciarAnimacao();
    }

    public void IniciarAnimacao()
    {
        canvasGroup.DOFade(alphaFinal, duracao).SetEase(Ease.Linear)
            .OnComplete(DesativarBloqueioRaios);
    }

    void DesativarBloqueioRaios()
    {
        canvasGroup.blocksRaycasts = false; // Permite que os cliques passem pela imagem
    }
}
