using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedKnight : Enemy
{
    public float redKnightHealth = 12f;
    public float redKnightDamageInterval = 1f;
    public float redKnightDamage = 10f;
    public bool startFacingRight = true;


    void Start()
    {
        maxHealth = redKnightHealth;
        damageAmount = redKnightDamage;
        damageInterval = redKnightDamageInterval;
        capsuleHeight = 2f;
        stoppingDistance = 0.5f;
        movingRight = startFacingRight;

        player = GameObject.Find("Player").transform;

        currentHealth = maxHealth;
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerScript = player.GetComponent<PlayerController>();
        t = transform;

        if (movingRight)
        {
            moveDirection = 1;
            t.localScale = new Vector3(Mathf.Abs(t.localScale.x), t.localScale.y, t.localScale.z);
        }
        else
        {
            moveDirection = -1;
            t.localScale = new Vector3(-Mathf.Abs(t.localScale.x), t.localScale.y, t.localScale.z);
        }
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            isDead = true;
            anim.SetBool("isDead", true);
        }
        if (!isDead && !playerScript.isDead)
        {
            CheckCapsuleCast();
        }
    }

    void FixedUpdate()
    {
        if (!isDead && !playerScript.isDead)
        {
            if (!isChasing)
            {
                Patrol();
                CheckForPlayer();
            }
            else
            {
                FollowPlayer();
            }
        }
        else if (!isDead && playerScript.isDead && !anim.GetBool("isIdle"))
        {
            anim.SetBool("isIdle", true);
        }
    }
}
