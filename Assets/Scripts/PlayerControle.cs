using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControle : MonoBehaviour
{
    public Transform[] _iConVida;
    [SerializeField] Transform _telaGameOver;
    public Transform _camera;
    public Canvas _canvas;
    public Transform _virtualCam;
    public Transform _localCam;
    public Camera[] _camera2;
    public Button _reiniciar;
    public Transform _player;
    public GameController _gameController;
    void Start()
    {
        _gameController=Camera.main.GetComponent<GameController>();
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
            _gameController._gamerOver = true;
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
        SceneManager.LoadScene("MapaBeta");
    }
    
}
