using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(ParticleSystem))]
public class PlayerController : Damageable
{
    public PlayerConfig playerConfig;
    private static Vector2 BOUNDS = new Vector2(7.5f, 7);
    private Rigidbody2D rb;
    private ParticleSystem bulletParticleSystem;

    public GameObject impactExplosion;
    public List<ParticleCollisionEvent> collisionEvents;

    void Awake()
    {
        collisionEvents = new List<ParticleCollisionEvent>();
        rb = GetComponent<Rigidbody2D>();
        bulletParticleSystem = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * playerConfig.speed;


        var xValidPosition = Mathf.Clamp(rb.position.x, -BOUNDS.x, BOUNDS.x);
        var yValidPosition = Mathf.Clamp(rb.position.y, -BOUNDS.y, BOUNDS.y);
        rb.position = new Vector2(xValidPosition, yValidPosition);

        if (Input.GetKeyUp(KeyCode.Space))
        {
            bulletParticleSystem.Stop();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            bulletParticleSystem.Play();
        }
    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = bulletParticleSystem.GetCollisionEvents(other, collisionEvents);

        for (int i = 0; i < numCollisionEvents; i++)
        {
            Instantiate(impactExplosion, collisionEvents[i].intersection, Quaternion.identity);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.TryGetComponent<Enemy>(out Enemy e))
        {
            Debug.Log("Daamn");
            ApplyDamage(health);
        }
    }
}