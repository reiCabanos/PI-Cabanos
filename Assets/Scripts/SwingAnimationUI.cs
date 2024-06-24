using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class SwingAnimationUI : MonoBehaviour
{
    [Header("Configurações da Animação")]
    [SerializeField] private List<RectTransform> uiElements;
    [SerializeField] private float breezeDuration = 2f;      // Duração do movimento
    [SerializeField] private float breezeStrength = 10f;    // Intensidade do movimento
    [SerializeField] private Ease breezeEase = Ease.InOutSine; // Tipo de easing

    private void Start()
    {
        AnimateBreeze();
    }

    private void AnimateBreeze()
    {
        foreach (RectTransform uiElement in uiElements)
        {
            // Calcula as posições inicial e final
            Vector3 startPosition = uiElement.anchoredPosition;
            Vector3 targetPosition = startPosition + new Vector3(Random.Range(-breezeStrength, breezeStrength), 0, 0);

            // Cria a sequência de animação
            Sequence breezeSequence = DOTween.Sequence();
            breezeSequence
                .Append(uiElement.DOAnchorPos(targetPosition, breezeDuration).SetEase(breezeEase)) // Movimento para um lado
                .Append(uiElement.DOAnchorPos(startPosition, breezeDuration).SetEase(breezeEase)) // Movimento de volta
                .SetLoops(-1, LoopType.Yoyo); // Repete indefinidamente
        }
    }
}
