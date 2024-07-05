using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Thief : Enemy
{
    float shootDamageInterval = 2f;
    bool resetShootCooldown = true;

    public float thiefMaxHealth = 9f;
    public float thiefSpeed = 2f;
    public float thiefDamageInterval = 1f;
    public float thiefCapsuleHeight = 2f;
    public float thiefStoppingDistance = 7f;
    public float thiefRetreatDistance = 5f;
    public float thiefChaseDuration = 1f;
    public float damageAmountKnife;
    public float damageAmountArrow;
    public bool startFacingRight = true;
    public GameObject Bow;
    public GameObject Arrow;
    public Transform BowTip;
    public Transform groundDetectionBack;

    [SerializeField] private AudioClip arrowSoundClip;
    [SerializeField] private AudioClip attackSoundClip;

    void Start()
    {
        maxHealth = thiefMaxHealth;
        speed = thiefSpeed;
        damageInterval = thiefDamageInterval;
        capsuleHeight = thiefCapsuleHeight;
        stoppingDistance = thiefStoppingDistance;
        retreatDistance = thiefRetreatDistance;
        chaseDuration = thiefChaseDuration;
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
            PlayDeathSound();
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
        else if (!isDead && playerScript.isDead && !anim.GetBool("isIdle"))
        {
            anim.SetBool("isIdle", true);
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
                rb.velocity = new Vector2((direction.x * speed), rb.velocity.y);
            }
            else if ((distance.x * moveDirection) < stoppingDistance && (distance.x * moveDirection) < retreatDistance)
            {

                RaycastHit2D groundInfoBack = Physics2D.Raycast(groundDetectionBack.position, Vector2.down, 0.5f, groundLayer);
                if (groundInfoBack.collider == true)
                {
                    anim.SetBool("isIdle", false);
                    rb.velocity = new Vector2((-direction.x * speed), rb.velocity.y);
                }
                else
                {
                    anim.SetBool("isIdle", true);
                    rb.velocity = new Vector2(0f, rb.velocity.y);
                }
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

            RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 0.5f, groundLayer);
            if (groundInfo.collider == false)
            {
                anim.SetBool("isIdle", true);
                rb.velocity = new Vector2(0f, rb.velocity.y);
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
                SoundFXManager.instance.PlaySoundFXCLip(arrowSoundClip, transform, 1f);
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

    public void doMeleeAttackSound()
    {
        SoundFXManager.instance.PlaySoundFXCLip(attackSoundClip, transform, 1f);
    }
}
