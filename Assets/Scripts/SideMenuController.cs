using UnityEngine;
using UnityEngine.InputSystem;  // Para o novo Input System
using DG.Tweening;
using UnityEngine.UI;

public class SideMenuController : MonoBehaviour
{
    public RectTransform PanelMenu;  // O painel lateral que ser� movido
    public float moveDuration = 0.5f;  // Dura��o da anima��o
    public float moveDistance = -295f;  // Dist�ncia que o menu percorre (pode ajustar conforme necess�rio)

    public bool isMenuVisible;  // Controla se o menu est� vis�vel ou n�o

    public Button _btMochila;
    public Button _btCelular;

    public HudControles hudcontrole;

    private void Update()
    {

        //ShowHideMenu();
        


    }
    private void Start()
    {
        // Inicializa o menu fora da tela (oculto)
       // PanelMenu.anchoredPosition = new Vector2(-moveDistance, PanelMenu.anchoredPosition.y);
        isMenuVisible = true;  // Garante que o menu come�a oculto
    }
    public void SetRight(InputAction.CallbackContext context)
    {
        if (context.performed && isMenuVisible)
        {
            // Esconde o menu (move para fora da tela)
            PanelMenu.DOAnchorPosX(-moveDistance, moveDuration);
            isMenuVisible = false;
            _btMochila.Select();
            _btCelular.Select();
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
    public void HideMenu()
    {
        if (isMenuVisible)
        {
            // Esconde o menu (move para fora da tela)
            PanelMenu.DOAnchorPosX(-moveDistance, moveDuration);
            isMenuVisible = false;
        }
    }

    public void ShowMenu()
    {
        if (!isMenuVisible)
        {
            // Mostra o menu (move para dentro da tela)
            PanelMenu.DOAnchorPosX(0, moveDuration);
            isMenuVisible = true;
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
