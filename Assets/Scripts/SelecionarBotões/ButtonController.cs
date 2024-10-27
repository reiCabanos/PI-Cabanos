using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public Button[] buttons; // Array de botões
    private int currentIndex = 0; // Índice do botão selecionado

    private void Update()
    {
        // Mover para a esquerda
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            currentIndex = Mathf.Max(0, currentIndex - 1);
            SelectButton();
        }

        // Mover para a direita
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            currentIndex = Mathf.Min(buttons.Length - 1, currentIndex + 1);
            SelectButton();
        }

        // Selecionar botão com "A"
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            buttons[currentIndex].onClick.Invoke(); // Simula o clique no botão atual
        }
    }

    private void SelectButton()
    {
        EventSystem.current.SetSelectedGameObject(buttons[currentIndex].gameObject);
    }
}
