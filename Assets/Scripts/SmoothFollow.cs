using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Transform Target;
    public float speed = 2.0f;

    void Update()
    {
        float Interpolation = speed * Time.deltaTime;
        Vector3 position = transform.localPosition;

        position.x = Mathf.Lerp(transform.position.x, Target.position.x, Interpolation);
        position.y = Mathf.Lerp(transform.position.y, Target.position.y+5, Interpolation);
        position.z = Mathf.Lerp(transform.position.z, Target.position.z, Interpolation);

        transform.position = position;
    }
}
