using UnityEngine;
using TMPro;
using System.Collections;

public class GerenciadorMiniGame : MonoBehaviour
{
    public TextMeshProUGUI problemText; // Texto para exibir o problema matemático
    public TextMeshProUGUI timerText; // Texto para exibir o temporizador
    public float problemDuration = 5.0f; // Tempo para resolver o problema (ex.: 5 segundos)

    private BlockSpawner blockSpawner; // Referência ao BlockSpawner
    private int correctAnswer; // Armazena a resposta correta
    private int previousAnswer = -1; // Armazena a resposta anterior para evitar repetições
    private float remainingTime; // Tempo restante para responder
    private bool problemActive = false; // Verifica se o problema está ativo

    void Start()
    {
        // Obtem o BlockSpawner diretamente da Câmera Principal
        blockSpawner = Camera.main.GetComponent<BlockSpawner>();

        if (blockSpawner == null)
        {
            Debug.LogError("BlockSpawner não encontrado na Câmera Principal!");
            return;
        }

        GenerateProblem(); // Gera o primeiro problema
    }

    void GenerateProblem()
    {
        int num1, num2;

        do
        {
            // Gera dois números aleatórios com soma máxima de 10
            num1 = Random.Range(0, 10);
            num2 = Random.Range(0, 10 - num1);
            correctAnswer = num1 + num2;

        } while (correctAnswer == previousAnswer); // Garante que o resultado é diferente do anterior

        // Atualiza o resultado anterior
        previousAnswer = correctAnswer;

        // Exibe o problema no texto
        problemText.text = $"{num1} + {num2}";

        // Inicia o temporizador
        remainingTime = problemDuration;
        problemActive = true;

        // Chama a função para randomizar os números dos blocos
        blockSpawner.RandomizeBlocks();

        Debug.Log($"Problema gerado: {problemText.text}, Resposta correta: {correctAnswer}");
    }

    void Update()
    {
        if (problemActive)
        {
            // Atualiza o temporizador
            remainingTime -= Time.deltaTime;
            timerText.text = $"Tempo: {remainingTime:F1} s";

            // Verifica se o tempo acabou
            if (remainingTime <= 0)
            {
                problemActive = false;
                CheckAnswer();
            }
        }
    }

    void CheckAnswer()
    {
        // Desativa todos os blocos, exceto o que contém a resposta correta
        foreach (GameObject block in BlockPool.SharedInstance.pooledObjects)
        {
            TextMeshPro textMesh = block.GetComponentInChildren<TextMeshPro>();
            if (textMesh != null && int.TryParse(textMesh.text, out int blockNumber))
            {
                // Apenas o bloco correto permanece ativo
                if (blockNumber == correctAnswer)
                {
                    block.SetActive(true);
                    Debug.Log("Bloco correto ativado");
                }
                else
                {
                    block.SetActive(false);
                }
            }
        }

        StartCoroutine(WaitAndGenerateNewProblem());
    }

    IEnumerator WaitAndGenerateNewProblem()
    {
        // Aguarda um curto período antes de gerar o próximo problema
        yield return new WaitForSeconds(2.0f);
        GenerateProblem();
    }
}
