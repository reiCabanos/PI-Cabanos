using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destruirmiveis : MonoBehaviour
{
    private float _speed;
    private float _endPosX;
    private NuvemManager nuvemManager; // Refer�ncia ao NuvemManager

    void Start()
    {
        // Obter refer�ncia ao NuvemManager (atrav�s de FindObjectOfType, GetComponentInParent etc.)
        nuvemManager = FindObjectOfType<NuvemManager>();
    }

    public void StarFloating(float speed, float endPosx)
    {
        _speed = speed;
        _endPosX = endPosx;

        // Obter uma nuvem do pool de objetos
        GameObject nuvem = nuvemManager.GetCloudFromPool(8);

        // Verificar se a nuvem foi obtida
        if (nuvem != null)
        {
            nuvem.transform.position = transform.position; // Posicionar a nuvem
            nuvem.SetActive(true); // Ativar a nuvem
        }
    }

    void Update()
    {
        transform.Translate(Vector3.right * (Time.deltaTime * _speed));

        if (transform.position.x > _endPosX)
        {
            // Desativar a nuvem (atrav�s do pool de objetos)
            nuvemManager.DesativarNuvem(gameObject); // Passar a pr�pria nuvem para desativa��o
        }
    }
}
