using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcsControlle : MonoBehaviour
{
    //public Transform _alvo;
    
    [SerializeField] ControleNpc _ContNpc;
    public List<GameObject> _iniVivo_1L;
    public List<GameObject> _iniMorto_1L;
   [SerializeField] Transform _pos;
    public List<Transform> _pos1;
    public float timer = 15;
    float oldTimer;
    bool isRunning = true;

    void Start()
    {
        
        _ContNpc = Camera.main.GetComponent<ControleNpc>();
        //Invoke("InimigoStart1", 0.5f);
        //Invoke("InimigoStart2", 0.5f);
        oldTimer = timer;
    }
    void Update()
    {
        if (isRunning)
        {
            oldTimer -= Time.deltaTime;
           // GetComponent<Text>().text = "Tempo: " + Mathf.RoundToInt(timer).ToString() + " s";

            if (oldTimer < 0) { 
                //isRunning = false;
                Debug.Log("Aparecer Inimigo!!");
                oldTimer = timer;
                InimigoStart1();
                InimigoStart2();
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
    
}
