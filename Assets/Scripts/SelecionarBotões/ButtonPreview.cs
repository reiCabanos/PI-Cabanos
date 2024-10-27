using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonPreview : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public GameObject previewPanel; // O painel de pr�-visualiza��o

    // M�todo chamado quando o bot�o � selecionado
    public void OnSelect(BaseEventData eventData)
    {
        previewPanel.SetActive(true); // Mostrar a pr�-visualiza��o
    }

    // M�todo chamado quando o bot�o � desmarcado/deselecionado
    public void OnDeselect(BaseEventData eventData)
    {
        previewPanel.SetActive(false); // Esconder a pr�-visualiza��o
    }
}
