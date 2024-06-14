using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float Health = 11f;
    public float speed = 2f;
    public Transform groundDetection;
    public Transform player;
    public float detectionRange = 5f;
    public float fovAngle = 45f;
    public LayerMask playerLayer;
    public float chaseDuration = 2f;
    public float damageAmount = 1f;
    public float damageInterval = 1f;
    public float capsuleHeight = 2f; // Capsule height
    public float capsuleRadius = 0.5f; // Capsule radius
    public float moveDirection = 1f;
    public float stoppingDistance = 0.5f;

    Animator anim;
    Rigidbody2D rb;
    Transform t;

    private bool movingRight = true;
    private bool isChasing = false;
    private Coroutine stopChaseCoroutine;
    private bool isPlayerInsideCapsule = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        t = transform;
        StartCoroutine(DamagePlayer());
    }

    void Update()
    {
        if (Health <= 0)
        {
            Die();
        }

        CheckCapsuleCast();
    }

    private void FixedUpdate()
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

    void Die()
    {
        //anim.SetTrigger("Dead); <-- no futuro pra animação de morte
        Destroy(gameObject);
    }

    void Patrol()
    {
        rb.velocity = new Vector2((moveDirection) * speed, rb.velocity.y);

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 1f);
        if (groundInfo.collider == false)
        {
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
    }

    void CheckForPlayer()
    {
        Vector2 direction = movingRight ? Vector2.right : Vector2.left;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRange, playerLayer);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                Vector2 directionToPlayer = (hit.transform.position - transform.position).normalized;
                float angleToPlayer = Vector2.Angle(direction, directionToPlayer);

                if (angleToPlayer < fovAngle / 2)
                {
                    isChasing = true;
                    if (stopChaseCoroutine != null)
                    {
                        StopCoroutine(stopChaseCoroutine);
                    }
                    return;
                }
            }
        }
    }

    void FollowPlayer()
    {
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            Vector2 distance = (player.position - transform.position);
            if((distance.x * moveDirection) > stoppingDistance)
            {
                rb.velocity = new Vector2(direction.x, 0) * speed;
            }
            else
            {
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

            if (Vector2.Distance(transform.position, player.position) > detectionRange)
            {
                if (stopChaseCoroutine == null)
                {
                    stopChaseCoroutine = StartCoroutine(StopChaseAfterDelay());
                }
            }
        }
    }

    IEnumerator StopChaseAfterDelay()
    {
        yield return new WaitForSeconds(chaseDuration);
        isChasing = false;
        stopChaseCoroutine = null;
    }

    void Flip()
    {
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

    void CheckCapsuleCast()
    {
        Vector2 point1 = new Vector2(transform.position.x, transform.position.y + capsuleHeight);
        Vector2 point2 = new Vector2(transform.position.x, transform.position.y + 0.55f);
        float radius = capsuleRadius;

        RaycastHit2D hit = Physics2D.CapsuleCast(point1, point2, CapsuleDirection2D.Vertical, 0f, Vector2.right, 0f, playerLayer);
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            isPlayerInsideCapsule = true;
        }
        else
        {
            isPlayerInsideCapsule = false;
        }
    }

    IEnumerator DamagePlayer()
    {
        while (true)
        {
            if (isPlayerInsideCapsule && player != null)
            {
                player.GetComponent<PlayerController>().TakeDamage(damageAmount);
            }
            yield return new WaitForSeconds(damageInterval);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Vector3 fovLine1 = Quaternion.AngleAxis(fovAngle / 2, Vector3.forward) * transform.right * detectionRange;
        Vector3 fovLine2 = Quaternion.AngleAxis(-fovAngle / 2, Vector3.forward) * transform.right * detectionRange;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, fovLine1);
        Gizmos.DrawRay(transform.position, fovLine2);

        Gizmos.color = Color.green;
        Vector2 point1 = new Vector2(transform.position.x, transform.position.y + capsuleHeight);
        Vector2 point2 = new Vector2(transform.position.x, transform.position.y + 0.55f);
        float radius = capsuleRadius;

        Gizmos.DrawWireSphere(point1, radius);
        Gizmos.DrawWireSphere(point2, radius);

        Gizmos.DrawLine(point1 + Vector2.left * radius, point2 + Vector2.left * radius);
        Gizmos.DrawLine(point1 + Vector2.right * radius, point2 + Vector2.right * radius);
    }
}
