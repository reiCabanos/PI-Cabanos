using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolM : MonoBehaviour
{
    [HideInInspector] public GameObject prefab;
    public int poolSize = 100;

    [Header("public for read only purposes")]
    public List<GameObject> objectPool;

    private void Start()
    {
        // Create the object pool and prepopulate it with objects
        objectPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            objectPool.Add(obj);
        }
    }
    public GameObject GetObject()
    {
        // Find the first inactive object in the pool and return it
        foreach (GameObject obj in objectPool)
        {
            if (obj != null && !obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        // If no inactive objects are found, create a new one and add it to the pool
        GameObject newObj = Instantiate(prefab, transform);
        objectPool.Add(newObj);
        return newObj;
    }

    public void ReturnObject(GameObject obj)
    {
        // Deactivate the object and return it to the pool
        obj.SetActive(false);
    }
}
