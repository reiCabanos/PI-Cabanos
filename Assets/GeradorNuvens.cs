using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeradorNuvens : MonoBehaviour
{
    
    [SerializeField]
    GameObject[] clouds;

    [SerializeField]
    float spawnInterval;

    [SerializeField]
    GameObject endPoint;



    Vector3 startPos;

    public GameObject _temp;

    // Object pool
    private List<GameObject> cloudPool;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        cloudPool = new List<GameObject>();
        Invoke("AttemptSpawn", spawnInterval);
    }



    void AttemptSpawn()
    {
        GetCloudFromPool();
    }




    void GetCloudFromPool()
    {
        GameObject bullet = ObjectPool.SharedInstance.GetPooledObject();
        if (bullet != null)
        {
            bullet.transform.position = transform.position;

            // bullet.transform.rotation = turret.transform.rotation;
            bullet.SetActive(true);
            bullet.GetComponent<Nuvens>().Destruir();
            Invoke("AttemptSpawn", spawnInterval);
        }
    }
}
