using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
public class Damageable : MonoBehaviour
{
    public int scoreIncrement = 100;
    public byte health = 4;
    public GameObject explosion;

    public Type t;

    public OnDeath OnDeathHandler;
    public Material flashMaterial;
    private SpriteRenderer spriteRenderer;
    // private Material originalMaterial;
    // private Coroutine flashRoutine;
    private bool isPlayer;

    void Awake()
    {
        isPlayer = TryGetComponent<PlayerController>(out PlayerController player);
        // spriteRenderer = GetComponent<SpriteRenderer>();
        // Debug.Log(spriteRenderer);
        // originalMaterial = spriteRenderer.material;
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

    public void ApplyDamage(byte value = 1, bool spawnExplosion = true)
    {
        health -= value;

        // StartCoroutine(DoHitBlink());

        if (health <= 0)
        {
            if (spawnExplosion)
            {
                Instantiate(explosion, transform.position, Quaternion.identity);
            }
            if (OnDeathHandler != null)
            {
                OnDeathHandler(this);
            };
            Destroy(gameObject);
            GameManager.Instance.AddScore(scoreIncrement);
        }
    }

    // private IEnumerator DoHitBlink()
    // {
    //     Debug.Log(this.name);
    //     spriteRenderer.material = flashMaterial;
    //     yield return new WaitForSeconds(0.2f);
    //     spriteRenderer.material = originalMaterial;
    //     flashRoutine = null;
    // }
}

public delegate void OnDeath(Damageable thisEnemy);
