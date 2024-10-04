using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobo : Enemy
{
    public float LoboMaxHealth = 12f;
    public float LoboSpeed = 2.5f;
    public float LoboDamage = 10f;
    public float LoboDamageInterval = 1f;
    public float LoboCapsuleHeight = 2f;
    public float LoboStoppingDistance = 0.5f;
    public float LoboChaseDuration = 3.5f;
    public bool startFacingRight = true;
    private bool isAttacking = false;

    [SerializeField] private AudioClip attackSoundClip;


    void Start()
    {
        maxHealth = LoboMaxHealth;
        speed = LoboSpeed;
        damageAmount = LoboDamage;
        damageInterval = LoboDamageInterval;
        capsuleHeight = LoboCapsuleHeight;
        stoppingDistance = LoboStoppingDistance;
        chaseDuration = LoboChaseDuration;
        movingRight = startFacingRight;

        player = GameObject.Find("Player").transform;

        currentHealth = maxHealth;
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerScript = player.GetComponent<PlayerController>();
        t = transform;

        impulseSource = GetComponent<CinemachineImpulseSource>();

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
            if (!isChasing && !isAttacking) // Prevent movement while attacking
            {
                Patrol();
                CheckForPlayer();
            }
            else if (isChasing && !isAttacking) // Prevent movement while attacking
            {
                FollowPlayer();
            }
        }
        else if (!isDead && playerScript.isDead && !anim.GetBool("isIdle"))
        {
            anim.SetBool("isIdle", true);
        }
    }

    public void StopMovement()
    {
        isAttacking = true;
        rb.velocity = Vector2.zero; // Stop the Rigidbody2D movement
    }

    public void ResumeMovement()
    {
        isAttacking = false;
    }

    public void doAttackSound()
    {
        SoundFXManager.instance.PlaySoundFXCLip(attackSoundClip, transform, 1f);
    }
}
