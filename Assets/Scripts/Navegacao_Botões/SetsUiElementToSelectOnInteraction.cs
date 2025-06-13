using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


    public class SetsUiElementToSelectOnInteraction : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] private EventSystem eventSystem;
        [SerializeField] private Selectable elementToSelect;

        [Header("Visualization")]
        [SerializeField] private bool showVisualization;
        [SerializeField] private Color navigationColour = Color.cyan;

        private void OnDrawGizmos()
        {
            if (!showVisualization)
                return;

            if (elementToSelect == null)
                return;

            Gizmos.color = navigationColour;
            Gizmos.DrawLine(gameObject.transform.position, elementToSelect.gameObject.transform.position);
        }

        private void Reset()
        {
            eventSystem = FindObjectOfType<EventSystem>();

            if (eventSystem == null)
                Debug.Log("Não foi encontrado um Sistema de Eventos na sua Cena.", this);
        }

        public void JumpToElement()
        {
            if (eventSystem == null)
                Debug.Log("Este item ainda não possui nenhum sistema de eventos referenciado.", this);

            if (elementToSelect == null)
                Debug.Log("Isso deve pular para onde?", this);

            eventSystem.SetSelectedGameObject(elementToSelect.gameObject);
        }
    }
