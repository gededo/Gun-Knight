using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedKnight : Enemy
{
    public float redKnightMaxHealth = 12f;
    public float redKnightSpeed = 2.5f;
    public float redKnightDamage = 10f;
    public float redKnightDamageInterval = 1f;
    public float redKnightCapsuleHeight = 2f;
    public float redKnightStoppingDistance = 0.5f;
    public float redKnightChaseDuration = 3.5f;
    public bool startFacingRight = true;

    [SerializeField] private AudioClip attackSoundClip;


    void Start()
    {
        maxHealth = redKnightMaxHealth;
        speed = redKnightSpeed;
        damageAmount = redKnightDamage;
        damageInterval = redKnightDamageInterval;
        capsuleHeight = redKnightCapsuleHeight;
        stoppingDistance = redKnightStoppingDistance;
        chaseDuration = redKnightChaseDuration;
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

    public void doAttackSound()
    {
        SoundFXManager.instance.PlaySoundFXCLip(attackSoundClip, transform, 1f);
    }
}
