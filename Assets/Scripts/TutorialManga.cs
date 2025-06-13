using DG.Tweening;
using SmallHedge.SomDialogo;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManga : MonoBehaviour
{
    public string[] _textButons;
    public string[] _textTutors;
    public Image[] _imgTutors;
    public Transform _panelTutor;
    public int _conText = -1;
    public bool _fimTutor;
    public TextMeshProUGUI _textProTutor;
    public TextMeshProUGUI _textProButon;
    public GameController _gameController;
    public GameObject _textProTutor1;
   
    public GameObject _imag1;
   
   
    public GameObject _tutorManga;
    public MoveNew _moveNew;
    public GameObject _panelTutorPrefab;
    public GameObject _slider; // Slider de Menu

    void Start()
    {
        _gameController = Camera.main.GetComponent<GameController>();

        // Desativa o tutorial no início
        _panelTutor.localScale = Vector3.zero;
    }


    public void PrimeiroTutorial(int value, int value2)
    {
        _slider.SetActive(false); // Desativa o slider de Menu
        // Exibe o tutorial baseado no valor recebido
        if (value2 == 0) // texto jogo
        {
           
            _textProButon.text = _textButons[value];
            _textProTutor.text = _textTutors[value2];

        }
        else if (value2 == 1) // tutorial movimento
        {
            
            _imag1.SetActive(true);
            _textProTutor1.SetActive(false);
           

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
        _tutorManga.SetActive(false);
        _slider.SetActive(true); // ativar o slider de Menu

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
               
                _fimTutor = true;
                _panelTutorPrefab.SetActive(false);

                _gameController._gamerOver = false;
                TutorFechar();
                _tutorManga.SetActive(false);
                _moveNew.podeAvancarManga = false;




            }
        }
        else
        {
            _moveNew.podeAvancarManga = false;
            _gameController._gamerOver = false;
            TutorFechar();


        }



    }
}
