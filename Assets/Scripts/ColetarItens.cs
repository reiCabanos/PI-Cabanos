using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColetarItens : MonoBehaviour
{
    [SerializeField] private string _nome;
    [SerializeField] private int _tipo;
    [SerializeField] private int _valor;
    [SerializeField] MeshRenderer  _testura;
    [SerializeField] GameObject _partSaida;
   
    public virtual void Start()
    {
        _testura = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    protected virtual void ConstroyItens()
    {

    }
    public  virtual void DestroyItens()
    {

    }
    public virtual int Tipo
    {
        get { return _tipo; }
        set { _tipo = value; }

    }
    public virtual string Nome
    {
        get { return _nome; }
        set { _nome = value; }
    }
    public virtual int Valor
    {
        get { return _valor; }
        set
        {
            _valor = value;
        }
    }
    public virtual MeshRenderer Testura
    {
        get { return _testura; }
        set
        {
            _testura = value;
        }
            
    }
    public virtual GameObject PartSaida
    {
        get { return _partSaida; }
        set
        {
            _partSaida = value;
        }
    }
    
}
