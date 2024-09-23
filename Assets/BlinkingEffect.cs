using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; // Certifique-se de importar a biblioteca DOTween
using TMPro; // Certifique-se de importar a biblioteca TextMeshPro

public class BlinkingEffect : MonoBehaviour
{
    public Button button; // Arraste o botão aqui no Unity
    public TextMeshProUGUI textMeshPro; // Arraste o TextMeshPro aqui no Unity
    public float blinkDuration = 0.5f; // Duração de cada piscada

    private CanvasGroup buttonCanvasGroup;
    private CanvasGroup textCanvasGroup;

    void Start()
    {
        // Verifica se o botão foi atribuído e configura o CanvasGroup
        if (button != null)
        {
            buttonCanvasGroup = button.gameObject.AddComponent<CanvasGroup>();
            StartBlinkingButton();
        }
        else
        {
            Debug.LogError("Nenhum botão foi atribuído!");
        }

        // Verifica se o TextMeshPro foi atribuído e configura o CanvasGroup
        if (textMeshPro != null)
        {
            textCanvasGroup = textMeshPro.gameObject.AddComponent<CanvasGroup>();
            StartBlinkingText();
        }
        else
        {
            Debug.LogError("Nenhum TextMeshPro foi atribuído!");
        }
    }

    void StartBlinkingButton()
    {
        // Anima a opacidade do botão
        buttonCanvasGroup.DOFade(0, blinkDuration).SetLoops(-1, LoopType.Yoyo);
    }

    void StartBlinkingText()
    {
        // Anima a opacidade do TextMeshPro
        textCanvasGroup.DOFade(0, blinkDuration).SetLoops(-1, LoopType.Yoyo);
    }
}
