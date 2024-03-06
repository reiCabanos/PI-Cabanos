using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayGamer : MonoBehaviour
{
    [SerializeField] Transform _player;
    [SerializeField] Transform _play;
    public PlayerMove _playerMove;
    public ControlePersonagem _controle;
    public Button _botao;
    void Start()
    {
        _controle = Camera.main.GetComponent<ControlePersonagem>();
    }

    // Update is called once per frame
    void Update()
    {

        
    }
    public void GamePlay()
    {
        if (_controle==false)
        {


        }
    }
}
