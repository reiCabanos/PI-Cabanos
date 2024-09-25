using UnityEngine;
using UnityEngine.UI; // Para usar UI elements como Bot�o

public class ActivateObjectToggleCamera : MonoBehaviour
{
    public GameObject objetoParaAtivar; // O objeto que ser� ativado/desativado
    public Camera cameraParaDesativar; // A c�mera que ser� desativada
   // public Button botaoAtivar; // O bot�o que ser� clicado

    private bool isActive = false; // Controle de estado

    void Start()
    {
        // Atribuir a fun��o ao clique do bot�o
       // botaoAtivar.onClick.AddListener(ToggleObjectAndCamera);
    }

    void Update()
    {
        // Verificar se a tecla "E" foi pressionada
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleObjectAndCamera();
        }
    }

    void ToggleObjectAndCamera()
    {
        // Alterna o estado do objeto e da c�mera
        isActive = !isActive;
        objetoParaAtivar.SetActive(isActive); // Ativa ou desativa o objeto
        cameraParaDesativar.gameObject.SetActive(!isActive); // Desativa ou ativa a c�mera
    }
}
