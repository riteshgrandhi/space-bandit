using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : Damageable
{
    public PlayerConfig playerConfig;
    private static Vector2 BOUNDS = new Vector2(7.5f, 7);
    private Rigidbody2D rb;
    private ParticleSystem bulletParticleSystem;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        bulletParticleSystem = GetComponentInChildren<ParticleSystem>();
    }

    protected override void Update()
    {
        base.Update();
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Enemy>(out Enemy e))
        {
            ApplyDamage(health);
        }
    }
}