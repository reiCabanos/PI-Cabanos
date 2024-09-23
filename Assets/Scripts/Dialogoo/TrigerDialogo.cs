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
        // Quando o jogador entra no trigger, o diálogo é exibido
        if (other.gameObject.CompareTag("DialogoTriger") && _hudControles._telaDiaIni)
        {
            _painelTutorial.PainelOn(true, _dialogo);
            isDialogActive = true;  // Marca o diálogo como ativo
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // O OnTriggerExit não faz nada em relação ao fechamento do diálogo
        // Mantemos o diálogo aberto até que o jogador o feche manualmente
        if (other.gameObject.CompareTag("DialogoTriger"))
        {
            // O diálogo continua ativo mesmo se o jogador sair da área do trigger
            // Não desativa o painel aqui
        }
    }

    void Update()
    {
        // Verifica se o jogador pressiona ESC para fechar o diálogo manualmente
        if (isDialogActive && Input.GetKeyDown(KeyCode.Escape))
        {
            _painelTutorial.PainelOn(false, null);
            isDialogActive = false; // Diálogo foi fechado
        }
    }
}
