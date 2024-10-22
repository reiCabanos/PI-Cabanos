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
    public Image _imgT;
    public Transform _panelTutor;
    public int _conText=-1;
    public bool _fimTutor;
    public TextMeshProUGUI _textProTutor;
    public TextMeshProUGUI _textProButon;
    public PlayerMove _playerMove;  // Script de movimento do jogador
    public GameController _gameController;
     

    void Start()
    {
        
        _gameController = Camera.main.GetComponent<GameController>();
        _imgT.enabled = false;
        
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
            _imgT.enabled = true;
            _imgT.sprite = _imgTutors[0].sprite;
        }
        else if (value2 == 2) // tutorial jump
        {
            _imgT.enabled = true;
            _imgT.sprite = _imgTutors[1].sprite;
        }
        else if (value2 == 3) // tutorial tabua
        {
            _imgT.enabled = true;
            _imgT.sprite = _imgTutors[2].sprite;
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
       
    }

    public void AvancarTutor1()
    {
        _fimTutor = false;
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
            PrimeiroTutorial(1, 3); // Tutorial de capturar mangas
            _conText++;
        }

        else
        {
            TempoTutorOff();
            _fimTutor = true;
            TutorFechar();   // Fecha o painel do tutorial
        }
        
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
                PrimeiroTutorial(0, 3); // Tutorial de capturar mangas
                _conText++;
            }
            else if (_conText == 4)
            {
                _fimTutor = true;

                PrimeiroTutorial(1, 1);
                //StartCoroutine(TempoCont());


            }
        }
        else
        {

            _gameController._gamerOver = false;
            TutorFechar();
            

        }



    }



}


