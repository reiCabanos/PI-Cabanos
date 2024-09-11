using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class TrigerDialogo : MonoBehaviour
{
    // Start is called before the first frame update
    public Dialogo _dialogo;
    public PainelTutorial _painelTutorial;

    void Start()
    {
        _painelTutorial = Camera.main.GetComponent<PainelTutorial>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DialogoTriger"))
        {
            _painelTutorial.PainelOn(true, _dialogo);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("DialogoTriger"))
        {
            _painelTutorial.PainelOn(false, null);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
