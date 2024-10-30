using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GerenciadorDeJogadores : MonoBehaviour
{
    public PlayerInputManager inputManager;
    public int maxPlayers = 2;

    public delegate void PlayersReadyEvent(List<PlayerInput> players);
    public event PlayersReadyEvent OnPlayersReady;

    public delegate void PlayerListUpdatedEvent(List<PlayerInput> players);
    public event PlayerListUpdatedEvent OnPlayerListUpdated;

    private List<PlayerInput> players = new List<PlayerInput>();

    void Start()
    {
        if (inputManager == null)
        {
            inputManager = FindObjectOfType<PlayerInputManager>();
        }

        if (inputManager != null)
        {
            inputManager.onPlayerJoined += OnPlayerJoined;
            inputManager.onPlayerLeft += OnPlayerLeft;
        }
        else
        {
            Debug.LogError("PlayerInputManager não encontrado!");
        }
    }

    private void OnPlayerJoined(PlayerInput player)
    {
        players.Add(player);
        Debug.Log($"Jogador {players.Count} entrou na partida!");

        if (players.Count == maxPlayers)
        {
            OnPlayersReady?.Invoke(players); // Dispara o evento para iniciar o jogo
        }

        OnPlayerListUpdated?.Invoke(players); // Atualiza a lista de jogadores
    }

    private void OnPlayerLeft(PlayerInput player)
    {
        players.Remove(player);
        Debug.Log("Um jogador saiu da partida.");

        OnPlayerListUpdated?.Invoke(players); // Atualiza a lista de jogadores
    }

    void OnDestroy()
    {
        if (inputManager != null)
        {
            inputManager.onPlayerJoined -= OnPlayerJoined;
            inputManager.onPlayerLeft -= OnPlayerLeft;
        }
    }
}
