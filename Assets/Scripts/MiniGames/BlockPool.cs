using System.Collections.Generic;
using UnityEngine;

public class BlockPool : MonoBehaviour
{
    public static BlockPool SharedInstance; // Singleton para acesso global

    public List<GameObject> pooledObjects; // Lista de objetos poolados
    public GameObject objectToPool; // Prefab do bloco a ser poolado
    public int amountToPool; // Quantidade de objetos a serem poolados

    void Awake()
    {
        // Configura��o do singleton
        SharedInstance = this;
    }

    void Start()
    {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = Instantiate(objectToPool);
            obj.SetActive(false); // Inicialmente, mant�m todos os objetos inativos
            pooledObjects.Add(obj); // Adiciona o objeto � lista do pool
        }
        Debug.Log($"{amountToPool} objetos instanciados e adicionados ao pool.");
    }

    // Fun��o para obter um objeto inativo do pool
    public GameObject GetPooledObject()
    {
        foreach (GameObject obj in pooledObjects)
        {
            if (!obj.activeInHierarchy)
            {
                Debug.Log("Objeto inativo encontrado e retornado.");
                return obj; // Retorna o primeiro objeto inativo encontrado
            }
        }
        Debug.LogWarning("Nenhum objeto inativo encontrado no pool.");
        return null; // Se todos os objetos est�o ativos, retorna null
    }
}
