using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float Health = 11f; // Enemy's health

    public float speed = 2f; // Enemy's movement speed
    public Transform groundDetection; // Transform used for ground detection
    public Transform player; // Reference to the player
    public float detectionRange = 5f; // Detection range for the player
    public float fovAngle = 45f; // Field of vision angle
    public LayerMask playerLayer; // Layer mask to specify what layers the enemy should detect
    public float chaseDuration = 2f; // Time to wait before stopping the chase

    private bool movingRight = true; // Determines if the enemy is moving right
    private bool isChasing = false; // Determines if the enemy is currently chasing the player
    private Coroutine stopChaseCoroutine; // Coroutine to stop chasing after a delay

    void Update()
    {
        if (!isChasing)
        {
            Patrol(); // Patrol if not chasing the player
            CheckForPlayer(); // Check if the player is in detection range
        }
        else
        {
            FollowPlayer(); // Follow the player if chasing
        }

        if (Health <= 0)
        {
            Destroy(gameObject); // Destroy the enemy if health is zero or below
        }
    }

    void Patrol()
    {
        // Move the enemy in the current direction
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        // Detect ground
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 1f);
        if (groundInfo.collider == false)
        {
            // If there is no ground, flip the enemy's direction
            if (movingRight)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }
        }
    }

    void CheckForPlayer()
    {
        // Determine the direction the enemy is facing
        Vector2 direction = movingRight ? Vector2.right : Vector2.left;

        // Check if the player is within detection range
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRange, playerLayer);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                // Calculate the angle between the enemy's forward direction and the direction to the player
                Vector2 directionToPlayer = (hit.transform.position - transform.position).normalized;
                float angleToPlayer = Vector2.Angle(direction, directionToPlayer);

                // Check if the player is within the FOV angle
                if (angleToPlayer < fovAngle / 2)
                {
                    isChasing = true; // Start chasing the player
                    if (stopChaseCoroutine != null)
                    {
                        StopCoroutine(stopChaseCoroutine); // Stop any existing stop chase coroutine
                    }
                    return; // Exit the loop as player is detected
                }
            }
        }
    }

    void FollowPlayer()
    {
        if (player != null)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, (new Vector2(player.position.x, transform.position.y)), step); // Move towards the player

            // Determine the direction to the player and flip the enemy accordingly
            if (player.position.x > transform.position.x && !movingRight)
            {
                Flip();
            }
            else if (player.position.x < transform.position.x && movingRight)
            {
                Flip();
            }

            // Detect ground
            RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 1f);
            if (groundInfo.collider == false)
            {
                // If there is no ground, stop chasing and flip direction
                isChasing = false;
                if (movingRight)
                {
                    transform.eulerAngles = new Vector3(0, -180, 0);
                    movingRight = false;
                }
                else
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    movingRight = true;
                }
            }

            // If the player moves out of detection range, start the coroutine to stop chasing
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
        yield return new WaitForSeconds(chaseDuration); // Wait for the specified chase duration
        isChasing = false; // Stop chasing the player
        stopChaseCoroutine = null; // Reset the coroutine reference
    }

    void Flip()
    {
        movingRight = !movingRight; // Toggle the moving direction
        transform.eulerAngles = new Vector3(0, movingRight ? 0 : -180, 0); // Flip the enemy's orientation
    }

    // Optional: Visualize the FOV in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange); // Draw the detection range

        Vector3 fovLine1 = Quaternion.AngleAxis(fovAngle / 2, Vector3.forward) * transform.right * detectionRange;
        Vector3 fovLine2 = Quaternion.AngleAxis(-fovAngle / 2, Vector3.forward) * transform.right * detectionRange;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, fovLine1); // Draw one side of the FOV cone
        Gizmos.DrawRay(transform.position, fovLine2); // Draw the other side of the FOV cone
    }
}