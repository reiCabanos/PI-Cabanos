using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.EventSystems;

public class SideMenuController : MonoBehaviour
{
    public RectTransform sideMenu;  // Refer�ncia ao menu lateral
    public Button[] menuButtons;    // Array com os bot�es do menu
    public float slideDuration = 0.5f;  // Dura��o da anima��o de deslizamento
    private bool isMenuOpen = false; // Estado atual do menu (aberto ou fechado)
    public float slideDistance = 250f;  // Dist�ncia que o menu desliza

    private void Start()
    {
        // Coloca o menu fora da tela no in�cio
        sideMenu.anchoredPosition = new Vector2(-slideDistance, sideMenu.anchoredPosition.y);
    }

    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;

        // Anima��o para abrir ou fechar o menu
        float targetPositionX = isMenuOpen ? 0 : -slideDistance;
        sideMenu.DOAnchorPosX(targetPositionX, slideDuration).SetEase(Ease.InOutSine);
    }

    public void OnMenuButtonPressed(int buttonIndex)
    {
        // L�gica para quando um bot�o do menu � pressionado
        Debug.Log("Bot�o pressionado: " + menuButtons[buttonIndex].name);

        // Adicione sua l�gica aqui, como mudar de cena ou abrir outra tela
    }
}
