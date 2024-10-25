using UnityEngine;
using DG.Tweening; // Certifique-se de importar a biblioteca DOTween

public class SimuladorMovimentoPerson : MonoBehaviour
{
    public Transform targetTransform; // O Transform do personagem a ser animado
    public float moveDistance = 15f; // Dist�ncia maior para frente
    public float moveDuration = 3f; // Dura��o do movimento para frente
    public float jumpHeight = 20f; // Pulo significativamente mais alto
    public float jumpDuration = 1f; // Dura��o maior para o pulo

    void Start()
    {
        // Verifica se o Transform foi atribu�do
        if (targetTransform != null)
        {
            StartWalkingAndJumping();
        }
        else
        {
            Debug.LogError("Nenhum Transform foi atribu�do!");
        }
    }

    void StartWalkingAndJumping()
    {
        // Movimento de andar para frente e voltar
        targetTransform.DOMoveX(targetTransform.position.x + moveDistance, moveDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.Linear);

        // Movimento de pular bem mais alto e descer
        targetTransform.DOLocalMoveY(targetTransform.localPosition.y + jumpHeight, jumpDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.OutQuad); // Ajuste no easing para um pulo mais suave
    }
}
