using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Damageable : MonoBehaviour
{
    public byte health = 4;
    public GameObject explosion;

    public Type t;

    void OnParticleCollision(GameObject other)
    {
        ApplyDamage();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("No Touchie");
        // bool isSuccess = collision.gameObject.TryGetComponent<PlayerController>(out PlayerController playerController);
        // if (isSuccess)
        // {
        //     ApplyDamage(health);
        // }
        ApplyDamage(health);
    }

    public void ApplyDamage(byte value = 1)
    {
        health -= value;

        if (health <= 0)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
