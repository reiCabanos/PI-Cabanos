using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


    public class RememberCurrentlySelectedGameObject : MonoBehaviour
    {
        [SerializeField] private EventSystem eventSystem;
        [SerializeField] private GameObject lastSelectedElement;

        private void Reset()
        {
            eventSystem = FindObjectOfType<EventSystem>();

            if (!eventSystem)
            {
                Debug.Log("Não foi encontrado um Sistema de Eventos nesta cena.", this);
                return;
            }

            lastSelectedElement = eventSystem.firstSelectedGameObject;
        }

        private void Update()
        {
            if (!eventSystem)
                return;

            if (eventSystem.currentSelectedGameObject &&
                lastSelectedElement != eventSystem.currentSelectedGameObject)
                lastSelectedElement = eventSystem.currentSelectedGameObject;

            if (!eventSystem.currentSelectedGameObject && lastSelectedElement)
                eventSystem.SetSelectedGameObject(lastSelectedElement);
        }
    }





public class EventSystemAccess : MonoBehaviour
{
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private Selectable firstItemToSelect;

    private void Start()
    {
        if (eventSystem == null)
            return;

        eventSystem.firstSelectedGameObject = firstItemToSelect.gameObject;
    }
}