using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    // Move player in 2D space
    public float speed = 5f;
    public float jumpHeight = 50f;
    public float Health;
    public bool isDead = false;
    public Camera mainCamera;
    public LayerMask groundLayer;
    public GameObject bulletPrefab;
    public GameObject Gun;
    public GameObject GunTip;

    bool isGrounded;
    bool facingRight = true;
    float moveDirection = 0;
    Vector3 cameraPos;
    Rigidbody2D rb;
    BoxCollider2D mainCollider;
    Animator anim;
    Transform t;

    void GroundCheck()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 1f;

        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
        if (hit.collider != null)
        {
            anim.SetBool("isJumping", false);
            isGrounded = true;
        }
        else
        {
            anim.SetBool("isJumping", true);
            isGrounded = false;
        }
    }

    // Use this for initialization
    void Start()
    {
        t = transform;
        rb = GetComponent<Rigidbody2D>();
        mainCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        facingRight = t.localScale.x > 0;

        if (mainCamera)
        {
            cameraPos = mainCamera.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Health <= 0)
        {
            isDead = true;
            Gun.SetActive(false);
            anim.SetBool("isDead", true);
        }

        // Movement controls
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            moveDirection = Input.GetKey(KeyCode.A) ? -1 : 1;
        }
        else
        {
            if (isGrounded)
            {
                moveDirection = 0;
            }
        }

        //Shooting controls
        if(Input.GetMouseButtonDown(0) && !isDead)
        {
            Animator GunAnim = Gun.GetComponent<Animator>();
            GunAnim.SetTrigger("IsShooting");
            GameObject bullet = (GameObject)Instantiate(bulletPrefab, GunTip.transform.position, transform.rotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (facingRight)
            {
                bulletScript.moveToX = 1;
            }
            else
            {
                bulletScript.moveToX = -1;
            }
        }

        // Change facing direction
        if (moveDirection != 0 && !isDead)
        {
            if (moveDirection > 0 && !facingRight)
            {
                facingRight = true;
                t.localScale = new Vector3(Mathf.Abs(t.localScale.x), t.localScale.y, transform.localScale.z);
            }
            if (moveDirection < 0 && facingRight)
            {
                facingRight = false;
                t.localScale = new Vector3(-Mathf.Abs(t.localScale.x), t.localScale.y, t.localScale.z);
            }
        }

        // Jumping
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && isGrounded && !isDead)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            //anim.SetBool("isJumping", true);
        }

        // Camera follow
        if (mainCamera)
        {
            mainCamera.transform.position = new Vector3(t.position.x, cameraPos.y, cameraPos.z);
        }
    }

    void FixedUpdate()
    {
        // Apply movement velocity
        if (!isDead)
        {
            rb.velocity = new Vector2((moveDirection) * speed, rb.velocity.y);
        }
        if(rb.velocity.x != 0 && !isDead)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
        GroundCheck();
        anim.SetFloat("yVelocity", rb.velocity.y);
    }
}