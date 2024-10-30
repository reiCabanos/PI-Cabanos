using UnityEngine;
using UnityEngine.InputSystem;

public class ManipuladorDeColisaoJogador : MonoBehaviour
{
    private GerenciadorJogo gameManager;
    private PlayerInput playerInput;

    void Start()
    {
        // Encontra o GerenciadorJogo na cena
        gameManager = FindObjectOfType<GerenciadorJogo>();
        playerInput = GetComponent<PlayerInput>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Verifica se o jogador entrou na EliminationZone
        if (other.CompareTag("EliminationZone") && gameManager != null)
        {
            int playerID = playerInput.playerIndex + 1;  // Usa o índice do jogador
            gameManager.OnPlayerHitTrigger(playerID);    // Notifica o GerenciadorJogo
        }
    }
}
