using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
public class Damageable : MonoBehaviour
{
    public int scoreIncrement = 100;
    public byte health = 4;
    public GameObject explosion;
    public OnDeath OnDeathHandler;
    public Material flashMaterial;
    private SpriteRenderer spriteRenderer;
    private Material originalMaterial;
    private Coroutine flashRoutine;
    public GameObject impactExplosion;
    public bool isDead = false;
    public List<ParticleCollisionEvent> collisionEvents;
    protected virtual void Awake()
    {
        collisionEvents = new List<ParticleCollisionEvent>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
    }

    protected virtual void Update()
    {
        if (isDead)
        {
            return;
        }

        if (Mathf.Abs(transform.position.x) > 10)
        {
            ApplyDamage(health, false);
        }
    }

    protected virtual void OnParticleCollision(GameObject other)
    {
        if (isDead)
        {
            return;
        }

        Instantiate(impactExplosion, gameObject.transform.position + Vector3.left * gameObject.GetComponent<Collider2D>().bounds.extents.x, Quaternion.identity);

        ApplyDamage();
    }

    public void ApplyDamage(byte value = 1, bool kia = true)
    {
        if (isDead)
        {
            return;
        }

        health -= value;

        if (kia)
        {
            flashRoutine = StartCoroutine(DoHitBlink());
        }

        spriteRenderer.material = flashMaterial;

        if (health <= 0)
        {
            isDead = true;
            if (kia)
            {
                GameManager.Instance.AddScore(scoreIncrement);
                Instantiate(explosion, transform.position, Quaternion.identity);
            }
            if (OnDeathHandler != null)
            {
                OnDeathHandler(this);
            };
            spriteRenderer.material = flashMaterial;
            Destroy(gameObject, kia ? 0.1f : 5f);
        }
    }

    private IEnumerator DoHitBlink()
    {
        spriteRenderer.material = flashMaterial;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material = originalMaterial;
        StopCoroutine(flashRoutine);
        flashRoutine = null;
    }
}

public delegate void OnDeath(Damageable thisEnemy);
