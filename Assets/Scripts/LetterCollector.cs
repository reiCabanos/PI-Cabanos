using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LetterCollector : MonoBehaviour
{
    public string completeText = "";
    public int currentCharIndex = 0;

    void Start()
    {
        GetComponent<TextMeshProUGUI>().text = "A";
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("txt"))
        {
            // Obter o texto do GameObject colidido
            TextMeshProUGUI triggerText = other.gameObject.GetComponent<TextMeshProUGUI>();
            string character = triggerText.text;

            // Adicionar o caractere à string completa
            completeText += character;

            // Atualizar o texto principal
            GetComponent<Text>().text = completeText;

            // Incrementar o índice do caractere
            currentCharIndex++;
            Debug.Log("LetrasA");
        }
    }
}
