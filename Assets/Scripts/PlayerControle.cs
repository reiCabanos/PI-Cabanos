using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
/*using UnityEditor.ShaderGraph;*/
using UnityEngine;
using UnityEngine.InputSystem;
/*using UnityEngine.Rendering.HighDefinition;*/
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Progress;


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
    //public MoveNew _moveNew
    public PlayerMove _playerMove;
    public ControlePersonagem _controle;

    public string[] _textButons;
    public string[] _textTutors;

    public TextMeshProUGUI _textProTutor;
    public TextMeshProUGUI _textProButon;
    public TextMeshProUGUI _textoContagem;
    public Transform _panelTutor;
    public Button _ButtomNuul;

    public Image[] _imgTutors;
    public Image _imgT;

    public int _conText;
   

    void Start()
    {
        TextoTutor(0, 0);
         _controle = Camera.main.GetComponent<ControlePersonagem>();
        _imgT.enabled = false;
        _ButtomNuul.Select();
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


    IEnumerator TempoTutorON()
    {
        _panelTutor.DOScale(1.5f, .25f);
        yield return new WaitForSeconds(.25f);
        _panelTutor.DOScale(1f, .25f);   
   
    }

    public void TempoTutorOff()
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
    public void Recomeca()
    {
        _conText++;


        if (_conText ==4 && _playerMove._isReseting == true)
        {
            TextoTutor(0, 4);
            TempoTutorON();
           
        }
       else if (_conText >4  && _playerMove._isReseting == true)
       {
            TextoTutor(2, 1);
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
        _playerMove.Corretrue();


    }

    void Update()
    {

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

    }
    public void GamerReiniciar()
    {
     //   SceneManager.LoadScene("MapaBeta");
     //   _controle._stop=false;
    }
    
}
