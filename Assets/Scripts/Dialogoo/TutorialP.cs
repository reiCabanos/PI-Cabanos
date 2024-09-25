using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialP : MonoBehaviour
{
    public TextMeshProUGUI _nomeTexto;
    
    public Transform _painelTutor;
    public List<Dialogo> dialogos = new List<Dialogo>();
    public DialogoScript dialogoScript; // Refer�ncia ao DialogoScript
    public HudControles hudControles;

    public void PainelOn(bool value, Dialogo dialogo)
    {
        if (value == true)
        {
            _nomeTexto.text = dialogo._nome;
            
            _painelTutor.DOScale(1, .25f);
            dialogoScript.IniciarDialogo(dialogo._texto); // Inicia o di�logo a partir do DialogoScript
            dialogoScript.AnimarSprite(); // Inicia a anima��o do sprite
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
