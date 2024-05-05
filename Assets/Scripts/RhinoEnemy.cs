using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhinoEnemy : MonoBehaviour
{
    public Transform player;
    public float patrolSpeed = 2f;
    public float boostedSpeed = 10f; // Speed during the boost
    public float detectionRadius = 5f; // This could be the trigger radius for boosting
    public float bounceForce = 300f; // Amount of force to apply for the bounce, adjustable in Inspector
    public float wallHitDelay = 1.5f;
    public float boostDelay = .5f;
    public float bounceDelay = .5f;
    private Vector2 patrolDirection = Vector2.right;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    enum State
    {
        Patrolling,
        Boosting,
        BouncingBack
    }

    private State currentState = State.Patrolling;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Patrolling:
                Patrol();
                CheckForBoostTrigger();
                break;
            case State.Boosting:
                Boost();
                break;
            case State.BouncingBack:
                rb.velocity = new Vector2(0 * patrolDirection.x, rb.velocity.y);
                break;
        }
    }

    void Patrol()
    {
        rb.velocity = new Vector2(patrolSpeed * patrolDirection.x, rb.velocity.y);
        FlipSprite(patrolDirection.x);
    }

    void CheckForBoostTrigger()
    {
        // Check if the player is within the detection radius and in front of the enemy
        Vector2 toPlayer = player.position - transform.position;
        float distanceToPlayer = toPlayer.magnitude;
        float dotProduct = Vector2.Dot(patrolDirection, toPlayer.normalized);

        if (distanceToPlayer <= detectionRadius && dotProduct > 0) // dotProduct > 0 means player is in front
        {
            StartCoroutine(DelayedBoostStart());
        }
    }

    void Boost()
    {
        rb.velocity = new Vector2(boostedSpeed * patrolDirection.x, rb.velocity.y);
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            Vector2 bounceDirection = -rb.velocity.normalized; // Reverse the current direction
            bounceDirection += Vector2.up; // Add an upward force component
            bounceDirection.Normalize(); // Normalize to ensure consistent force application

            rb.AddForce(bounceDirection * bounceForce, ForceMode2D.Impulse);            
            BounceBack();
        }
    }

    void BounceBack()
    {
        currentState = State.BouncingBack;
        StartCoroutine(BounceDelay());
    }

    void FlipSprite(float direction)
    {
        spriteRenderer.flipX = direction < 0;
    }

    IEnumerator ResetToPatrol()
    {
        
        yield return new WaitForSeconds(wallHitDelay); // Add a brief delay
        patrolDirection = -patrolDirection; // Flip patrol direction
        currentState = State.Patrolling;
    }

    IEnumerator DelayedBoostStart()
    {
        yield return new WaitForSeconds(boostDelay); // Wait for the specified delay
        currentState = State.Boosting;
    }

    IEnumerator BounceDelay()
    {
        yield return new WaitForSeconds(bounceDelay); // Wait for the specified delay
        currentState = State.BouncingBack;
        StartCoroutine(ResetToPatrol());
    }
}
