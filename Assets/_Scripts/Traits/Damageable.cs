using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Damageable : MonoBehaviour
{
    public byte health = 4;
    public GameObject explosion;

    public Type t;

    public OnDeath OnDeathHandler;
    private bool isPlayer;

    void Awake()
    {
        isPlayer = TryGetComponent<PlayerController>(out PlayerController player);
    }

    void Update()
    {
        if (Mathf.Abs(transform.position.x) > 10)
        {
            ApplyDamage(health, false);
        }
    }

    void OnParticleCollision(GameObject other)
    {
        if (!isPlayer)
        {
            ApplyDamage();
        }
    }

    // void OnCollisionEnter2D(Collision2D collision)
    // {
    // bool isSuccess = collision.gameObject.TryGetComponent<PlayerController>(out PlayerController playerController);
    // if (isSuccess)
    // {
    //     Debug.Log("No Touchie");
    //     ApplyDamage(health);
    // }
    // ApplyDamage(health);
    // }

    public void ApplyDamage(byte value = 1, bool spawnExplosion = true)
    {
        health -= value;

        if (health <= 0)
        {
            if (spawnExplosion)
            {
                Instantiate(explosion, transform.position, Quaternion.identity);
            }
            OnDeathHandler(this);
            Destroy(gameObject);
        }
    }
}

public delegate void OnDeath(Damageable thisEnemy);
