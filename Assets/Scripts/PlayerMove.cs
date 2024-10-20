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
    [SerializeField] public Transform _p;
    public Transform _posT;
    [SerializeField] Transform _pontos;
    [SerializeField] Transform _o;

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
    public bool _checkPass;
    public bool _isPausadoButton;
    public Transform _temp;
    public GameObject _pont2;
    public float _speedBoostMultiplier = 1.5f;
    public TextMeshProUGUI _lifeText;
    public Transform _inicialRestat;
    public GameObject _panel1;
    public Transform _telaReiniciar;
    public TextMeshProUGUI _cont;
    public float _timeCout;
    public bool _timeOver = false;
    public float _elapsedTime = 0f; 
    public  bool _isCounting = false; 
    public bool _reiniciarJ;
   [SerializeField] private Transform[] _allRestatPoints; // Lista de todos os pontos de reinício
   [SerializeField] private float _coyoteTime = 0.2f;  // Tempo extra para permitir o pulo após deixar o chão
   private float _coyoteTimeCounter;
    [SerializeField] private float _jumpBufferTime = 0.2f;  // Tempo para armazenar o comando de pulo
    private float _jumpBufferCounter;



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
        if (_isCounting && !_timeOver)
        {
            
            _timeCout -= Time.deltaTime;

            // Verifica se o tempo acabou
            if (_timeCout <= 0)
            {
                _timeCout = 0; 
                _timeOver = true; 
                _isCounting = false;
                TempoEsgotado();
            }

            // Atualiza o texto com o novo valor do tempo
            _cont.text = _timeCout.ToString("F0");
        }


        if (_isCounting && !_timeOver)
        {
            _timeCout -= Time.deltaTime;
            if (_timeCout <= 0)
            {
                _timeCout = 0;
                _timeOver = true;
                _isCounting = false;
                TempoEsgotado();
            }
            _cont.text = _timeCout.ToString("F0");
        }

        // Checa se o controle está ativo
        if (_controle._stop == false)
        {
            // Verifica se o player está no chão
            _checkGround = _characterController.isGrounded;
            if (_checkGround)
            {
                _playerVelocity.y = 0;
                _coyoteTimeCounter = _coyoteTime;  
            }
            else
            {
                _coyoteTimeCounter -= Time.deltaTime;  
            }

            // Movimentação e animações
            float tempSpeed = Mathf.Abs(_moveX) + Mathf.Abs(_moveZ);
            _anim.SetFloat("correndo", tempSpeed);
            _anim.SetBool("chekground", _characterController.isGrounded);
            if (!_characterController.isGrounded) Gravity();
            _speedAnimY = _characterController.velocity.y;
            _anim.SetFloat("pulandoY", _speedAnimY);
            _anim.SetBool("IsRunning", _checkwalk);
            _anim.SetBool("parado", true);
            
            if (_checkMove) Andar();

            // Buffer de pulo e "coyote time"
            if (_checkJump)
            {
                _jumpBufferCounter = _jumpBufferTime;  
            }
            else
            {
                _jumpBufferCounter -= Time.deltaTime;  
            }

            if (_autoCorrer)
            {
                CorrerAuto();
                _anim.SetFloat("correndo", 6);
                _anim.SetBool("parado", false);
            }

            // Realiza o pulo se houver "coyote time" ou buffer de pulo disponível
            if (_jumpBufferCounter > 0 && _coyoteTimeCounter > 0 && _checkJump)
            {
                Jump();  
                _jumpBufferCounter = 0; 
                _checkJump = false;  
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
            if (_checkGround || _coyoteTimeCounter > 0)
            {
                _playerVelocity.y = Mathf.Sqrt(_jumpForce * -2f * _gravityValue); // Executa o pulo      
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
           
            _speed = _speed + 0.2f; 

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

        if (other.gameObject.CompareTag("teste") && !_checkPass)
        {
            _checkPass = true;
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
        _checkPass = false;
    }


    public void StopPlayer(bool value)
    {
        _isCounting = false;
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
            _o.DOScale(0, 0f);

     


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
        _timeCout = 60;
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
        _fimM = false;

        _controle._stop = false;
        _anim.SetFloat("correndo", 0);
        _anim.SetBool("parado", false);

        // Ocultar a tela de fim de jogo
        _fim.DOScale(0, 0.5f);
        _telaReiniciar.DOScale(0, 0.5f);
    }
    public void TimeCorrida()
    {
        _cont.text = _timeCout.ToString("F0"); 
        _isCounting = true; 
        _timeOver = false;  
      


    }
    void TempoEsgotado()
    {
        
        // Encontra o ponto de reinício mais próximo
        _posRestatPlayer = GetNearestRestatPoint();
        if (_posRestatPlayer != null)
        {
            // Usa o ponto de reinício armazenado no momento
            transform.position = _posRestatPlayer.position;
        }
        else
        {
            // Caso contrário, usa a posição inicial como fallback
            transform.position = _inicialRestat.position;
        }

        StopPlayer(true);

        // Mesma lógica de reset que já existe no OnTriggerEnter para a tag "filho"
        _pont1.SetActive(false);
        StartCoroutine(Dano());

        _isReseting = true;

        // Reduz a vida do jogador
        _quantVida--;
        if (_quantVida == 0)
        {
            _reiniciarJ = false; // Marca que o jogo deve ser reiniciado
        }

        // Atualiza o HUD de vida
        _playerControle.CheckIcomVida(_quantVida);
        _lifeText.text = _quantVida.ToString();
    }
    // Função para encontrar o ponto de reinício mais próximo
    Transform GetNearestRestatPoint()
    {
        Transform nearestPoint = null;
        float shortestDistance = Mathf.Infinity; 
        Vector3 playerPosition = transform.position; 
        // Itera sobre todos os pontos de reinício disponíveis
        foreach (Transform restatPoint in _allRestatPoints)
        {
            float distance = Vector3.Distance(playerPosition, restatPoint.position); // Calcula a distância até o ponto de reinício

            // Se a distância for menor que a distância mais curta encontrada até agora, atualiza o ponto mais próximo
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestPoint = restatPoint;
            }
        }

        return nearestPoint;
    }



}
