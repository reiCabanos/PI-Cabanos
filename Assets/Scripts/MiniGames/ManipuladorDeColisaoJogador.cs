using UnityEngine;
using UnityEngine.InputSystem;

public class ManipuladorDeColisaoJogador : MonoBehaviour
{
    private GerenciadorJogo gameManager;
    private PlayerInput playerInput;
    private int playerID;

    void Start()
    {
        // Encontra o GerenciadorJogo na cena
        gameManager = FindObjectOfType<GerenciadorJogo>();
        playerInput = GetComponent<PlayerInput>();

        if (playerInput != null)
        {
            playerID = playerInput.playerIndex + 1;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EliminationZone") && gameManager != null)
        {
            Debug.Log($"Jogador {playerID} entrou na EliminationZone.");
            gameManager.OnPlayerHitTrigger(playerID);
        }
    }
}
