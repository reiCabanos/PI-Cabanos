using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class BlockSpawner : MonoBehaviour
{
    public Vector3 spawnPosition; // Posi��o inicial para o primeiro bloco
    public Quaternion spawnRotation = Quaternion.identity; // Rota��o padr�o para os blocos
    public float spacing = 1.5f; // Espa�o entre os blocos

    private List<int> randomNumbers; // Lista para armazenar n�meros randomizados
    private Dictionary<int, Color> numberColors; // Dicion�rio para armazenar a cor de cada n�mero

    void Start()
    {
        GenerateRandomNumbers(); // Inicializa os n�meros aleat�rios
        ArrangeBlocks(); // Organiza os blocos
    }

    // Fun��o para gerar e randomizar n�meros para os blocos
    public void RandomizeBlocks()
    {
        GenerateRandomNumbers(); // Gera novos n�meros aleat�rios
        ArrangeBlocks(); // Redistribui os n�meros nos blocos com as novas cores
    }

    private void GenerateRandomNumbers()
    {
        randomNumbers = new List<int>();
        numberColors = new Dictionary<int, Color>();

        // Gera n�meros aleat�rios de 0 a 9 at� preencher os 64 blocos
        for (int i = 0; i < 64; i++)
        {
            int randomNumber = Random.Range(0, 10);
            randomNumbers.Add(randomNumber);

            // Atribui uma cor se o n�mero ainda n�o tiver uma
            if (!numberColors.ContainsKey(randomNumber))
            {
                numberColors[randomNumber] = RandomColor();
            }
        }

        // Embaralha a lista para distribuir os n�meros de forma aleat�ria
        ShuffleList(randomNumbers);
    }

    private void ArrangeBlocks()
    {
        int index = 0; // �ndice para acessar os n�meros e cores randomizados

        foreach (GameObject block in BlockPool.SharedInstance.pooledObjects)
        {
            if (block != null)
            {
                // Define a posi��o do bloco em uma grade
                float x = spawnPosition.x + (index % 8) * spacing;
                float z = spawnPosition.z + (index / 8) * spacing;
                block.transform.position = new Vector3(x, spawnPosition.y, z);
                block.transform.rotation = spawnRotation;

                // Define o n�mero do bloco e sua cor correspondente
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

                block.SetActive(true); // Garante que o bloco est� ativo
                index++;
            }
        }
    }

    // Fun��o para embaralhar a lista de n�meros aleat�rios
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

    // Fun��o para gerar uma cor aleat�ria
    private Color RandomColor()
    {
        return new Color(Random.value, Random.value, Random.value);
    }
}
