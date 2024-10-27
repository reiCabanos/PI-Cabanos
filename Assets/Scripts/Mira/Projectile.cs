using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float damageToDo = 25f;
    [SerializeField] private float returnTime = 3f;
    private float timer;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Aumenta a influência da gravidade
        rb.mass = 5f; // Aumente o valor da massa
        rb.AddForce(Physics.gravity * rb.mass * 2f); // Multiplica a gravidade padrão
    }
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
