using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TutoriasJogo : MonoBehaviour
{
    public string[] _textButons;
    public string[] _textTutors;
    public Image[] _imgTutors;
    public Image _imgT;
    public Transform _panelTutor;
    public int _conText;
    public bool _fimTutor;
    public TextMeshProUGUI _textProTutor;
    public TextMeshProUGUI _textProButon;
    public TextMeshProUGUI _textoContagem;
    public GameController _gameController;
    private PlayerInput _playerInput;

    void Start()
    {
        _gameController = Camera.main.GetComponent<GameController>();
        _imgT.enabled = false;

       
       
    }

    

    public void PrimeiroTutorial(int value, int value2)
    {
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

    void TutorFechar()
    {
        _panelTutor.transform.localScale = Vector3.zero;
        _gameController._gamerOver = false;
    }

    public void AvancarTutor1()
    {
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
            _fimTutor = true;
            TutorFechar();   // Fecha o painel do tutorial
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Tutor1"))
        {
            _conText = 0;
            _fimTutor = false;
            AvancarTutor1();
        }
    }

    public void TempoTutorOff()
    {
        if (!_fimTutor)
        {
            _conText++;
            if (_conText < 4)
            {
                PrimeiroTutorial(0, _conText);
                TempoTutorON();
            }
        }
        else
        {
            _gameController._gamerOver = false;
            TutorFechar();
        }
    }

    private void Update()
    {
        if (Keyboard.current.zKey.wasPressedThisFrame)
        {
            AvancarTutor1();
        }
    }
}
