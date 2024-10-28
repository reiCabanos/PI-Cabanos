using UnityEngine;
using TMPro;
using System.Collections;

public class GerenciadorMiniGame : MonoBehaviour
{
    public TextMeshProUGUI problemText; // Texto para exibir o problema matem�tico
    public TextMeshProUGUI timerText; // Texto para exibir o temporizador
    public float problemDuration = 5.0f; // Tempo para resolver o problema (ex.: 5 segundos)

    private BlockSpawner blockSpawner; // Refer�ncia ao BlockSpawner
    private int correctAnswer; // Armazena a resposta correta
    private int previousAnswer = -1; // Armazena a resposta anterior para evitar repeti��es
    private float remainingTime; // Tempo restante para responder
    private bool problemActive = false; // Verifica se o problema est� ativo

    void Start()
    {
        // Obtem o BlockSpawner diretamente da C�mera Principal
        blockSpawner = Camera.main.GetComponent<BlockSpawner>();

        if (blockSpawner == null)
        {
            Debug.LogError("BlockSpawner n�o encontrado na C�mera Principal!");
            return;
        }

        GenerateProblem(); // Gera o primeiro problema
    }

    void GenerateProblem()
    {
        int num1, num2;

        do
        {
            // Gera dois n�meros aleat�rios com soma m�xima de 10
            num1 = Random.Range(0, 10);
            num2 = Random.Range(0, 10 - num1);
            correctAnswer = num1 + num2;

        } while (correctAnswer == previousAnswer); // Garante que o resultado � diferente do anterior

        // Atualiza o resultado anterior
        previousAnswer = correctAnswer;

        // Exibe o problema no texto
        problemText.text = $"{num1} + {num2}";

        // Inicia o temporizador
        remainingTime = problemDuration;
        problemActive = true;

        // Chama a fun��o para randomizar os n�meros dos blocos
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
        // Desativa todos os blocos, exceto o que cont�m a resposta correta
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
        // Aguarda um curto per�odo antes de gerar o pr�ximo problema
        yield return new WaitForSeconds(2.0f);
        GenerateProblem();
    }
}
