using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PainelTutorial : MonoBehaviour
{
    public TextMeshProUGUI _nomeTexto;
    public Button _btFechar;
    public Transform _painelTutor;
    public List<Dialogo> dialogos = new List<Dialogo>();
    public DialogoScript dialogoScript; // Referência ao DialogoScript
    public HudControles hudControles;

    public void PainelOn(bool value, Dialogo dialogo)
    {
        if (value == true)
        {
            _nomeTexto.text = dialogo._nome;
            _btFechar.Select();
            _painelTutor.DOScale(1, .25f);
            dialogoScript.IniciarDialogo(dialogo._texto); // Inicia o diálogo a partir do DialogoScript
            dialogoScript.AnimarSprite(); // Inicia a animação do sprite
        }
        else
        {
            _painelTutor.DOScale(0, .25f);
        }
    }

    void Start()
    {
        _painelTutor.localScale = Vector3.zero;
    }
}
