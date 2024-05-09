using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhinoEnemy : MonoBehaviour
{
    public Transform player;
    // Current state
    public State rhinoState;
    // Speed when in patrolling state
    public float patrolSpeed = 2f;
    // Speed during the boost
    public float boostedSpeed = 10f;
    // Trigger radius for dashing
    public float detectionRadius = 5f; 
    // Amount of force to apply for the bounce, adjustable in Inspector
    public float bounceForce = 10f; 
    // Delay after hitting wall
    public float wallHitDelay = 1.5f;
    // Delay before starting dash
    public float boostDelay = .5f; 
    // Delay before returning to patrol mode
    public float fatigueDuration = .5f;

    private Vector2 patrolDirection = Vector2.left;

    private Rigidbody2D rb;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private AudioSource audioSource;

    //This represents the audio manager game object 
    AudioManager audioManager;

    public enum State
    {
        Patrolling,
        Dashing,
        Fatigued,
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rhinoState = State.Patrolling;
        animator = GetComponent<Animator>();
        //Find the audio manager game object
        audioManager = FindAnyObjectByType<AudioManager>().GetComponent<AudioManager>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        switch (rhinoState)
        {
            case State.Patrolling:
                animator.SetBool("Dashing", false);
                animator.SetBool("WallHit", false);
                Patrol();
                CheckForBoostTrigger();
                break;
            case State.Dashing:
                animator.SetBool("Dashing", true);
                Boost();
                break;
            case State.Fatigued:
                
                rb.velocity = new Vector2(0 * patrolDirection.x, rb.velocity.y); //Stops movement
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
        rb.velocity = new Vector2(boostedSpeed * patrolDirection.x, rb.velocity.y); // Increases speed
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            audioManager.Play(AudioManager.Sound.RhinoDamage); // Plays wall hit sound
            Vector2 bounceDirection = -rb.velocity.normalized; // Reverse the current direction
            bounceDirection += Vector2.up; // Add an upward force component
            bounceDirection.Normalize(); // Normalize to ensure consistent force application

            rb.AddForce(bounceDirection * bounceForce, ForceMode2D.Impulse);
            StartCoroutine(Recovering());
        }
    }
    // Flips rhino sprite
    void FlipSprite(float direction)
    {
        spriteRenderer.flipX = direction < 0;
    }

    // Delays boost
    IEnumerator DelayedBoostStart()
    {
        yield return new WaitForSeconds(boostDelay); // Wait for the specified delay
        rhinoState = State.Dashing;
    }

    // Delays time before Rhino recovers
    IEnumerator Recovering()
    {
        audioSource.Pause();
        animator.SetBool("WallHit", true);
        yield return new WaitForSeconds(fatigueDuration); // Wait for the specified delay
        rhinoState = State.Fatigued;
        StartCoroutine(ResetToPatrol());
    }

    // Resets state to patrolling
    IEnumerator ResetToPatrol()
    {
        yield return new WaitForSeconds(wallHitDelay); // Add a brief delay
        audioSource.Play();
        patrolDirection = -patrolDirection; // Flip patrol direction
        rhinoState = State.Patrolling;
    }
}
