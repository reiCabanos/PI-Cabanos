using UnityEngine;
using DG.Tweening; // Certifique-se de importar a biblioteca DOTween
using TMPro; // Certifique-se de importar a biblioteca TextMeshPro

public class Efeito3D : MonoBehaviour
{
    public Transform targetTransform; // Arraste o objeto Transform aqui no Unity
    public float rotationDuration = 0.5f; // Dura��o de cada movimento de rota��o
    public float movementDuration = 0.5f; // Dura��o de cada movimento de subida/descida
    public float rotationAngle = 15f; // �ngulo de rota��o
    public float movementDistance = 0.5f; // Dist�ncia de movimento para cima/baixo

    void Start()
    {
        // Verifica se o Transform foi atribu�do
        if (targetTransform != null)
        {
            StartRotatingAndMoving();
        }
        else
        {
            Debug.LogError("Nenhum Transform foi atribu�do!");
        }
    }

    void StartRotatingAndMoving()
    {
        // Anima a rota��o em loop
        targetTransform.DORotate(new Vector3(0, rotationAngle, 0), rotationDuration)
            .SetLoops(-1, LoopType.Yoyo);

        // Anima o movimento para cima e para baixo em loop
        targetTransform.DOLocalMoveY(targetTransform.localPosition.y + movementDistance, movementDuration)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
