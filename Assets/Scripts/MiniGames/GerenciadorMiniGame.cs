using DG.Tweening; // Certifique-se de ter o DOTween instalado
using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class GerenciadorMiniGame : MonoBehaviour
{
    public TextMeshProUGUI problemText;
    public TextMeshProUGUI timerText;
    public float problemDuration = 5.0f;
    private BlockSpawner blockSpawner;
    private int correctAnswer;
    private int previousAnswer = -1;
    private float remainingTime;
    private bool problemActive = false;
    private bool gameStarted = false;
    public List<ManipuladorDeColisaoJogador> _manipuladorDeColisaoJogadors;

    void Start()
    {
        blockSpawner = Camera.main.GetComponent<BlockSpawner>();
        if (blockSpawner == null)
        {
            Debug.LogError("BlockSpawner n�o encontrado na C�mera Principal!");
            return;
        }

        // Esconde os textos inicialmente
        if (problemText != null) problemText.gameObject.SetActive(false);
        if (timerText != null) timerText.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        if (!gameStarted)
        {
            gameStarted = true;
            // Ativa os textos
            if (problemText != null) problemText.gameObject.SetActive(true);
            if (timerText != null) timerText.gameObject.SetActive(true);

            blockSpawner.StartGame();
            GenerateProblem();
        }
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

        // Adiciona a anima��o de pulsar
        AnimateProblemText();

        // Chama a fun��o para randomizar os n�meros dos blocos
        blockSpawner.RandomizeBlocks();

        Debug.Log($"Problema gerado: {problemText.text}, Resposta correta: {correctAnswer}");
        Debug.Log("Gera blocos ");
        Invoke("Chamarjogadores", 1);
    }

    void AnimateProblemText()
    {
        if (problemText != null)
        {
            // Faz o texto pulsar entre 1x e 1.2x no eixo X e Y, em loop
            problemText.transform.DOScale(new Vector3(1.2f, 1.2f, 1), 0.5f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }
    }

    void Chamarjogadores()
    {
        for (int i = 0; i < _manipuladorDeColisaoJogadors.Count; i++)
        {
            _manipuladorDeColisaoJogadors[i].VoltarPlayer();
        }
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
