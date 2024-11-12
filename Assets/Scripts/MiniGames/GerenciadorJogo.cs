using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class GerenciadorJogo : MonoBehaviour
{
    [Header("Player Settings")]
    public int playerLives = 3;

    [Header("UI References")]
    public Button startGameButton;
    public TextMeshProUGUI player1LivesText;
    public TextMeshProUGUI player2LivesText;
    public TextMeshProUGUI gameStateText;
    public TextMeshProUGUI timerText;

    [Header("Game Settings")]
    public float initialProblemDuration = 5.0f;
    public float minimumProblemDuration = 2.0f;
    public float timeDecreaseRate = 0.2f;
    public float singlePlayerWinTime = 120.0f;

    private GerenciadorMiniGame miniGameManager;
    private BlockSpawner blockSpawner;
    private List<PlayerData> players = new List<PlayerData>();
    private bool gameInProgress = false;
    private float gameTimer = 0.0f;

    private class PlayerData
    {
        public PlayerInput playerInput;
        public int lives;

        public PlayerData(PlayerInput input, int lives)
        {
            playerInput = input;
            this.lives = lives;
        }
    }

    void Start()
    {
        miniGameManager = FindObjectOfType<GerenciadorMiniGame>();
        blockSpawner = Camera.main?.GetComponent<BlockSpawner>();

        SetupInitialUI();

        // Registro dos eventos de entrada e saída de jogadores
        var playerInputManager = PlayerInputManager.instance;
        if (playerInputManager != null)
        {
            playerInputManager.onPlayerJoined += OnPlayerJoined;
            playerInputManager.onPlayerLeft += OnPlayerLeft;
        }

        // Configura o botão de iniciar para chamar o método OnStartButtonClicked
        startGameButton.onClick.AddListener(OnStartButtonClicked);
    }

    private void SetupInitialUI()
    {
        startGameButton.interactable = false;
        UpdateGameStateText("Aguardando jogadores...");
        UpdateLivesDisplay();
        UpdateTimerDisplay();
    }

    // Método chamado quando um jogador se junta à partida
    private void OnPlayerJoined(PlayerInput playerInput)
    {
        players.Add(new PlayerData(playerInput, playerLives));
        UpdateGameStateText($"Jogador {players.Count} entrou! ({players.Count}/{PlayerInputManager.instance.maxPlayerCount})");

        // Ativa o botão iniciar se houver 1 jogador e o jogo ainda não tiver começado
        if (players.Count == 1 && !gameInProgress)
        {
            startGameButton.interactable = true;
        }

        // Inicia o jogo automaticamente se houver exatamente 2 jogadores e o jogo ainda não começou
        if (players.Count == 2 && !gameInProgress)
        {
            IniciarJogo(players.ConvertAll(p => p.playerInput));
        }
    }

    // Método chamado quando um jogador sai da partida
    private void OnPlayerLeft(PlayerInput playerInput)
    {
        var playerToRemove = players.Find(p => p.playerInput == playerInput);
        if (playerToRemove != null)
        {
            players.Remove(playerToRemove);
            UpdateGameStateText($"Jogador saiu! ({players.Count}/{PlayerInputManager.instance.maxPlayerCount})");

            // Verifica se o jogo precisa terminar se todos os jogadores saírem
            if (players.Count == 0 && gameInProgress)
            {
                EndGame(-1); // Empate
            }
        }

        // Atualiza a interatividade do botão "Iniciar" dependendo do número de jogadores
        startGameButton.interactable = players.Count == 1 && !gameInProgress;
    }

    // Método auxiliar para iniciar o jogo ao clicar no botão
    public void OnStartButtonClicked()
    {
        // Inicia o jogo apenas se houver 1 jogador e o jogo ainda não tiver começado
        if (players.Count == 1 && !gameInProgress)
        {
            IniciarJogo(players.ConvertAll(p => p.playerInput));
        }
    }

    // Inicia o jogo quando o número de jogadores está completo ou quando o botão "Iniciar" é pressionado
    public void IniciarJogo(List<PlayerInput> playerInputs)
    {
        if (gameInProgress) return;

        players.Clear();

        foreach (var playerInput in playerInputs)
        {
            players.Add(new PlayerData(playerInput, playerLives));
        }

        gameInProgress = true;
        PlayerInputManager.instance.DisableJoining(); // Impede novos jogadores de entrar
        UpdateGameStateText("Jogo em andamento!");
        startGameButton.interactable = false;

        miniGameManager.problemDuration = initialProblemDuration;
        miniGameManager.StartGame();
        StartCoroutine(DecreaseProblemDuration());

        if (players.Count == 1)
        {
            gameTimer = singlePlayerWinTime;
            StartCoroutine(GameTimerCountdown());
        }

        Debug.Log("Jogo iniciado com " + players.Count + " jogadores.");
        UpdateLivesDisplay();
    }

    // Notifica que um jogador foi atingido, chamado pelo ManipuladorDeColisaoJogador
    public void OnPlayerHitTrigger(int playerID)
    {
        if (gameInProgress && playerID > 0 && playerID <= players.Count)
        {
            PlayerData player = players[playerID - 1];
            player.lives--;
            UpdateLivesDisplay();

            if (player.lives <= 0)
            {
                EliminatePlayer(player);
            }
        }
    }

    private void EliminatePlayer(PlayerData player)
    {
        Debug.Log($"Jogador {players.IndexOf(player) + 1} foi eliminado.");
        player.playerInput.DeactivateInput();
        OnPlayerLeft(player.playerInput); // Chama OnPlayerLeft para atualizar a lista
        CheckGameEnd();
    }

    private void CheckGameEnd()
    {
        if (players.Count == 1 && gameInProgress)
        {
            EndGame(players[0].playerInput.playerIndex + 1);
        }
        else if (players.Count == 0)
        {
            EndGame(-1); // Empate
        }
    }

    private void EndGame(int winnerID)
    {
        gameInProgress = false;
        string endMessage = winnerID > 0 ? $"Jogador {winnerID} venceu!" : "Empate!";
        UpdateGameStateText(endMessage);
        miniGameManager.enabled = false;
        blockSpawner.enabled = false;

        foreach (var player in players)
        {
            player.playerInput.DeactivateInput();
        }

        PlayerInputManager.instance.EnableJoining(); // Permite novos jogadores para a próxima partida
        startGameButton.interactable = true;
        Debug.Log("Jogo finalizado.");
    }

    private IEnumerator GameTimerCountdown()
    {
        while (gameInProgress && gameTimer > 0)
        {
            yield return new WaitForSeconds(1f);
            gameTimer--;
            UpdateTimerDisplay();

            if (players.Count == 1 && gameTimer <= 0)
            {
                EndGame(players[0].playerInput.playerIndex + 1);
                yield break;
            }
        }
    }

    private void UpdateLivesDisplay()
    {
        player1LivesText.text = players.Count > 0 ? $"P1 Vidas: {players[0].lives}" : "P1 Vidas: 0";
        player2LivesText.text = players.Count > 1 ? $"P2 Vidas: {players[1].lives}" : "P2 Vidas: 0";
    }

    private void UpdateGameStateText(string message)
    {
        if (gameStateText != null)
            gameStateText.text = message;
    }

    private void UpdateTimerDisplay()
    {
        if (timerText != null)
            timerText.text = $"Tempo restante: {gameTimer:F0} segundos";
    }

    private IEnumerator DecreaseProblemDuration()
    {
        while (gameInProgress && miniGameManager.problemDuration > minimumProblemDuration)
        {
            yield return new WaitForSeconds(10f);
            miniGameManager.problemDuration = Mathf.Max(
                miniGameManager.problemDuration - timeDecreaseRate,
                minimumProblemDuration
            );
        }
    }
}
