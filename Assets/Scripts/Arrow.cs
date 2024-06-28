using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Arrow : MonoBehaviour
{
    public float speed = 10f;
    public Rigidbody2D rb;
    public float moveToX;
    public float EnemyArrowDamage;

    void Start()
    {
        rb.velocity = new Vector2(rb.velocity.x, 3f);
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 2f);
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2((moveToX) * speed, rb.velocity.y);
        transform.right = rb.velocity;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController Player = collision.GetComponent<PlayerController>();
            Player.TakeDamage(EnemyArrowDamage);
        }
        Destroy(gameObject);
    }
}