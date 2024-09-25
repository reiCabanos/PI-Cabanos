using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogoTrigger : MonoBehaviour
{

    // Start is called before the first frame update
    public Dialogo _dialogo;
    public PainelTutorial _painelTutorial;
    private bool isDialogActive;

    void Start()
    {
        _painelTutorial = Camera.main.GetComponent<PainelTutorial>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DialogoTriger"))
        {
            _painelTutorial.PainelOn(true, _dialogo);
            isDialogActive = true;  // Marca o di�logo como ativo
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("DialogoTriger"))
        {
            _painelTutorial.PainelOn(false, null);
        }
    }
    // Update is called once per frame
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
