using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.EventSystems;

public class SideMenuController : MonoBehaviour
{
    public RectTransform sideMenu;  // Referência ao menu lateral
    public Button[] menuButtons;    // Array com os botões do menu
    public float slideDuration = 0.5f;  // Duração da animação de deslizamento
    private bool isMenuOpen = false; // Estado atual do menu (aberto ou fechado)
    public float slideDistance = 250f;  // Distância que o menu desliza

    private void Start()
    {
        // Coloca o menu fora da tela no início
        sideMenu.anchoredPosition = new Vector2(-slideDistance, sideMenu.anchoredPosition.y);
    }

    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;

        // Animação para abrir ou fechar o menu
        float targetPositionX = isMenuOpen ? 0 : -slideDistance;
        sideMenu.DOAnchorPosX(targetPositionX, slideDuration).SetEase(Ease.InOutSine);
    }

    public void OnMenuButtonPressed(int buttonIndex)
    {
        // Lógica para quando um botão do menu é pressionado
        Debug.Log("Botão pressionado: " + menuButtons[buttonIndex].name);

        // Adicione sua lógica aqui, como mudar de cena ou abrir outra tela
    }
}
