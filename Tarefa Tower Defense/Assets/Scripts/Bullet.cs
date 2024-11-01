using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static Bullet instance;
    private void Awake()
    {
        instance = this;
    }
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] public float bulletdamage = 1f;
    [SerializeField] private float lifetime = 5f;

    private Transform target;

    private void Start()
    {

        Destroy(gameObject, lifetime);
    }


    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    private void FixedUpdate()
    {

        if (!target) return;


        Vector2 direction = (target.position - transform.position).normalized;


        rb.velocity = direction * bulletSpeed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        Health enemyHealth = other.gameObject.GetComponent<Health>();


        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(bulletdamage);

        }


        Destroy(gameObject);
    }
}