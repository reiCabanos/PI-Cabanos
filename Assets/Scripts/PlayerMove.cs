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
using Image = UnityEngine.UI.Image;
using System.Threading;

/*using UnityEditor.ShaderGraph;*/

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float _moveX;
    [SerializeField] float _moveZ;
    public float _speed;
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
   
    public float value;
    float _moveDuration = 1f;
    
    [SerializeField] bool _checkJump;
    [SerializeField] bool _checkGround; 
    [SerializeField] bool _checkwalk;
    [SerializeField] bool _checkMove;
    public bool _autoCorrer; 
    bool _isStandingStill = false; 
    public  bool _isReseting = false;
   
   


    CharacterController _characterController;
   public Animator _anim;

    public Vector3 _playerVelocity;
    Vector3 _moveDir;
    Vector3 _input; 

    [SerializeField] Transform _orientation;
    [SerializeField] Transform _posRestatPlayer;
    public Transform _moveCamera;
    public Transform _fim;
    [SerializeField] private Transform[] _scores; 
    [SerializeField] public Transform _coinNextPos;
    [SerializeField] Transform _t;
    [SerializeField] public Transform p;
    public Transform _posT;
    [SerializeField] Transform _pontos;
    [SerializeField] Transform o;

    public  int _quantVida = 3;
    [SerializeField] int _mod; 
    private int _index; 
    public int _scoreCounter;

    [SerializeField] PlayerControle _playerControle;
    PlayerPontos _playerPontos;
    [SerializeField] ControlePersonagem _controle;

    [SerializeField] GameObject _pont1;
    
    public Button _fimG;
    public Button _comecarNovamente;
    public bool _fimM;

    public TextMeshProUGUI _coinCounterTex;
    public bool checkPass;
    public bool isPausadoButton;
    public Transform _temp;
    public GameObject _pont2;
    public float speedBoostMultiplier = 1.5f;
    public TextMeshProUGUI _lifeText;
    public Transform _inicialRestat;
    public GameObject _panel1;
    public Transform _telaReiniciar;
    public TextMeshProUGUI _cont;
    public float _timeCout;
    public bool timeOver = false;
    public float elapsedTime = 0f; 
    public  bool isCounting = false; 
    
    public bool _reiniciarJ;










    void Start()
    {
        
        _characterController =GetComponent<CharacterController>();

       
        _timer = _timeValue;
        _anim = GetComponent<Animator>();
       _playerPontos = Camera.main.GetComponent<PlayerPontos>();
        _controle= Camera.main.GetComponent<ControlePersonagem>();
        _checkMove = true;
        value = -1;


    }
    void Update()
    {
        // Apenas atualiza o tempo se a contagem estiver ativa
        if (isCounting && !timeOver)
        {
            // Decrementa o contador com base no deltaTime
            _timeCout -= Time.deltaTime;

            // Verifica se o tempo acabou
            if (_timeCout <= 0)
            {
                _timeCout = 0; // Evita que o tempo fique negativo
                timeOver = true; // Marca que o tempo acabou
                isCounting = false; // Para a contagem
            }

            // Atualiza o texto com o novo valor do tempo
            _cont.text = _timeCout.ToString("F0");
        }


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
        Vector3 move = (_orientation.forward * _moveZ + _orientation.right * _moveX) * _speed;
        move.y = _playerVelocity.y;
        _characterController.Move(move * Time.deltaTime);

        // Mantém o player alinhado com o eixo Y
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);


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
          
            Vector3 m = value.ReadValue<Vector3>();
            _moveX = m.x;
            _moveZ = m.y;
        }

        Debug.Log("jjkçll");
    }


    public void SetJump(InputAction.CallbackContext value)
    {
      
        _checkJump = true;

        Debug.Log("jummp");
    }
    public void SetMoveWalk(InputAction.CallbackContext value)
    {
        _checkwalk = value.performed;
        
    }
    public void SetAvanca(InputAction.CallbackContext value)
    {

        _playerControle.TempoTutorOff();

        Debug.Log("Avançass");
    }

    void Jump()
{
    if (_checkGround && _checkJump)
    {
      /*      // Impedir múltiplos saltos até que o jogador toque o chão novamente
            _checkGround = true;
            _checkJump = true;*/

            // Reseta a velocidade Y antes de aplicar o impulso do salto para evitar efeitos acumulativos de gravidade
            _playerVelocity.y = 0;

        // Aplica o impulso do salto de forma mais suave, ajustando a força de salto
        _playerVelocity.y = Mathf.Sqrt(_jumpForce * -2f * _gravityValue);  // -2f dá um salto mais responsivo

        // **Opcional**: Adicione um efeito de impulso visual ou som para dar feedback ao jogador
        // AudioManager.Play("JumpSound");  // Exemplo de som de pulo
    }
}

    void Gravity()
    {

        if (!_checkGround)
        {
            _playerVelocity.y += _gravityValue * Time.deltaTime;
        }

        _characterController.Move(_playerVelocity * Time.deltaTime);


    }
    
        
       
    

    private void OnTriggerEnter(Collider other)
    {
        

        if (other.gameObject.CompareTag("filho"))
        {
            StopPlayer(true) ;
           // StartCoroutine(TempoPlayer());
            _posRestatPlayer = other.GetComponent<Resetar>()._posRestat;
            _pont1.SetActive(false);
            StartCoroutine(Dano());
           
            _isReseting = true;

            _quantVida--;
            if (_quantVida == 0)
            {
                _reiniciarJ = false;  
                
            }


            _playerControle.CheckIcomVida(_quantVida);
            _lifeText.text = _quantVida.ToString();
          

            
        }
        if (other.gameObject.CompareTag("p2"))
        {

             RotacaoDaCamera();
            _pont1.SetActive(false);
            value *= -1;
            
         

        }
        if (other.gameObject.CompareTag("point3"))
        {

            SegundaRotacao();
            _pont2.SetActive(false);
            value *= -1;



        }
        if (other.gameObject.CompareTag("point4"))
        {

            UltimaRotacao();
            value *= -1;



        }
        if (other.gameObject.CompareTag("point5"))
        {

            transform.DORotate(new Vector3(transform.localEulerAngles.x, -439.518f, transform.localEulerAngles.z), 1f, RotateMode.Fast).SetEase(Ease.InSine);


        }
        if (other.gameObject.CompareTag("point6"))
        {

            transform.DORotate(new Vector3(transform.localEulerAngles.x, -86.897f, transform.localEulerAngles.z), 1f, RotateMode.Fast).SetEase(Ease.InSine);


        }
        if (other.gameObject.CompareTag("point7"))
        {

            transform.DORotate(new Vector3(transform.localEulerAngles.x, 271.7f, transform.localEulerAngles.z), 1f, RotateMode.Fast).SetEase(Ease.InSine);


        }

        if (other.gameObject.CompareTag("i") && _isReseting == true )
        {

            _playerControle._conText = 3;


            _playerControle.Recomeca();


        }
        if (other.gameObject.CompareTag("item") && _autoCorrer==true )
        {
            // _playerPontos.SomarPontos(1);

            // other.GetComponent<ColetarItens>().DestroyItens();

            _speed = _speed + 0.5f;
            Debug.Log("dddssddda");

        }
        
        

        if (other.gameObject.CompareTag("fimGamer"))
        {

            _controle._stop=true;
            _anim.SetFloat("correndo", 0);
            _anim.SetBool("parado", true);
            
         
            _fim.DOScale(1, 0.5f);
            _comecarNovamente.Select();
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
       


    }
   private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("i"))
        {

            _isReseting = false;

        


        }
    }

    public void RotacaoDaCamera()
    {
         StartCoroutine(TempoRotacao());
        _moveCamera.DORotate(new Vector3(_moveCamera.localEulerAngles.x, 14.791f, _moveCamera.localEulerAngles.z), 1f, RotateMode.Fast).SetEase(Ease.InQuad);
        transform.DORotate (new Vector3(transform.localEulerAngles.x, -333.951f, transform.localEulerAngles.z),1f, RotateMode.Fast).SetEase(Ease.InSine); 
        //_fim.DORotate(new Vector3(_fim.localEulerAngles.x, -270, _fim.localEulerAngles.z), 1f, RotateMode.Fast).SetEase(Ease.InQuad);


    }
    public void SegundaRotacao()
    {
        StartCoroutine(TempoRotacao());
        _moveCamera.DORotate(new Vector3(_moveCamera.localEulerAngles.x, -89.966f, _moveCamera.localEulerAngles.z), 1f, RotateMode.Fast).SetEase(Ease.InQuad);
        transform.DORotate(new Vector3(transform.localEulerAngles.x, -94.987f, transform.localEulerAngles.z), 1f, RotateMode.Fast).SetEase(Ease.InSine);
        


    }
    public void UltimaRotacao()
    {
        StartCoroutine(TempoRotacao());
        _moveCamera.DORotate(new Vector3(_moveCamera.localEulerAngles.x, 185.974f, _moveCamera.localEulerAngles.z), 1f, RotateMode.Fast).SetEase(Ease.InQuad);
        transform.DORotate(new Vector3(transform.localEulerAngles.x, 174.85f, transform.localEulerAngles.z), 1f, RotateMode.Fast).SetEase(Ease.InSine);



    }


    public void CorrerAuto()
    {
        _characterController.Move(transform.forward * _speed * Time.deltaTime);
        _checkwalk = true;




    }
    
    public IEnumerator Dano()
    {

     
        
        _checkMove = false;
        yield return new WaitForSeconds(0.5f);
        transform.position = _posRestatPlayer.position;
        yield return new WaitForSeconds(0.5f);
        _checkMove = true;
       

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


    public void StopPlayer(bool value)
    {
        isCounting = false;
        if (value)
        {
            _anim.SetFloat("correndo", 0);
        }
        _controle._stop = value;     
        _anim.SetBool("parado", value);
       

       

    }
    IEnumerator TempoRotacao()
    {
        _controle._stop = true;
        _anim.SetFloat("correndo", 0); 
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
    public void ReiniciarJogo()
    {
        _timeCout = 10;
        _cont.text = ("0");

        _reiniciarJ = true;

        
        _playerControle._fimTutor = false;

        _playerControle._conText=0;
        _playerControle.AvancarTutor();

        // Restaurar a posição inicial do personagem
        transform.position = _inicialRestat.position;
        _pont1.SetActive(false);
        _pont2.SetActive(false);
        // Restaurar a rotação inicial do personagem
        transform.DORotate(new Vector3(transform.localEulerAngles.x, -86.955f, transform.localEulerAngles.z), 1f, RotateMode.Fast).SetEase(Ease.InSine);
        transform.rotation = Quaternion.identity;
        _moveCamera.DORotate(new Vector3(_moveCamera.localEulerAngles.x, -83.92f, _moveCamera.localEulerAngles.z), 1f, RotateMode.Fast).SetEase(Ease.InQuad);
        // Restaurar a velocidade do personagem
        _speed = 2;

        // Restaurar a vida do personagem
        _quantVida = 3;
        _playerControle.AtualizarVidaHUD();

        // Restaurar o estado de movimento do personagem
        _checkMove = true;

        // Restaurar o estado de corrida automática
        _autoCorrer = false;

        // Restaurar a pontuação do jogo
        _scoreCounter = 0;
        _playerControle.AtualizarPontuacaoHUD();

        // Desativar todos os itens coletados
        foreach (Transform item in _pontos)
        {
            item.gameObject.SetActive(true);
        }

        // Redefinir o índice do item coletado
        _index = 0;

        // Redefinir a variável de controle da fase
        //_isReseting = false;

        // Redefinir a variável de fim de jogo
        // Redefinir a variável de fim de jogo
        _fimM = false;

        // Reativar o controle do personagem
        _controle._stop = false;

        // Restaurar a animação do personagem
        _anim.SetFloat("correndo", 0);
        _anim.SetBool("parado", false);

        // Ocultar a tela de fim de jogo
        _fim.DOScale(0, 0.5f);
        _telaReiniciar.DOScale(0, 0.5f);

        // Selecionar o botão de reinício
       // _playerControle._reiniciar.Select();
    }
    public void TimeCorrida()
    {
        _cont.text = _timeCout.ToString("F0"); // Atualiza o texto com o valor inicial
        isCounting = true; // Inicia a contagem
        timeOver = false;  // Reseta a flag de fim de tempo
      


    }


}
