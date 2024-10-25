using UnityEngine;
using DG.Tweening; // Certifique-se de importar a biblioteca DOTween
using TMPro; // Certifique-se de importar a biblioteca TextMeshPro

public class Efeito3D : MonoBehaviour
{
    public Transform targetTransform; // Arraste o objeto Transform aqui no Unity
    public float rotationDuration = 0.5f; // Duração de cada movimento de rotação
    public float movementDuration = 0.5f; // Duração de cada movimento de subida/descida
    public float rotationAngle = 15f; // Ângulo de rotação
    public float movementDistance = 0.5f; // Distância de movimento para cima/baixo

    void Start()
    {
        // Verifica se o Transform foi atribuído
        if (targetTransform != null)
        {
            StartRotatingAndMoving();
        }
        else
        {
            Debug.LogError("Nenhum Transform foi atribuído!");
        }
    }

    void StartRotatingAndMoving()
    {
        // Anima a rotação em loop
        targetTransform.DORotate(new Vector3(0, rotationAngle, 0), rotationDuration)
            .SetLoops(-1, LoopType.Yoyo);

        // Anima o movimento para cima e para baixo em loop
        targetTransform.DOLocalMoveY(targetTransform.localPosition.y + movementDistance, movementDuration)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
