using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Damageable : MonoBehaviour
{

    public int health = 100;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnParticleCollision(GameObject other)
    {
        ApplyDamage();
    }

    public void ApplyDamage()
    {
        health -= 10;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
