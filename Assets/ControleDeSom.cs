using System.Collections; // Faz refer�ncia � biblioteca padr�o do C# para cole��es, usada para listas e arrays.
using System.Collections.Generic; // Faz refer�ncia � biblioteca para o uso de cole��es gen�ricas, como List<T>.
using TMPro; // Faz refer�ncia ao TextMeshPro, usado para manipula��o de texto na Unity.
using UnityEngine; // Faz refer�ncia � biblioteca principal da Unity, usada para funcionalidades de jogo.

public class ControleDeSom : MonoBehaviour
{
    // Start is called before the first frame update
    public List<AudioSource> _sonsMenu = new List<AudioSource>(); // Referencia objetos AudioSource na cena da Unity.
    public List<AudioSource> _sonsMusicas = new List<AudioSource>(); // Referencia objetos AudioSource na cena da Unity.
    public List<AudioSource> _sonsGames = new List<AudioSource>(); // Referencia objetos AudioSource na cena da Unity.

    [SerializeField] bool _somMenuMute; // N�o faz refer�ncia externa, usada localmente.
    [SerializeField] bool _somMusicaMute; // N�o faz refer�ncia externa, usada localmente.

    [SerializeField] TextMeshProUGUI _textSomMenu; // Referencia um objeto TextMeshProUGUI na cena da Unity.
    [SerializeField] TextMeshProUGUI _textSomMusica; // Referencia um objeto TextMeshProUGUI na cena da Unity.

    [SerializeField] bool _somMuteMult; // N�o faz refer�ncia externa, usada localmente.
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
                _sonsMusicas[i].Play(); // Chama o m�todo Play dos objetos AudioSource.
            }
        }
        else
        {
            _textSomMusica.text = "Mute"; // Manipula o texto do objeto TextMeshProUGUI.

            for (int i = 0; i < _sonsMusicas.Count; i++)
            {
                _sonsMusicas[i].Stop(); // Chama o m�todo Stop dos objetos AudioSource.
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
        // M�todo vazio, sem refer�ncia externa.
    }

    // Update is called once per frame
    void Update()
    {
        // M�todo vazio, sem refer�ncia externa.
    }
}
