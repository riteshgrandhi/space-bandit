using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(ParticleSystem))]
public class PlayerController : MonoBehaviour
{
    public PlayerConfig playerConfig;
    private static Vector2 BOUNDS = new Vector2(10, 6);
    private Rigidbody2D rb;
    private ParticleSystem bulletParticleSystem;

    void Awake()
    {
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
}