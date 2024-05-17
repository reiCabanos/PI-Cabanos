using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Estamina : MonoBehaviour
{
    [SerializeField] Slider _slider;
    public float _timetotal = 15;
    public float _oldTimer;
    public bool _estaminaOn;
    public int _estaminaStatus;



    // Start is called before the first frame update
    void Start()
    {

        _oldTimer = _timetotal;
        _slider.maxValue = _timetotal;
        _slider.value = _timetotal;

    }

    // Update is called once per frame
    void Update()
    {
        if (_estaminaStatus == 1) /// Ativar contagem regressiva
        {
            _oldTimer -= Time.deltaTime * 3;
            _slider.value = _oldTimer;

            if (_oldTimer < 0) // Estamina vazia 
            {

                _estaminaStatus = 2;

            }

        }
        else if (_estaminaStatus == 2)
        {
            _oldTimer += Time.deltaTime;
            _slider.value = _oldTimer;

            if (_oldTimer > _timetotal) // Estamina vazia 
            {

                _estaminaStatus = 0;

            }


        }



    }
}

