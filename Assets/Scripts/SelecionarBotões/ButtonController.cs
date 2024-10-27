using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public Button[] buttons; // Array de bot�es
    private int currentIndex = 0; // �ndice do bot�o selecionado

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

        // Selecionar bot�o com "A"
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            buttons[currentIndex].onClick.Invoke(); // Simula o clique no bot�o atual
        }
    }

    private void SelectButton()
    {
        EventSystem.current.SetSelectedGameObject(buttons[currentIndex].gameObject);
    }
}
