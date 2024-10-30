using UnityEngine;
using UnityEngine.InputSystem;

public class ManipuladorDeColisaoJogador : MonoBehaviour
{
    private GerenciadorJogo gameManager;
    private PlayerInput playerInput;
    private int playerID;
    public Vector3 _posicaoinicial;
    bool checkCaiu;
    GerenciadorMiniGame gerenciadorMini;

    void Start()
    {
        // Encontra o GerenciadorJogo na cena
        gameManager = FindObjectOfType<GerenciadorJogo>();
        playerInput = GetComponent<PlayerInput>();
        gerenciadorMini = Camera.main.GetComponent<GerenciadorMiniGame>();
        gerenciadorMini._manipuladorDeColisaoJogadors.Add(gameObject.GetComponent<ManipuladorDeColisaoJogador>());
        _posicaoinicial = transform.position;
        if (playerInput != null)
        {
            playerID = playerInput.playerIndex + 1;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EliminationZone") && gameManager != null && checkCaiu==false)
        {
            Debug.Log($"Jogador {playerID} entrou na EliminationZone.");
            gameManager.OnPlayerHitTrigger(playerID);
            Debug.Log("perde vida");
      
            checkCaiu = true;
        }
    }

    public void VoltarPlayer()
    {
        if (checkCaiu)
        {
            transform.position = _posicaoinicial;
            checkCaiu = false;
        }
    }
}
