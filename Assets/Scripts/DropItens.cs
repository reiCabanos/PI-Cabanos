using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItens : MonoBehaviour
{
    public static DropItens SharedInstance;
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;
    public NpcsControlle _contreleNpcs;

    

   
    

    void Awake()
    {
        SharedInstance = this;
        _contreleNpcs = Camera.main.GetComponent<NpcsControlle>();


    }

    protected virtual void Start()
    {
       
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
           
        }
       
        
    }


    public GameObject GetPooledObject()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;

    }
    
    
}
