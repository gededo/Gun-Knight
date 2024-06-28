using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Thief : Enemy
{
    float shootDamageInterval = 2f;
    bool resetShootCooldown = true;

    public float damageAmountKnife;
    public float damageAmountArrow;
    public GameObject Bow;
    public GameObject Arrow;
    public Transform BowTip;

    void Start()
    {
        maxHealth = 100f;
        damageInterval = 1f;
        capsuleHeight = 2f;
        stoppingDistance = 7f;
        retreatDistance = 5f;
        chaseDuration = 1f;
        movingRight = true;

        player = GameObject.Find("Player").transform;

        currentHealth = maxHealth;
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerScript = player.GetComponent<PlayerController>();
        t = transform;
    }

    void Update()
    {

        if (currentHealth <= 0)
        {
            isDead = true;
            Bow.gameObject.SetActive(false);
            anim.SetBool("isDead", true);
        }
        if (!isDead && !playerScript.isDead)
        {
            CheckCapsuleCast();
        }
        if (isPlayerInsideCapsule)
        {
            damageAmount = damageAmountKnife;
        }
        else 
        {
            damageAmount = damageAmountArrow;
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
                
                FollowPlayerThief();
            }
        }
    }

    protected void FollowPlayerThief()
    {

        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            Vector2 distance = (player.position - transform.position);
            if ((distance.x * moveDirection) > stoppingDistance && (distance.x * moveDirection) > retreatDistance)
            {
                anim.SetBool("isIdle", false);
                rb.velocity = new Vector2(direction.x, rb.velocity.y) * speed;
            }
            else if ((distance.x * moveDirection) < stoppingDistance && (distance.x * moveDirection) < retreatDistance)
            {
                anim.SetBool("isIdle", false);
                rb.velocity = new Vector2(-direction.x, rb.velocity.y) * speed;
            }
            else
            {
                anim.SetBool("isIdle", true);
                rb.velocity = new Vector2(0, 0);
            }

            if (player.position.x > transform.position.x && !movingRight)
            {
                Flip();
            }
            else if (player.position.x < transform.position.x && movingRight)
            {
                Flip();
            }

            RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 1f);
            if (groundInfo.collider == false)
            {
                isChasing = false;
                if (movingRight)
                {
                    moveDirection = -1;
                    t.localScale = new Vector3(-Mathf.Abs(t.localScale.x), t.localScale.y, t.localScale.z);
                    movingRight = false;
                }
                else
                {
                    moveDirection = 1;
                    t.localScale = new Vector3(Mathf.Abs(t.localScale.x), t.localScale.y, t.localScale.z);
                    movingRight = true;
                }
            }

            RaycastHit2D wallInfo = Physics2D.Raycast(wallDetection.position, Vector2.right, 0.1f, groundLayer);
            if (wallInfo.collider == true)
            {
                isChasing = false;
                if (movingRight)
                {
                    moveDirection = -1;
                    t.localScale = new Vector3(-Mathf.Abs(t.localScale.x), t.localScale.y, t.localScale.z);
                    movingRight = false;
                }
                else
                {
                    moveDirection = 1;
                    t.localScale = new Vector3(Mathf.Abs(t.localScale.x), t.localScale.y, t.localScale.z);
                    movingRight = true;
                }
            }

            if (Vector2.Distance(transform.position, player.position) > detectionRange)
            {
                if (stopChaseCoroutine == null)
                {
                    stopChaseCoroutine = StartCoroutine(StopChaseAfterDelayThief());
                }
            }

            if (resetShootCooldown && !isPlayerInsideCapsule)
            {
                GameObject arrow = (GameObject)Instantiate(Arrow, BowTip.position, transform.rotation);
                Arrow arrowScript = arrow.GetComponent<Arrow>();
                arrowScript.EnemyArrowDamage = damageAmountArrow;
                if (movingRight)
                {
                    arrowScript.moveToX = 1;
                }
                else
                {
                    arrowScript.moveToX = -1;
                }
                resetShootCooldown = false;
                StartCoroutine(AttackCooldownBow());
            }
        }
    }

    IEnumerator StopChaseAfterDelayThief()
    {
        yield return new WaitForSeconds(chaseDuration);
        isChasing = false;
        stopChaseCoroutine = null;
    }

    IEnumerator AttackCooldownBow()
    {
        yield return new WaitForSeconds(shootDamageInterval);
        resetShootCooldown = true;
    }

    public void disableBow()
    {
        Bow.gameObject.SetActive(false);
    }

    public void enableBow()
    {
        Bow.gameObject.SetActive(true);
    }
}
