using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destruirmivems : MonoBehaviour
{
    private float _speed;
    private float _endPosX;



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

        if (transform.position.x > _endPosX)
        {
            Destroy(gameObject);
        }
    }
}
