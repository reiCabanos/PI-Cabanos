using UnityEngine;
using DG.Tweening; // Certifique-se de importar a biblioteca DOTween
using TMPro; // Certifique-se de importar a biblioteca TextMeshPro

public class EfeitoPiscante : MonoBehaviour
{
    public Transform targetTransform; // Arraste o objeto Transform aqui no Unity
    public float blinkDuration = 0.5f; // Duração de cada piscada

    private CanvasGroup targetCanvasGroup;

    void Start()
    {
        // Verifica se o Transform foi atribuído e configura o CanvasGroup
        if (targetTransform != null)
        {
            targetCanvasGroup = targetTransform.gameObject.AddComponent<CanvasGroup>();
            StartBlinkingTransform();
        }
        else
        {
            Debug.LogError("Nenhum Transform foi atribuído!");
        }
    }

    void StartBlinkingTransform()
    {
        // Anima a opacidade do objeto Transform
        targetCanvasGroup.DOFade(0, blinkDuration).SetLoops(-1, LoopType.Yoyo);

        // Verifica se o Transform contém um TextMeshProUGUI e inicia o efeito de piscar
        TextMeshProUGUI textMeshPro = targetTransform.GetComponent<TextMeshProUGUI>();
        if (textMeshPro != null)
        {
            CanvasGroup textCanvasGroup = textMeshPro.gameObject.AddComponent<CanvasGroup>();
            StartBlinkingText(textCanvasGroup);
        }
    }

    void StartBlinkingText(CanvasGroup textCanvasGroup)
    {
        // Anima a opacidade do TextMeshPro
        textCanvasGroup.DOFade(0, blinkDuration).SetLoops(-1, LoopType.Yoyo);
    }
}
