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
    private const int ROWS = 6;  // Alterado de 8 para 6
    private const int COLS = 8;
    private const int TOTAL_BLOCKS = ROWS * COLS;  // Agora s�o 48 blocos

    // Cores vivas predefinidas para os n�meros
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
        new Color(0.6f, 1f, 0.2f),     // Verde-lim�o
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

        // Inicializa as cores para cada n�mero (1-10)
        for (int i = 1; i <= 10; i++)
        {
            numberColors[i] = vividColors[i - 1];
        }

        // Preenche a lista com n�meros de 1 a 10 repetidos
        for (int i = 0; i < TOTAL_BLOCKS; i++)  // Agora loop at� 48
        {
            int number = (i % 10) + 1; // Gera n�meros de 1 a 10 ciclicamente
            randomNumbers.Add(number);
        }

        // Embaralha os n�meros para distribui��o aleat�ria
        ShuffleList(randomNumbers);

        // Organiza os blocos
        ArrangeInitialBlocks();
    }

    private void ArrangeInitialBlocks()
    {
        int index = 0;
        if (BlockPool.SharedInstance == null || BlockPool.SharedInstance.pooledObjects == null)
        {
            Debug.LogError("BlockPool n�o est� pronto!");
            return;
        }

        foreach (GameObject block in BlockPool.SharedInstance.pooledObjects)
        {
            if (block != null && index < TOTAL_BLOCKS)  // Garante que s� 48 blocos sejam posicionados
            {
                // Calcula a posi��o na grade 6x8
                int row = index / COLS;
                int col = index % COLS;
                float x = spawnPosition.x + col * spacing;
                float z = spawnPosition.z + row * spacing;

                // Posiciona o bloco
                block.transform.position = new Vector3(x, spawnPosition.y, z);
                block.transform.rotation = spawnRotation;

                // Define o n�mero inicial (1-10)
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
            else if (block != null)
            {
                block.SetActive(false);  // Desativa os blocos extras
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

        // Mant�m as mesmas cores vivas para os n�meros
        for (int i = 0; i < TOTAL_BLOCKS; i++)  // Agora gera 48 n�meros
        {
            int randomNumber = Random.Range(1, 11); // N�meros de 1 a 10
            randomNumbers.Add(randomNumber);
        }
        ShuffleList(randomNumbers);
    }

    public void ResetSpawner()
    {
        foreach (GameObject block in BlockPool.SharedInstance.pooledObjects)
        {
            if (block != null)
            {
                block.SetActive(false);  // Desativa todos os blocos
            }
        }
        InitializeBlocks(); // Reorganiza os blocos para a configura��o inicial
    }


    private void ArrangeBlocks()
    {
        int index = 0;
        foreach (GameObject block in BlockPool.SharedInstance.pooledObjects)
        {
            if (block != null && index < TOTAL_BLOCKS)  // Garante que s� 48 blocos sejam posicionados
            {
                int row = index / COLS;
                int col = index % COLS;
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
            else if (block != null)
            {
                block.SetActive(false);  // Desativa os blocos extras
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