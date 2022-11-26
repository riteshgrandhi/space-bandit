using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AstroidController : Enemy
{
    public float force = 4;
    private Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Vector2 targetPosition = new Vector2(-10, Random.Range(7, -7));

        rb.AddForce((targetPosition -  AsVector2(rb.transform.position)) * force);
    }

    public Vector2 AsVector2(Vector3 _v)
    {
        return new Vector2(_v.x, _v.y);
    }
}