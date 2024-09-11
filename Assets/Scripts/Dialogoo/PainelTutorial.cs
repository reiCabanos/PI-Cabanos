using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PainelTutorial : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI _nomeTexto;
    public TextMeshProUGUI _contTexto;
    public Button _btFechar;
    public Transform _painelTutor;
    public List<Dialogo> dialogos = new List<Dialogo>();

    public void PainelOn(bool value, Dialogo dialogo)
    {
        if (value == true)
        {
            _nomeTexto.text = dialogo._nome;
            _contTexto.text = dialogo._texto;
            _btFechar.Select();
            _painelTutor.DOScale(1, .25f);

        }
        else
        {
            _painelTutor.DOScale(1, .25f);

        }

    }
    void Start()
    {

        _painelTutor.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
