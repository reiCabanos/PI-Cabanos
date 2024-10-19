using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
/*using UnityEditor.ShaderGraph;*/
using UnityEngine;
using UnityEngine.InputSystem;
/*using UnityEngine.Rendering.HighDefinition;*/
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerControle : MonoBehaviour
{
    public Transform[] _iConVida;
    [SerializeField] Transform _telaGameOver;
    public Transform _camera;
    public Canvas _canvas;
    
    public Camera[] _camera2;
    public Button _reiniciar;
    public Transform _player;
    public GameController _gameController;
   
    public PlayerMove _playerMove;
    public ControlePersonagem _controle;

    public string[] _textButons;
    public string[] _textTutors;

    public TextMeshProUGUI _textProTutor;
    public TextMeshProUGUI _textProButon;
    public TextMeshProUGUI _textoContagem;
    public Transform _panelTutor;
    public Button _ButtomNuul;
    public Button _btAvanca;

    public Image[] _imgTutors;
    public Image _imgT;

    public int _conText;
    bool _fimTutor;
    public int _cont;
   
    
    bool _fimGame;
   
    public TextMeshProUGUI _textoPontuaca;
    public TextMeshProUGUI _textoVida;



    void Start()
    {
        TextoTutor(0, 0);
         _controle = Camera.main.GetComponent<ControlePersonagem>();
        _imgT.enabled = false;
        _ButtomNuul.Select();
        //_textoPontuacao = GameObject.Find("TextoPontuacao").GetComponent<Text>();
    }

    public void TextoTutor(int value, int value2)
    {
      //  _textProButon.transform.parent.gameObject.SetActive(false); 
        if (value2 == 0)//texto jogo
        {
            _textProButon.text = _textButons[value];
            _textProTutor.text = _textTutors[value2];
          
        }
        if (value2 == 1)//tutorial movimento
        {
            _imgT.enabled = true;
            _imgT.sprite = _imgTutors[0].sprite;
        }
        if (value2 == 2)//tutorial jump
        {
            _imgT.enabled = true;
            _imgT.sprite = _imgTutors[1].sprite;
        }
        if (value2 == 3)//tutorial tabua
        {
            _imgT.enabled = true;
            _imgT.sprite = _imgTutors[2].sprite;
          
        }

        else
        {
            _textProButon.transform.parent.gameObject.SetActive(true);
         
            _textProButon.text = _textButons[value];
            _textProTutor.text = _textTutors[value2];

        }
      


        StartCoroutine(TempoTutorON());
    }
    

    public void AvancarTutor()
    {
        // Verifica se ainda há tutoriais a serem mostrados
        if (_conText == 0)
        {
            TextoTutor(0, 0); // Primeiro tutorial
            _conText++;
        }
        else if (_conText == 1)
        {
            TextoTutor(0, 1); // Tutorial de movimento
            _conText++;
        }
        else if (_conText == 2)
        {
            TextoTutor(0, 2); // Tutorial de salto
            _conText++;
        }
        else if (_conText == 3)
        {
            TextoTutor(1, 3); // Tutorial da tábua
            _conText++;
        }
        else
        {
            // Todos os tutoriais foram mostrados, então finaliza o tutorial
            _fimTutor = true;
            TempoTutorOff(); // Finaliza a exibição dos tutoriais
            TutorFechar();   // Fecha o painel do tutorial
        }
    }



    IEnumerator TempoTutorON()
    {
        _panelTutor.DOScale(1.5f, .25f);
        yield return new WaitForSeconds(.25f);
        _panelTutor.DOScale(1f, .25f);   
   
    }
   
    public void TempoTutorOff()
    {
        if (!_fimTutor)
        {
            _conText++;
            if (_conText < 4)
            {
                TextoTutor(0, _conText);
                TempoTutorON();
            }
            if (_conText == 4)
            {
                TextoTutor(1, 1);
                StartCoroutine(TempoCont());
               

            }
        }
        else
        {
           
            _playerMove.StopPlayer(false);
            TutorFechar();
            _playerMove._timeCout = 10; // Ajuste conforme a visibilidade da variável
            _playerMove.isCounting = true;
            _playerMove.timeOver = false;

        }
       
     
    }
    public void TentarNovamente()
    {
        _cont++;
       
       if (_cont>=1)
        {
            TextoTutor(2, 1);
            _fimTutor = true;
            _btAvanca.interactable = false;
            StartCoroutine(TempoCont());


        }
    }
    public void Recomeca()
    {
        _conText++;




        if (_conText == 4 && _playerMove._isReseting == true && !_fimGame)
        {
            TextoTutor(2, 4);
            _btAvanca.interactable = true;
            
            TempoTutorON();
           
           

        }
        else if (_conText > 4 && _playerMove._isReseting == true)
        {
            TextoTutor(2, 1);
            _fimTutor = true;
            _btAvanca.interactable = false;
            StartCoroutine(TempoCont());
           
        }

        
        

    }
   



    void TutorFechar()
    {
        _panelTutor.transform.localScale = Vector3.zero;
        _controle._stop = false;
    }

    IEnumerator TempoCont()
    {

        _imgT.enabled = false;
        _fimTutor = true;
        _btAvanca.interactable = false;
        _textoContagem.text = "" + 3;
        yield return new WaitForSeconds(1);
        _textoContagem.text = "" + 2;
        yield return new WaitForSeconds(1);
        _textoContagem.text = "" + 1;
        yield return new WaitForSeconds(1);
        _textoContagem.text = "";

        _textProButon.text = "";
        _textProTutor.text =  "";
        //começar correr
        TutorFechar();
          _playerMove.TimeCorrida();
        
        _playerMove.Corretrue();
        _btAvanca.interactable = true;
      

    }

    public void HudCamera1()
    {
        _canvas.worldCamera = _camera2[0];
    }
    public void HudCamera2()
    {
        _canvas.worldCamera = _camera2[1];
    }
    public void CheckIcomVida(int vida)
    {

        if (vida <= 0)
        {
            _fimGame = true;
            _iConVida[0].DOScale(0, 0.5f);
            _telaGameOver.DOScale(1, 0.5f);
            HudCamera2();
            //_controle._stop = true;
            _reiniciar.Select();
            
        }
        else if (vida == 1)
        {
            _iConVida[1].DOScale(0, 0.5f);

        }

        else if (vida == 2)
        {
            _iConVida[2].DOScale(0, 0.5f);
        }
        else if (vida == 3)
        {
            _iConVida[3].DOScale(0, 0.5f);
        }

    }
    public void AtualizarVidaHUD()
    {
        for (int i = 0; i < _iConVida.Length; i++)
        {
            if (i < _playerMove._quantVida)
            {
                _iConVida[i].DOScale(1, 0.5f);
                _textoVida.text = _playerMove._quantVida.ToString();
            }
            else
            {
                _iConVida[i].DOScale(0, 0.5f);
            }
        }
    }
    public void AtualizarPontuacaoHUD()
    {
        _textoPontuaca.text = _playerMove._scoreCounter.ToString();
    }
    public void GamerReiniciar()
    {
        SceneManager.LoadScene("MapaBeta");
      // _controle._stop=false;
    }
    

}
