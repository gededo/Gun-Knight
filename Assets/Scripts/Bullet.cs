using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public Rigidbody2D rb;
    //public GameObject Player;
    public float moveToX;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 0.7f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //rb.AddForce(new Vector2((moveToX) * 20, 0));
        rb.velocity = new Vector2((moveToX) * speed, rb.velocity.y);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Enemy enemyScript = collision.GetComponent<Enemy>();
            enemyScript.Health -= 5f;
        }
        Destroy(gameObject);
    }
}
