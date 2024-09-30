using System.Collections; // Faz referência à biblioteca padrão do C# para coleções, usada para listas e arrays.
using System.Collections.Generic; // Faz referência à biblioteca para o uso de coleções genéricas, como List<T>.
using TMPro; // Faz referência ao TextMeshPro, usado para manipulação de texto na Unity.
using UnityEngine; // Faz referência à biblioteca principal da Unity, usada para funcionalidades de jogo.

public class ControleDeSom : MonoBehaviour
{
    // Start is called before the first frame update
    public List<AudioSource> _sonsMenu = new List<AudioSource>(); // Referencia objetos AudioSource na cena da Unity.
    public List<AudioSource> _sonsMusicas = new List<AudioSource>(); // Referencia objetos AudioSource na cena da Unity.
    public List<AudioSource> _sonsGames = new List<AudioSource>(); // Referencia objetos AudioSource na cena da Unity.

    [SerializeField] bool _somMenuMute; // Não faz referência externa, usada localmente.
    [SerializeField] bool _somMusicaMute; // Não faz referência externa, usada localmente.

    [SerializeField] TextMeshProUGUI _textSomMenu; // Referencia um objeto TextMeshProUGUI na cena da Unity.
    [SerializeField] TextMeshProUGUI _textSomMusica; // Referencia um objeto TextMeshProUGUI na cena da Unity.

    [SerializeField] bool _somMuteMult; // Não faz referência externa, usada localmente.
    [SerializeField] TextMeshProUGUI _textSomMult; // Referencia um objeto TextMeshProUGUI na cena da Unity.

    public void ListSomMenu()
    {
        _somMenuMute = !_somMenuMute;
        if (_somMenuMute)
        {
            _textSomMenu.text = "Mute"; // Manipula o texto do objeto TextMeshProUGUI.
        }
        else
        {
            _textSomMenu.text = "Som"; // Manipula o texto do objeto TextMeshProUGUI.
        }
        for (int i = 0; i < _sonsMenu.Count; i++)
        {
            _sonsMenu[i].mute = _somMenuMute; // Manipula o estado mute dos objetos AudioSource.
        }
    }

    public void ListSomMusica()
    {
        _somMusicaMute = !_somMusicaMute;
        if (_somMusicaMute)
        {
            _textSomMusica.text = "Som"; // Manipula o texto do objeto TextMeshProUGUI.

            for (int i = 0; i < _sonsMusicas.Count; i++)
            {
                _sonsMusicas[i].Play(); // Chama o método Play dos objetos AudioSource.
            }
        }
        else
        {
            _textSomMusica.text = "Mute"; // Manipula o texto do objeto TextMeshProUGUI.

            for (int i = 0; i < _sonsMusicas.Count; i++)
            {
                _sonsMusicas[i].Stop(); // Chama o método Stop dos objetos AudioSource.
            }
        }
    }

    public void ListSomMults()
    {
        _somMuteMult = !_somMuteMult;
        if (_somMuteMult)
        {
            _textSomMult.text = "Multado Bola"; // Manipula o texto do objeto TextMeshProUGUI.
        }
        else
        {
            _textSomMult.text = "Som Bola"; // Manipula o texto do objeto TextMeshProUGUI.
        }

        for (int i = 0; i < _sonsGames.Count; i++)
        {
            _sonsGames[i].mute = _somMuteMult; // Manipula o estado mute dos objetos AudioSource.
        }
    }

    void Start()
    {
        // Método vazio, sem referência externa.
    }

    // Update is called once per frame
    void Update()
    {
        // Método vazio, sem referência externa.
    }
}
