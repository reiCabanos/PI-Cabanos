using UnityEngine;
using UnityEngine.InputSystem;  // Para o novo Input System
using DG.Tweening;

public class SideMenuController : MonoBehaviour
{
    public RectTransform PanelMenu;  // O painel lateral que será movido
    public float moveDuration = 0.5f;  // Duração da animação
    public float moveDistance = -295f;  // Distância que o menu percorre (pode ajustar conforme necessário)

    private bool isMenuVisible = false;  // Controla se o menu está visível ou não

    private void Update()
    {

        //ShowHideMenu();
        


    }
    private void Start()
    {
        // Inicializa o menu fora da tela (oculto)
        PanelMenu.anchoredPosition = new Vector2(-moveDistance, PanelMenu.anchoredPosition.y);
        isMenuVisible = false;  // Garante que o menu começa oculto
    }
    public void SetRight(InputAction.CallbackContext context)
    {
        if (context.performed && isMenuVisible)
        {
            // Esconde o menu (move para fora da tela)
            PanelMenu.DOAnchorPosX(-moveDistance, moveDuration);
            isMenuVisible = false;
        }
    }
    public void SetLeft(InputAction.CallbackContext context)
    {
        if (context.performed && !isMenuVisible)
        {
            // Mostra o menu (move para dentro da tela)
            PanelMenu.DOAnchorPosX(-295f, moveDuration);
            isMenuVisible = true;
            Debug.Log("fff");
        }
        
    }

    public void ShowHideMenu()
    {
        if (isMenuVisible && Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Esconde o menu (move para fora da tela)
            PanelMenu.DOAnchorPosX(-moveDistance, moveDuration);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // Mostra o menu (move para dentro da tela)
            PanelMenu.DOAnchorPosX(-295f, moveDuration);
        }

        // Alterna o estado do menu
        isMenuVisible = !isMenuVisible;
    }
}
