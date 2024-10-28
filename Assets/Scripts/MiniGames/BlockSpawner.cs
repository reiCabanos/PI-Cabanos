using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class BlockSpawner : MonoBehaviour
{
    public Vector3 spawnPosition; // Posição inicial para o primeiro bloco
    public Quaternion spawnRotation = Quaternion.identity; // Rotação padrão para os blocos
    public float spacing = 1.5f; // Espaço entre os blocos

    private List<int> randomNumbers; // Lista para armazenar números randomizados
    private Dictionary<int, Color> numberColors; // Dicionário para armazenar a cor de cada número

    void Start()
    {
        GenerateRandomNumbers(); // Inicializa os números aleatórios
        ArrangeBlocks(); // Organiza os blocos
    }

    // Função para gerar e randomizar números para os blocos
    public void RandomizeBlocks()
    {
        GenerateRandomNumbers(); // Gera novos números aleatórios
        ArrangeBlocks(); // Redistribui os números nos blocos com as novas cores
    }

    private void GenerateRandomNumbers()
    {
        randomNumbers = new List<int>();
        numberColors = new Dictionary<int, Color>();

        // Gera números aleatórios de 0 a 9 até preencher os 64 blocos
        for (int i = 0; i < 64; i++)
        {
            int randomNumber = Random.Range(0, 10);
            randomNumbers.Add(randomNumber);

            // Atribui uma cor se o número ainda não tiver uma
            if (!numberColors.ContainsKey(randomNumber))
            {
                numberColors[randomNumber] = RandomColor();
            }
        }

        // Embaralha a lista para distribuir os números de forma aleatória
        ShuffleList(randomNumbers);
    }

    private void ArrangeBlocks()
    {
        int index = 0; // Índice para acessar os números e cores randomizados

        foreach (GameObject block in BlockPool.SharedInstance.pooledObjects)
        {
            if (block != null)
            {
                // Define a posição do bloco em uma grade
                float x = spawnPosition.x + (index % 8) * spacing;
                float z = spawnPosition.z + (index / 8) * spacing;
                block.transform.position = new Vector3(x, spawnPosition.y, z);
                block.transform.rotation = spawnRotation;

                // Define o número do bloco e sua cor correspondente
                int number = randomNumbers[index];
                TextMeshPro textMesh = block.GetComponentInChildren<TextMeshPro>();
                if (textMesh != null)
                {
                    textMesh.text = number.ToString();
                }

                MeshRenderer renderer = block.GetComponent<MeshRenderer>();
                if (renderer != null && numberColors.ContainsKey(number))
                {
                    renderer.material.color = numberColors[number];
                }

                block.SetActive(true); // Garante que o bloco está ativo
                index++;
            }
        }
    }

    // Função para embaralhar a lista de números aleatórios
    private void ShuffleList(List<int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    // Função para gerar uma cor aleatória
    private Color RandomColor()
    {
        return new Color(Random.value, Random.value, Random.value);
    }
}
