using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public Rigidbody2D rb;
    public float moveToX;
    public float playerBulletDamage;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 0.7f);
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2((moveToX) * speed, rb.velocity.y);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Enemy enemyScript = collision.GetComponent<Enemy>();
            if (enemyScript != null && enemyScript.isDead == false)
            {
                enemyScript.TakeDamage(playerBulletDamage);
                Destroy(gameObject);
            }
        }
        else if (collision.gameObject.tag != "Non-Collidable")
        {
            Destroy(gameObject);
        }
    }
}
