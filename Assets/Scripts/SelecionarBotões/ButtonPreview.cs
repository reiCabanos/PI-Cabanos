using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonPreview : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public GameObject previewPanel; // O painel de pré-visualização

    // Método chamado quando o botão é selecionado
    public void OnSelect(BaseEventData eventData)
    {
        previewPanel.SetActive(true); // Mostrar a pré-visualização
    }

    // Método chamado quando o botão é desmarcado/deselecionado
    public void OnDeselect(BaseEventData eventData)
    {
        previewPanel.SetActive(false); // Esconder a pré-visualização
    }
}
