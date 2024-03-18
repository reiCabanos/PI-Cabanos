using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NuvemManager : MonoBehaviour
{
    // Variáveis do Sistema de Nuvens
    [SerializeField]
    GameObject[] clouds;
    [SerializeField]
    float spawnInterval;
    [SerializeField]
    GameObject endPoint;
    [SerializeField]Vector3 startPos;
    List<GameObject> cloudPool;

    // Variáveis do Object Pool
    public static NuvemManager SharedInstance;
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;
    [SerializeField]public float objectToPoolX;



    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        // Inicialização do Object Pool
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }

        // Inicialização do Sistema de Nuvens
        startPos = transform.position;
        cloudPool = new List<GameObject>();
        float objectToPoolX = objectToPool.transform.position.x;
        Prewarm();
        Invoke("AttemptSpawn", spawnInterval);
    }

    void SpawnCloud(Vector3 startPos)
    {
        // Obter uma nuvem do pool de objetos
        int randomIndex = UnityEngine.Random.Range(0, clouds.Length);
        GameObject cloud = GetCloudFromPool(randomIndex);

        // Posicionar e configurar a nuvem
        float randomY = Random.Range(minY, maxY); // Define o intervalo Y para spawn
        cloud.transform.position = new Vector3(startPos.x, randomY, startPos.z);
        float scale = UnityEngine.Random.Range(0.9f, 2.5f); // AQUI MUDAR AS ESCALAS DAS NUVENS 
        cloud.transform.localScale = new Vector2(scale, scale);

        // Ativar a nuvem
        cloud.SetActive(true);
    }

    void AttemptSpawn()
    {
        SpawnCloud(startPos);
        Invoke("AttemptSpawn", spawnInterval);
    }

    void Prewarm()
    {
        int amountToSpawn = 10; // Quantidade de nuvens para spawn inicial

        // Definir o intervalo X e Y
        float minX = -40; // Limite inferior X
        float maxX = 200; // Limite superior X
        float minY = -30; // Limite inferior Y
        float maxY = 45; // Limite superior Y
       

        for (int i = 0; i < amountToSpawn; i++)
        {
            // Gerar posição aleatória dentro do intervalo
            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);
            Vector3 spawnPos = startPos + new Vector3(randomX, randomY,0);

            // Spawnar a nuvem na posição aleatória
            SpawnCloud(spawnPos);
        }
    }



    public GameObject GetCloudFromPool(int index)
    {
        foreach (GameObject cloud in pooledObjects)
        {
            if (!cloud.activeInHierarchy && cloud.name == clouds[index].name)
            {
                cloud.SetActive(true);
                return cloud;
            }
        }

        GameObject newCloud = Instantiate(clouds[index]);
        newCloud.name = clouds[index].name;
        cloudPool.Add(newCloud);
        return newCloud;
    }

    public void DesativarNuvem(GameObject nuvem)
    {
        nuvem.SetActive(false);
    }

    void Update()
    {
        // Mover as nuvens em direção ao ponto final (implementação específica)

        foreach (GameObject nuvem in cloudPool)
        {
            if (nuvem.activeInHierarchy)
            {
                // Mover a nuvem
                // ...

                // Desativar a nuvem quando alcançar o ponto final
                if (nuvem.transform.position.x > endPoint.transform.position.x)
                {
                    DesativarNuvem(nuvem);
                }
            }
        }
    }

    // Variáveis adicionais para definir o intervalo Y de spawn
    public float minY;
    public float maxY;
}
