using System.Collections; // Faz referência à biblioteca padrão do C# para coleções, usada para listas e arrays. 
using System.Collections.Generic; // Faz referência à biblioteca para o uso de coleções genéricas, como List<T>.
using TMPro; // Faz referência ao TextMeshPro, usado para manipulação de texto na Unity.
using UnityEngine; // Faz referência à biblioteca principal da Unity, usada para funcionalidades de jogo.

public class ControleDeSom : MonoBehaviour
{
    // Variáveis do menu de som
    public List<AudioSource> _sonsMenu = new List<AudioSource>(); // Referencia objetos AudioSource na cena da Unity.
    public List<AudioSource> _sonsMusicas = new List<AudioSource>(); // Referencia objetos AudioSource na cena da Unity.
    public List<AudioSource> _sonsGames = new List<AudioSource>(); // Referencia objetos AudioSource na cena da Unity.

    [SerializeField] bool _somMenuMute; // Não faz referência externa, usada localmente.
    [SerializeField] bool _somMusicaMute; // Não faz referência externa, usada localmente.
    [SerializeField] bool _somMuteMult; // Não faz referência externa, usada localmente.

    [SerializeField] TextMeshProUGUI _textSomMenu; // Referencia um objeto TextMeshProUGUI na cena da Unity.
    [SerializeField] TextMeshProUGUI _textSomMusica; // Referencia um objeto TextMeshProUGUI na cena da Unity.
    [SerializeField] TextMeshProUGUI _textSomMult; // Referencia um objeto TextMeshProUGUI na cena da Unity.

    // Sons de Movimentação por Terreno
    public AudioSource somPassoGrama; // Som de passos na grama
    public AudioSource somPassoPedra; // Som de passos na pedra
    public AudioSource somPassoAgua; // Som de passos na água
    private AudioSource somAtual; // Som atualmente em execução
    private bool isWalking = false; // Indica se o jogador está andando
    public CharacterController characterController; // Controlador do personagem
    [SerializeField] TextMeshProUGUI textSomEstado; // Texto de estado do som

    void Start()
    {
        somAtual = somPassoGrama; // Definir som padrão para grama
    }

    void Update()
    {
        // Verificar se o jogador está andando
        isWalking = characterController.velocity.magnitude > 0.1f;

        // Controle do som de passos
        if (isWalking && !somAtual.isPlaying)
        {
            somAtual.Play();
        }
        else if (!isWalking)
        {
            somAtual.Stop();
        }

        // Atualizar texto na UI
        AtualizarTextoSom();
    }

    // Método para detectar quando o jogador entra em uma superfície diferente
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Grama"))
        {
            TrocarSom(somPassoGrama, "Grama");
        }
        else if (other.CompareTag("Pedra"))
        {
            TrocarSom(somPassoPedra, "Pedra");
        }
        else if (other.CompareTag("Agua"))
        {
            TrocarSom(somPassoAgua, "Água");
        }
    }

    // Método para trocar o som atual
    private void TrocarSom(AudioSource novoSom, string tipoTerreno)
    {
        if (somAtual.isPlaying) somAtual.Stop(); // Para o som atual
        somAtual = novoSom; // Atualiza para o novo som
        textSomEstado.text = $"Andando em: {tipoTerreno}"; // Atualiza o texto UI
    }

    // Atualiza o texto da interface de usuário com o estado do som atual
    private void AtualizarTextoSom()
    {
        textSomEstado.text = isWalking ? $"Andando em: {somAtual.clip.name}" : "Parado";
    }

    // Métodos para controlar o som do menu
    public void ListSomMenu()
    {
        _somMenuMute = !_somMenuMute;
        _textSomMenu.text = _somMenuMute ? "Mute" : "Som"; // Atualiza texto
        foreach (var som in _sonsMenu) som.mute = _somMenuMute; // Altera estado mute
    }

    // Métodos para controlar o som da música
    public void ListSomMusica()
    {
        _somMusicaMute = !_somMusicaMute;
        _textSomMusica.text = _somMusicaMute ? "Som" : "Mute"; // Atualiza texto

        if (_somMusicaMute)
        {
            foreach (var musica in _sonsMusicas)
                musica.Play(); // Toca todas as músicas
        }
        else
        {
            foreach (var musica in _sonsMusicas)
                musica.Stop(); // Para todas as músicas
        }
    }

    // Métodos para controlar o som do jogo
    public void ListSomMults()
    {
        _somMuteMult = !_somMuteMult;
        _textSomMult.text = _somMuteMult ? "Multado Bola" : "Som Bola"; // Atualiza texto

        foreach (var som in _sonsGames)
            som.mute = _somMuteMult; // Altera estado mute
    }
}
