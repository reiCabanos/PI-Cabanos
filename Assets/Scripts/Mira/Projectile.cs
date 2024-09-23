using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float damageToDo = 25f;
    [SerializeField] private float returnTime = 3f;
    private float timer;
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > returnTime)
        {
            gameObject.SetActive(false);    
            timer = 0;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.GetComponent<EnemyHealth>() != null)
        {
            EnemyHealth eh = collision.collider.GetComponent<EnemyHealth>();
            eh.health -= damageToDo;
        }
    }
}
