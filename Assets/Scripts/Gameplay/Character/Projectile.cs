using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 3f;
    public int damage = 1;

    private Vector2 direction;
    private float damageMultiplier = 1f;

    public void Initialize(Vector2 shootDirection, float multiplier)
    {
        direction = shootDirection.normalized;
        damageMultiplier = multiplier;
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Health health = collision.GetComponent<Health>();

            if (health != null)
                health.TakeDamage(Mathf.RoundToInt(damage * damageMultiplier));

            Destroy(gameObject);
        }
    }
}