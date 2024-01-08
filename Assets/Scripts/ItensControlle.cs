using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItensControlle : MonoBehaviour
{
    [SerializeField] int _numbFrutas;
    [SerializeField] List<Transform> _posFrutas;

    private void Start()
    {

        Invoke("ItemOn",3);
    }
    void ItemOn()
    {
        for(int i=0; i<3; i++)
        {
            FrutaOn(i);
        }
        
    }
    void FrutaOn(int value)
    {
        GameObject bullet = FrutaPool.SharedInstance.GetPooledObject();
        if (bullet != null)
        {
            //bullet.transform.position = turret.transform.position;
            //bullet.transform.rotation = turret.transform.rotation;
            bullet.SetActive(true);
        }
    }
}
