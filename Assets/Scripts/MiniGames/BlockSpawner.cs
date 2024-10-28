using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class BlockSpawner : MonoBehaviour
{
    public Vector3 spawnPosition;
    public Quaternion spawnRotation = Quaternion.identity;
    public float spacing = 1.5f;
    private List<int> randomNumbers;
    private Dictionary<int, Color> numberColors;
    private bool gameStarted = false;

    // Cores vivas predefinidas para os números
    private readonly Color[] vividColors = new Color[]
    {
        new Color(1f, 0.2f, 0.2f),     // Vermelho vivo
        new Color(0.2f, 1f, 0.2f),     // Verde vivo
        new Color(0.2f, 0.2f, 1f),     // Azul vivo
        new Color(1f, 1f, 0.2f),       // Amarelo vivo
        new Color(1f, 0.2f, 1f),       // Rosa vivo
        new Color(0.2f, 1f, 1f),       // Ciano vivo
        new Color(1f, 0.6f, 0.2f),     // Laranja vivo
        new Color(0.8f, 0.2f, 0.8f),   // Roxo vivo
        new Color(0.6f, 1f, 0.2f),     // Verde-limão
        new Color(0.2f, 0.6f, 1f)      // Azul claro
    };

    void Start()
    {
        StartCoroutine(InitializeBlocksDelayed());
    }

    System.Collections.IEnumerator InitializeBlocksDelayed()
    {
        yield return null;
        InitializeBlocks();
    }

    public void InitializeBlocks()
    {
        randomNumbers = new List<int>();
        numberColors = new Dictionary<int, Color>();

        // Inicializa as cores para cada número (1-10)
        for (int i = 1; i <= 10; i++)
        {
            numberColors[i] = vividColors[i - 1];
        }

        // Preenche a lista com números de 1 a 10 repetidos
        for (int i = 0; i < 64; i++)
        {
            int number = (i % 10) + 1; // Gera números de 1 a 10 ciclicamente
            randomNumbers.Add(number);
        }

        // Embaralha os números para distribuição aleatória
        ShuffleList(randomNumbers);

        // Organiza os blocos
        ArrangeInitialBlocks();
    }

    private void ArrangeInitialBlocks()
    {
        int index = 0;
        if (BlockPool.SharedInstance == null || BlockPool.SharedInstance.pooledObjects == null)
        {
            Debug.LogError("BlockPool não está pronto!");
            return;
        }

        foreach (GameObject block in BlockPool.SharedInstance.pooledObjects)
        {
            if (block != null)
            {
                // Calcula a posição na grade 8x8
                int row = index / 8;
                int col = index % 8;
                float x = spawnPosition.x + col * spacing;
                float z = spawnPosition.z + row * spacing;

                // Posiciona o bloco
                block.transform.position = new Vector3(x, spawnPosition.y, z);
                block.transform.rotation = spawnRotation;

                // Define o número inicial (1-10)
                int number = randomNumbers[index];
                TextMeshPro textMesh = block.GetComponentInChildren<TextMeshPro>();
                if (textMesh != null)
                {
                    textMesh.text = number.ToString();
                }

                // Define a cor viva correspondente
                MeshRenderer renderer = block.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    renderer.material.color = numberColors[number];
                }

                block.SetActive(true);
                index++;
            }
        }
    }

    public void StartGame()
    {
        if (!gameStarted)
        {
            gameStarted = true;
            RandomizeBlocks();
        }
    }

    public void RandomizeBlocks()
    {
        if (!gameStarted) return;
        GenerateRandomNumbers();
        ArrangeBlocks();
    }

    private void GenerateRandomNumbers()
    {
        randomNumbers.Clear();

        // Mantém as mesmas cores vivas para os números
        for (int i = 0; i < 64; i++)
        {
            int randomNumber = Random.Range(1, 11); // Números de 1 a 10
            randomNumbers.Add(randomNumber);
        }
        ShuffleList(randomNumbers);
    }

    private void ArrangeBlocks()
    {
        int index = 0;
        foreach (GameObject block in BlockPool.SharedInstance.pooledObjects)
        {
            if (block != null)
            {
                int row = index / 8;
                int col = index % 8;
                float x = spawnPosition.x + col * spacing;
                float z = spawnPosition.z + row * spacing;

                block.transform.position = new Vector3(x, spawnPosition.y, z);
                block.transform.rotation = spawnRotation;

                int number = randomNumbers[index];
                TextMeshPro textMesh = block.GetComponentInChildren<TextMeshPro>();
                if (textMesh != null)
                {
                    textMesh.text = number.ToString();
                }

                MeshRenderer renderer = block.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    renderer.material.color = numberColors[number];
                }

                block.SetActive(true);
                index++;
            }
        }
    }

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
}