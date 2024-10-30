using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class GerenciadorJogo : MonoBehaviour
{
    [Header("Player Settings")]
    public int maxPlayers = 2;
    public int playerLives = 3;

    [Header("UI References")]
    public Button startGameButton;
    public TextMeshProUGUI player1LivesText;
    public TextMeshProUGUI player2LivesText;
    public TextMeshProUGUI gameStateText;

    [Header("Game Settings")]
    public float initialProblemDuration = 5.0f;
    public float minimumProblemDuration = 2.0f;
    public float timeDecreaseRate = 0.2f;

    [Header("Multiplayer References")]
    [SerializeField] private PlayerInputManager inputManager;

    private GerenciadorMiniGame miniGameManager;
    private BlockSpawner blockSpawner;
    private Dictionary<int, int> playerLivesRemaining = new Dictionary<int, int>();
    private bool gameInProgress = false;

    // Lista para controlar os jogadores ativos
    private List<PlayerInput> activePlayers = new List<PlayerInput>();

    void Start()
    {
        // Initialize references
        miniGameManager = FindObjectOfType<GerenciadorMiniGame>();
        blockSpawner = Camera.main.GetComponent<BlockSpawner>();

        // Encontrar o PlayerInputManager se não foi atribuído no Inspector
        if (inputManager == null)
            inputManager = FindObjectOfType<PlayerInputManager>();

        // Setup initial state
        startGameButton.interactable = false;
        UpdateGameStateText("Waiting for players...");

        // Initialize player lives
        for (int i = 1; i <= maxPlayers; i++)
        {
            playerLivesRemaining[i] = playerLives;
        }

        UpdateLivesDisplay();

        // Registrar callbacks do PlayerInputManager
        if (inputManager != null)
        {
            inputManager.onPlayerJoined += HandlePlayerJoined;
            inputManager.onPlayerLeft += HandlePlayerLeft;
        }
        else
        {
            Debug.LogError("PlayerInputManager não encontrado na cena!");
        }
    }

    void OnDestroy()
    {
        // Remover callbacks quando o objeto for destruído
        if (inputManager != null)
        {
            inputManager.onPlayerJoined -= HandlePlayerJoined;
            inputManager.onPlayerLeft -= HandlePlayerLeft;
        }
    }

    private void HandlePlayerJoined(PlayerInput playerInput)
    {
        if (activePlayers.Count < maxPlayers && !gameInProgress)
        {
            activePlayers.Add(playerInput);
            int playerID = activePlayers.Count;

            // Configurar o PlayerInput com o ID do jogador
            playerInput.gameObject.name = $"Player{playerID}";

            UpdateGameStateText($"Player {playerID} connected! ({activePlayers.Count}/{maxPlayers})");

            if (activePlayers.Count >= maxPlayers)
            {
                startGameButton.interactable = true;
                UpdateGameStateText("Press Start to begin!");
            }
        }
    }

    private void HandlePlayerLeft(PlayerInput playerInput)
    {
        if (gameInProgress)
        {
            int playerID = activePlayers.IndexOf(playerInput) + 1;
            EliminatePlayer(playerID);
        }

        activePlayers.Remove(playerInput);
        UpdateGameStateText($"Player left! ({activePlayers.Count}/{maxPlayers})");

        if (activePlayers.Count < maxPlayers)
        {
            startGameButton.interactable = false;
        }
    }

    public void StartGame()
    {
        if (activePlayers.Count >= maxPlayers && !gameInProgress)
        {
            gameInProgress = true;
            startGameButton.interactable = false;
            UpdateGameStateText("Game in progress!");

            // Start the mini-game
            miniGameManager.problemDuration = initialProblemDuration;
            miniGameManager.StartGame();

            // Start decreasing the problem duration over time
            StartCoroutine(DecreaseProblemDuration());
        }
    }

    public void OnPlayerHitTrigger(int playerID)
    {
        if (gameInProgress && playerLivesRemaining.ContainsKey(playerID))
        {
            playerLivesRemaining[playerID]--;
            UpdateLivesDisplay();

            // Check if player is eliminated
            if (playerLivesRemaining[playerID] <= 0)
            {
                EliminatePlayer(playerID);
            }
        }
    }

    private void EliminatePlayer(int playerID)
    {
        // Desativar o input do jogador eliminado
        if (playerID <= activePlayers.Count)
        {
            PlayerInput playerInput = activePlayers[playerID - 1];
            playerInput.DeactivateInput();
        }

        if (activePlayers.Count <= 1)
        {
            // Find the winning player
            int winner = 0;
            foreach (var player in playerLivesRemaining)
            {
                if (player.Value > 0)
                {
                    winner = player.Key;
                    break;
                }
            }

            EndGame(winner);
        }
    }

    private void EndGame(int winnerID)
    {
        gameInProgress = false;
        UpdateGameStateText($"Player {winnerID} wins!");
        miniGameManager.enabled = false;
        blockSpawner.enabled = false;

        // Desativar input de todos os jogadores
        foreach (var player in activePlayers)
        {
            player.DeactivateInput();
        }
    }

    private void UpdateLivesDisplay()
    {
        if (player1LivesText != null)
            player1LivesText.text = $"P1 Lives: {playerLivesRemaining[1]}";
        if (player2LivesText != null)
            player2LivesText.text = $"P2 Lives: {playerLivesRemaining[2]}";
    }

    private void UpdateGameStateText(string message)
    {
        if (gameStateText != null)
            gameStateText.text = message;
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