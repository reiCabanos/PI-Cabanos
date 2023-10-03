using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuVoltar : MonoBehaviour
{
    [SerializeField] private string _nomeDaScena;
    [SerializeField] GameObject _painelVoltar;
    public void Voltar()
    {
        SceneManager.LoadScene(_nomeDaScena);

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
