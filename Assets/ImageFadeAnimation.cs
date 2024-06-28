using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class ImageFadeAnimation : MonoBehaviour
{
    [Header("Configurações de Fade")]
    [Range(0f, 1f)] public float alphaInicial = 1f; // Alpha inicial (visível)
    [Range(0f, 1f)] public float alphaFinal = 0f;   // Alpha final (transparente)
    public float duracao = 1f;                     // Duração da animação em segundos

    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = alphaInicial; // Garante que a imagem começa com o alpha correto

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
