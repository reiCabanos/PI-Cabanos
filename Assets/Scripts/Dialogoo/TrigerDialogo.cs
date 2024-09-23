using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrigerDialogo : MonoBehaviour
{
    public Dialogo _dialogo;
    public PainelTutorial _painelTutorial;
    private bool isDialogActive;
    public HudControles _hudControles;

    void Start()
    {
        _painelTutorial = Camera.main.GetComponent<PainelTutorial>();
        _hudControles = Camera.main.GetComponent<PainelTutorial>().hudControles;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Quando o jogador entra no trigger, o di�logo � exibido
        if (other.gameObject.CompareTag("DialogoTriger") && _hudControles._telaDiaIni)
        {
            _painelTutorial.PainelOn(true, _dialogo);
            isDialogActive = true;  // Marca o di�logo como ativo
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // O OnTriggerExit n�o faz nada em rela��o ao fechamento do di�logo
        // Mantemos o di�logo aberto at� que o jogador o feche manualmente
        if (other.gameObject.CompareTag("DialogoTriger"))
        {
            // O di�logo continua ativo mesmo se o jogador sair da �rea do trigger
            // N�o desativa o painel aqui
        }
    }

    void Update()
    {
        // Verifica se o jogador pressiona ESC para fechar o di�logo manualmente
        if (isDialogActive && Input.GetKeyDown(KeyCode.Escape))
        {
            _painelTutorial.PainelOn(false, null);
            isDialogActive = false; // Di�logo foi fechado
        }
    }
}
