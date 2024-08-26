using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItensControlle : MonoBehaviour
{
    [SerializeField] int _numbFrutas;
    [SerializeField] int _numbTabuas;
    [SerializeField] List<Transform> _posFrutas;
    [SerializeField] List<Transform> _posTabuas;
   


    private void Start()
    {

        Invoke("ItemOn",3);
    }
    void ItemOn()
    {
        Shuffle(_posFrutas);
        for(int i=0; i<_posFrutas.Count; i++)
        {
            Debug.Log("y");
            FrutaOn(i);
            
        }

        for (int i = 0; i < _posTabuas.Count; i++)
        {
            Debug.Log("g");
            TabuasOn(i);
           

        }
        
    }
    
    void TabuasOn(int value)
    {
        GameObject bullet = TabuaPool.SharedInstance.GetPooledObject();
        if (bullet != null)
        {
            bullet.transform.position = _posTabuas[value].transform.position;
            //bullet.transform.rotation = turret.transform.rotation;
            bullet.SetActive(true);
        }
    }
    void FrutaOn(int value)
    {
        GameObject bullet = FrutaPool.SharedInstance.GetPooledObject();
        if (bullet != null)
        {
            bullet.transform.position = _posFrutas[value].transform.position;
            //bullet.transform.rotation = turret.transform.rotation;
            bullet.SetActive(true);
        }
    }
    public void Shuffle(List<Transform> lists)
    {
        for (int j=lists.Count-1;j>0;j--) {
            int rnd = UnityEngine.Random.Range(0, j+1) ;
            Transform temp = lists[j];
            lists[j] = lists[rnd];
            lists[rnd] = temp;
        }
    
    }
}
