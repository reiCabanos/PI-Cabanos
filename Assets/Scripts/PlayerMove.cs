using System.Collections;
using System.Collections.Generic;
/*using UnityEditor.ShaderKeywordFilter;*/
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityStandardAssets.Cameras;
using static UnityEngine.Rendering.DebugUI;
using DG.Tweening;
using UnityEngine.UIElements;
using Unity.Mathematics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using System;
using TMPro;
/*using UnityEditor.ShaderGraph;*/

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float _moveX;
    [SerializeField] float _moveZ;
    [SerializeField] float _speed;
    [SerializeField] float _jumpForce;
    [SerializeField] float _gravityValue = -9.8f;
    [SerializeField] float _girarSpeed;
    [SerializeField] float _rot;
    [SerializeField] float _velocidade;
    [SerializeField] float _timeValue;
    [SerializeField] float _smoothTime=0.0f; 
    float _currentvelocity;
    float _timer;
    float _speedAnimY;
   
    float value;
    float _moveDuration = 1f;
    
    [SerializeField] bool _checkJump;
    [SerializeField] bool _checkGround; 
    [SerializeField] bool _checkwalk;
    [SerializeField] bool _checkMove;
    [SerializeField] bool _autoCorrer; 
    bool _isStandingStill = false; 
    bool _isReseting = false;
  
   
    CharacterController _characterController;
    Animator _anim;

    public Vector3 _playerVelocity;
    Vector3 _moveDir;
    Vector3 _input; 

    [SerializeField] Transform _orientation;
    [SerializeField] Transform _posRestatPlayer;
    [SerializeField] Transform _posRestatPlayer2;
    public Transform _moveCamera;
    public Transform _fim;
    [SerializeField] private Transform[] _scores; 
    [SerializeField] public Transform _coinNextPos;
    [SerializeField] Transform _t;
    [SerializeField] public Transform p;
    public Transform _posT;
    [SerializeField] Transform _pontos;
    [SerializeField] Transform o;

    [SerializeField] int _quantVida = 3;
    [SerializeField] int _mod; 
    private int _index; 
    public int _scoreCounter;

    [SerializeField] PlayerControle _playerControle;
    PlayerPontos _playerPontos;
    [SerializeField] ControlePersonagem _controle;

    [SerializeField] GameObject _pont1;
    
    public Button _fimG;
    public bool _fimM;

    public TextMeshProUGUI _coinCounterTex;
    public bool checkPass;
    public float _speedMultiplier;
    public Transform _tutorialM;
    public Transform _tutorialJ;
    public Transform _tutorialT;
    public bool[] _checkpass;
    // public SkinnedMeshRenderer _skinnedMeshObject;



    void Start()
    {
        _characterController=GetComponent<CharacterController>();

       // SkinnedMeshRenderer skinnedMeshRenderer = _skinnedMeshObject.GetComponent<SkinnedMeshRenderer>();
        _timer = _timeValue;
        _anim = GetComponent<Animator>();
       _playerPontos = Camera.main.GetComponent<PlayerPontos>();
        _controle= Camera.main.GetComponent<ControlePersonagem>();
        _checkMove = true;
        value = -1;


    }
    void Update()
    {
        if (_controle._stop == false)
        {

            _checkGround = _characterController.isGrounded;
            if (_checkGround)
            {
                _playerVelocity.y = 0;

            }

            float tempSpeed = Mathf.Abs(_moveX) + Mathf.Abs(_moveZ);
            _anim.SetFloat("correndo", tempSpeed);
            _anim.SetBool("chekground", _characterController.isGrounded);

            if (_characterController.isGrounded == false)
            {
                Gravity();
            }
            _speedAnimY = _characterController.velocity.y;
            _anim.SetFloat("pulandoY", _speedAnimY);
            _anim.SetBool("IsRunning", _checkwalk);
            _anim.SetBool("parado", true);



            if (_checkMove)
            {
                Andar();
            }




            Jump();

            if (_checkJump)
            {
                _timer -= Time.deltaTime;
                if (_timer < 0)
                {
                    _checkJump = false;
                    _timer = _timeValue;

                }

            }
            if (_autoCorrer)
            {

                CorrerAuto();

                _anim.SetFloat("correndo", 6);
                _anim.SetBool("parado", false);





            }
            Gravity();


        }

        
    }
    void Andar()
    {
        _moveDir = (_orientation.forward * _moveZ + _orientation.right * _moveX) * _speed;

        // Movimento do personagem
        Vector3 move = new Vector3(_moveDir.x, _playerVelocity.y, _moveDir.z);
        _characterController.Move(move * Time.deltaTime);

        // Manter o personagem reto
        Vector3 currentEulerAngles = transform.eulerAngles;
        transform.eulerAngles = new Vector3(0, currentEulerAngles.y, 0);

        if (_checkwalk && _velocidade != 0)
        {
            _speed = 8f;
        }
        else
        {
            _speed = 4f;
        }
       

    }
    void RoationPlayer()
    {
        if (_input.sqrMagnitude==0)return;
            
       var tartAngle = Mathf.Atan2( _moveDir.x, _moveDir.z)*Mathf.Rad2Deg;
       var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, tartAngle, ref _currentvelocity, _smoothTime);
       transform.rotation=Quaternion.Euler(0,angle,0);
        

    }
    public void SetMove(InputAction.CallbackContext value)
    {
        if (_checkMove)
        {
            _controle._stop = false;
            Vector3 m = value.ReadValue<Vector3>();
            _moveX = m.x;
            _moveZ = m.y;
        }


    }


    public void SetJump(InputAction.CallbackContext value)
    {
        _controle._stop = false;
        _checkJump = true;
        
    }
    public void SetMoveWalk(InputAction.CallbackContext value)
    {
        _checkwalk = value.performed;
        
    }
    
    void Jump()
    {
        if (_checkGround==true && _checkJump)
        {
            _checkGround = false;
            _playerVelocity.y = 0;
            _playerVelocity.y = Mathf.Sqrt(_jumpForce /5 * -3.0f *_gravityValue);
        }
    }
    void Gravity()
    {
      
        _playerVelocity.y += _gravityValue * Time.deltaTime;
        _characterController.Move( _playerVelocity *Time.deltaTime);
      
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PauseTag") && !_checkpass[0])
        {
            _checkpass[0] = true;
            _controle._stop = true;
            _tutorialM.DOScale(1, 1f);

        }
        if (other.gameObject.CompareTag("FechaPauseTag") && !_checkpass[1])
        {
             _checkpass[1]=true;
            _tutorialM.DOScale(0, 0f);
            

        }
        if (other.gameObject.CompareTag("PauseTagJ") && !_checkpass[2])
        {
            _controle._stop = true;

            _checkpass[2] = true;
            _tutorialJ.DOScale(1, 1f);

        }
        if (other.gameObject.CompareTag("FechaPauseTagJ") && !_checkpass[3])
        {

            _checkpass[3] = true;
            _tutorialJ.DOScale(0, 0f);


        }

        if (other.gameObject.CompareTag("PauseTagT") && !_checkpass[4])
        {
            _controle._stop = true;

            _checkpass[4] = true;
            _tutorialT.DOScale(1, 1f);

        }
        if (other.gameObject.CompareTag("FechaPauseTagT") && !_checkpass[5])
        {
            _checkpass[5] = true;
            _tutorialT.DOScale(0, 0f);


        }


        if (other.gameObject.CompareTag("filho"))
        {
            StartCoroutine(TempoPlayer());
           _posRestatPlayer = other.GetComponent<Resetar>()._posRestat;
            _pont1.SetActive(false);
            StartCoroutine(Dano());

            _isReseting = true;

            _quantVida--;
            _playerControle.CheckIcomVida(_quantVida);
        }
        if (other.gameObject.CompareTag("p2"))
        {

             RotacaoDaCamera();
            _pont1.SetActive(true);
            value *= -1;
            //StartCoroutine(TempoPlayer());
           _posRestatPlayer2 = other.GetComponent<Resetar>()._posRestat;

        }

        if (other.gameObject.CompareTag("i"))
        {
            _moveCamera.localEulerAngles = new Vector3(_moveCamera.localEulerAngles.x, -144.043f, _moveCamera.localEulerAngles.z);
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, -144.043f, transform.localEulerAngles.z);


        }
        if (other.gameObject.CompareTag("item"))
        {
            _playerPontos.SomarPontos(1);

            other.GetComponent<ColetarItens>().DestroyItens();




        }
        


        if (other.gameObject.CompareTag("fimGamer"))
        {

            _controle._stop=true;
            _anim.SetFloat("correndo", 0);
            _anim.SetBool("parado", true);
            //transform.position = new Vector3(436.14f, 9.340258f,990.23f );
            transform.DORotate (new Vector3(transform.localEulerAngles.x, -117.454f, transform.localEulerAngles.z),1f, RotateMode.Fast).SetEase(Ease.InQuad); 
            _fim.DOScale(1, 0.5f);
            _fimG.Select();


        }

        if (other.gameObject.CompareTag("teste") && !checkPass)
        {
            checkPass = true;
            _scores[_index].gameObject.SetActive(true);
            other.transform.parent = _coinNextPos.parent;
            _t = other.transform;
            _scores[_index].position = other.transform.position;
            if (_index < 4)
                _index++;

            else
            {
                _index = 0;
            }
            


            StartCoroutine(Desativar());
           

        }
        if (other.gameObject.CompareTag("Speed"))
        {
            
            _speed =_speed+_speedMultiplier;
            Debug.Log("Aumentar velocidade");

        }


    }
    public void RotacaoDaCamera()
    {
         StartCoroutine(TempoRotacao());
        _moveCamera.DORotate(new Vector3(_moveCamera.localEulerAngles.x, -270, _moveCamera.localEulerAngles.z), 1f, RotateMode.Fast).SetEase(Ease.InQuad);
        transform.DORotate (new Vector3(transform.localEulerAngles.x, 117.454f, transform.localEulerAngles.z),1f, RotateMode.Fast).SetEase(Ease.InSine); 
        _fim.DORotate(new Vector3(_fim.localEulerAngles.x, -270, _fim.localEulerAngles.z), 1f, RotateMode.Fast).SetEase(Ease.InQuad);


    }
   

    public void CorrerAuto()
    {
        _moveDir = transform.forward * 1;
        _speed = 4f;

        _characterController.Move(_moveDir * Time.deltaTime * _speed);
        _checkwalk = true;

       
        
    }
    
    IEnumerator Dano()
    {
        _checkMove = false;
        yield return new WaitForSeconds(0.5f);
        transform.position = _posRestatPlayer.position;
        yield return new WaitForSeconds(0.5f);
        _checkMove =true;
       
    }
    IEnumerator Desativar()
    {
        _t.transform.SetParent(_posT.transform);
        _t.gameObject.SetActive(true);
        _t.DORotate(new Vector3(0, 360*3, 0), 1, RotateMode.WorldAxisAdd);
        _t.DOLocalMove(new Vector3(0, 0, 0), _moveDuration);
        _t.DOScale(new Vector3(0.5f, 0.1f, 1f), 2).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(1f);
        _scoreCounter++;
        _coinCounterTex.text = _scoreCounter.ToString();
        _t.gameObject.SetActive(false);
        checkPass = false;
    }


    IEnumerator TempoPlayer()
    {
        _controle._stop = true;
        _anim.SetFloat("correndo", 0);
        _anim.SetBool("parado", true);
        p.DOScale(0, 0f);
        //_skinnedMeshObject.enabled = false;
        yield return new WaitForSeconds(0.5f);
        //_skinnedMeshObject.enabled = true;
        p.DOScale(0.5164886f, 0.5164886f);
        _controle._stop = false;

    }
    IEnumerator TempoRotacao()
    {
        _controle._stop = true;
        _anim.SetFloat("correndo", 0);
        _anim.SetBool("parado", true);
        yield return new WaitForSeconds(0.2f);
        _controle._stop = false;

    }


    public void Corretrue()
    {
       
        _autoCorrer = true;
        
    }
    public void SetCorrer(InputAction.CallbackContext Value)
    {

        _autoCorrer = true;
            o.DOScale(0, 0f);
    
       


    }
    public void SetFimg(InputAction.CallbackContext value)
    {
        _fimM = true;
        SceneManager.LoadScene("MapaBeta");
       
    }
    public void FimGamer()
    {
        SceneManager.LoadScene("MapaBeta");
    }

}
