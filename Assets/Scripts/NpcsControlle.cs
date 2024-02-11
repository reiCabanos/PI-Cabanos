using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcsControlle : MonoBehaviour
{
    //public Transform _alvo;
    
    [SerializeField] ControleNpc _ContNpc;
    public List<GameObject> _iniVivo_1L;
    public List<GameObject> _iniMorto_1L;
    public List<Transform> _inifixo;
    [SerializeField] Transform _pos;
    public List<Transform> _pos1;
    public float timer = 15;
    float oldTimer;
    bool isRunning = true;




    void Start()
    {
        
        _ContNpc = Camera.main.GetComponent<ControleNpc>();
        oldTimer = timer;
    }
    void Update()
    {
        if (isRunning)
        {
            oldTimer -= Time.deltaTime;
           

            if (oldTimer < 0) { 
               
                oldTimer = timer;
                Invoke("InimigoStart1",0.1f);
                Invoke("InimigoStart2", 0.1f);
                Invoke("InimigoIten1", 0.1f);
                Invoke("NpcStart3", 0.1f);
                Invoke("NpcStart4", 0.1f);
                timer = Random.Range(3, 6);
                oldTimer = timer;
            }
        }

    }

  
    public void InimigoStart1()
    {
        GameObject bullet = Npc1.SharedInstance.GetPooledObject();
        if (bullet != null)
        {
            bullet.transform.position = _pos.position;
            bullet.GetComponent<SeguirPlayer>()._alvo = _pos1[0];
            bullet.transform.SetParent(_ContNpc._inimigosMae);
            _iniVivo_1L.Add(bullet);
            _iniMorto_1L.Remove(bullet);
            bullet.SetActive(true);
            
        }

    }
    public void InimigoStart2()
    {
        GameObject bullet = Npc2.SharedInstance.GetPooledObject();
        if (bullet != null)
        {
            bullet.transform.position = _pos.position;

            bullet.GetComponent<SeguirPlayer>()._alvo = _pos1[0];
            bullet.transform.SetParent(_ContNpc._inimigosMae);
            _iniVivo_1L.Add(bullet);
            _iniMorto_1L.Remove(bullet) ;
            bullet.SetActive(true);
        }
    }

    public void InimigoIten1()
    {
        GameObject bullet = DropItens.SharedInstance.GetPooledObject();
        if (bullet != null)
        {
            
            int number = Random.Range(0, _inifixo.Count);
            bullet.transform.position = _inifixo[number].position;
            bullet.transform.SetParent(_ContNpc._itensmap);
           
            bullet.SetActive(true);

        }
    }

    public void NpcStart3()
    {


        GameObject bullet = Npc3.SharedInstance.GetPooledObject();
        if (bullet != null)
        {
            bullet.transform.position = _pos.position;

            bullet.GetComponent<SeguirPlayer>()._alvo = _pos1[0];
            bullet.transform.SetParent(_ContNpc._inimigosMae);
            _iniVivo_1L.Add(bullet);
            _iniMorto_1L.Remove(bullet);
            bullet.SetActive(true);
        }


    }
    public void NpcStart4()
    {


        GameObject bullet = Npc4.SharedInstance.GetPooledObject();
        if (bullet != null)
        {
            bullet.transform.position = _pos.position;

            bullet.GetComponent<SeguirPlayer>()._alvo = _pos1[0];
            bullet.transform.SetParent(_ContNpc._inimigosMae);
            _iniVivo_1L.Add(bullet);
            _iniMorto_1L.Remove(bullet);
            bullet.SetActive(true);
        }


    }



}
