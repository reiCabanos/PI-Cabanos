using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] int _fase;
    [SerializeField] int _partFase;
    [SerializeField] int _life;
    [SerializeField] Transform[] _pos;
    [SerializeField] Transform _posPlayer;
    void Start()
    {

        // acessar a memoria do computador
        /* PlayerPrefs.SetInt("fase", 0);
         _fase = PlayerPrefs.GetInt("fase");
        */
        Carregar();
       // _posPlayer.transform.localPosition = _pos[_partFase].transform.position;
        
    }

    // Update is called once per frame
    public void AumentarFase()
    {
        _fase++;
    }
    public void Salvar()
    {
        PlayerPrefs.SetInt("fase", _fase);
    }
    public void Carregar()
    {
        _fase = PlayerPrefs.GetInt("fase");
    }
    void Update()
    {

    }
    public void CheckPointSalvar(Vector3 pos)
    {
        PlayerPrefs.SetFloat("posX", pos.x);
        PlayerPrefs.SetFloat("posY", pos.y);
        PlayerPrefs.SetFloat("posZ", pos.z);


    }
}

