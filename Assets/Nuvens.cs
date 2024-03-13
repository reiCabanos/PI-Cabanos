using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nuvens : MonoBehaviour
{
   
    [SerializeField] float _speed = 15;
    [SerializeField] float _endPosX;



    // Start is called before the first frame update
    public void StarFloating(float speed, float endPosx)
    {
        _speed = speed;
        _endPosX = endPosx;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * (Time.deltaTime * _speed));
    }

    public void Destruir()
    {
        Invoke("DestruirTime", 10);
    }
    void DestruirTime()
    {
        gameObject.SetActive(false);
    }
}
