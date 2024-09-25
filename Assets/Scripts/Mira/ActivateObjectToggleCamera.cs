using UnityEngine;
using UnityEngine.UI; // Para usar UI elements como Botão

public class ActivateObjectToggleCamera : MonoBehaviour
{
    public GameObject objetoParaAtivar; // O objeto que será ativado/desativado
    public Camera cameraParaDesativar; // A câmera que será desativada
   // public Button botaoAtivar; // O botão que será clicado

    private bool isActive = false; // Controle de estado

    void Start()
    {
        // Atribuir a função ao clique do botão
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
        // Alterna o estado do objeto e da câmera
        isActive = !isActive;
        objetoParaAtivar.SetActive(isActive); // Ativa ou desativa o objeto
        cameraParaDesativar.gameObject.SetActive(!isActive); // Desativa ou ativa a câmera
    }
}
