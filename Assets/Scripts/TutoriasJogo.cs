using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class TutoriasJogo : MonoBehaviour
{
    public string[] _textButons;
    public string[] _textTutors;
    public Image[] _imgTutors;
    //public Image _imgT;
    public Transform _panelTutor;
    public int _conText=-1;
    public int _conText2 = -1;
    public bool _fimTutor;
    public bool _fimTutor2;
    public TextMeshProUGUI _textProTutor;
    public TextMeshProUGUI _textProButon;
    public PlayerMove _playerMove;  // Script de movimento do jogador
    public GameController _gameController;
    public GameObject _textProTutor1;
    public GameObject _textProTutor2;
    public GameObject _imag1;
    public GameObject _imag2;
    public GameObject _imag3;
    public GameObject _tutor1;
    public GameObject _imag4;
    public GameObject _imag5;
    public GameObject _imag6;
    public GameObject _textProTutor3;
    public GameObject _textProTutor4;
    public GameObject _tutor2;





    void Start()
    {
        
        _gameController = Camera.main.GetComponent<GameController>();
        
        
        // Desativa o tutorial no início
        _panelTutor.localScale = Vector3.zero;
    }

    
    public void PrimeiroTutorial(int value, int value2)
    {
        // Exibe o tutorial baseado no valor recebido
        if (value2 == 0) // texto jogo
        {
            _textProButon.text = _textButons[value];
            _textProTutor.text = _textTutors[value2];
        }
        else if (value2 == 1) // tutorial movimento
        {
            /* _imgT.enabled = true;
             _imgT.sprite = _imgTutors[0].sprite;*/
            _imag1.SetActive(true);
            _textProTutor1.SetActive(false);

        }
        else if (value2 == 2) // tutorial jump
        {
            _imag1.SetActive(false);
            _imag2.SetActive(true);
        }
        else if (value2 == 3) // tutorial tabua
        {
            _imag2.SetActive(false);
            _imag3.SetActive(true);
        }
        else if (value2 == 4) // tutorial tabua
        {
           
            _imag3.SetActive(false);
             _textProTutor2.SetActive(true);
}
        // Inicia a animação de abertura do painel do tutorial
        StartCoroutine(TempoTutorON());
    }
    public void SegundoTutorial(int value, int value2)
    {
        // Exibe o tutorial baseado no valor recebido
        if (value2 == 0) // texto jogo
        {
            _textProButon.text = _textButons[value];
            _textProTutor.text = _textTutors[value2];
        }
        else if (value2 == 1) // Como mira
        {
            
            _imag4.SetActive(true);
            _textProTutor3.SetActive(false);

        }
        else if (value2 == 2) // como atira
        {
            _imag4.SetActive(false);
            _imag5.SetActive(true);
        }
        else if (value2 == 3) // como pegar manga
        {
            _imag5.SetActive(false);
            _imag6.SetActive(true);
        }
        else if (value2 == 4) // olhar inventario 
        {

            _imag6.SetActive(false);
            _textProTutor4.SetActive(true);
        }
        // Inicia a animação de abertura do painel do tutorial
        StartCoroutine(TempoTutorON());
    }
    IEnumerator TempoTutorON()
    {
        _panelTutor.DOScale(1.5f, .25f);  // Animação de escala
        yield return new WaitForSeconds(.25f);
        _panelTutor.DOScale(1f, .25f);
    }

    void TutorFechar()
    {
        _panelTutor.transform.localScale = Vector3.zero;
        _gameController._gamerOver = false; // Restaura o movimento do jogador
        _tutor1.SetActive(false);

    }
    void TutorFechar2()
    {
        _panelTutor.transform.localScale = Vector3.zero;
        _gameController._gamerOver = false; // Restaura o movimento do jogador
        _tutor2.SetActive(false);

    }

   
    public void TempoTutorOff()
    {
       
        if (!_fimTutor)
        {
            //_conText++;
            if (_conText == 0)
            {
                PrimeiroTutorial(0, 0); // Primeiro tutorial
                _conText++;
            }
            else if (_conText == 1)
            {
                PrimeiroTutorial(0, 1); // Tutorial de movimento
                _conText++;
            }
            else if (_conText == 2)
            {
                PrimeiroTutorial(0, 2); // Tutorial de salto
                _conText++;
            }
            else if (_conText == 3)
            {
                PrimeiroTutorial(0, 3); // objetivo inicial
                _conText++;
            }
            else if (_conText == 4)
            {
                PrimeiroTutorial(0, 4); // texto Final 
                _conText++;
            }

            else if (_conText == 5)
            {
                _fimTutor = true;

               


            }
        }
        else
        {

            _gameController._gamerOver = false;
            TutorFechar();
            

        }



    }
    public void TempoTutorOff2()
    {

        if (!_fimTutor2)
        {
            //_conText++;
            if (_conText2 == 0)
            {
                SegundoTutorial(0, 0); // Primeiro tutorial
                _conText2++;
            }
            else if (_conText2 == 1)
            {
                SegundoTutorial(0, 1); // Tutorial de movimento
                _conText2++;
            }
            else if (_conText2 == 2)
            {
                SegundoTutorial(0, 2); // Tutorial de salto
                _conText2++;
            }
            else if (_conText2 == 3)
            {
                SegundoTutorial(0, 3); // Tutorial de capturar mangas
                _conText++;
            }
            else if (_conText2 == 4)
            {
                SegundoTutorial(0, 4); // Tutorial de capturar mangas
                _conText2++;
            }

            else if (_conText2 == 5)
            {
                _fimTutor2 = true;




            }
        }
        else
        {

            _gameController._gamerOver = false;
            TutorFechar2();


        }



    }


}


