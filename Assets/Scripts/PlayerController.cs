using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerController : MonoBehaviour
{
    //configuration thought IDE 
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climeSpeed = 1f;
    [SerializeField] Vector2 deathJump = new Vector2(25, 50);


    // State 
    bool isAlive = true;
    bool isAttacking = false;
    bool canDoubleJump = false;
    float gravityStart;

    //cashed component refernces
    Rigidbody2D rigidBody2D;
    Animator animator;
    Collider2D mycollider2D;
    Collider2D feetBoxcollider2D;
    Sword sword;

    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        mycollider2D = GetComponent<CapsuleCollider2D>();
        feetBoxcollider2D = GetComponent<BoxCollider2D>();
        gravityStart = rigidBody2D.gravityScale;
        sword = FindObjectOfType<Sword>();
        FindObjectOfType<AudioManager>().Play(AudioManager.Sound.PlayerSpawn);
    }
    // Update is called once per frame
    void Update()
    {
        // Do not let user controll a dead player
        if (!isAlive) {
            return; 
        }
        // Controll the player behavior with input
        Run();
        Jump();
        Clime();
        Attack();
        CheckHazards();
        
    }

    // The Player should die if he touches something in the hazard layer. 
    private void CheckHazards()
    {
        if (feetBoxcollider2D.IsTouchingLayers(LayerMask.GetMask("Hazard")) || mycollider2D.IsTouchingLayers(LayerMask.GetMask("Hazard")))
        {
            Die();
        }
    }

    // handel climeing a ladder
    private void Clime()
    {

        if (!feetBoxcollider2D.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            rigidBody2D.gravityScale = gravityStart;
            return;
        }
        rigidBody2D.gravityScale = 0;
        float inputVelocity = Input.GetAxis("Vertical");
        rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, inputVelocity * climeSpeed);

    }
    // Take input from the user to attack
    private void Attack()
    {

        if (Input.GetButtonDown("Fire1") && !isAttacking)
        {
            // Animations has the events for killing a player
            animator.SetBool("isAttacking", true);
            isAttacking = true;

        }
        
    }
    // Trigger the actual killing on an enemy, controlled by the animation 
    private void KillEnemy()
    {
        
        sword.Attack();

    }

    // Stop the attack 
    private void StopAttack()
    {
        isAttacking = false;
        animator.SetBool("isAttacking", false);
        
    }

    // Handel running
    private void Run()
    {
        float inputVelocity = Input.GetAxisRaw("Horizontal");
        rigidBody2D.velocity = new Vector2(inputVelocity * runSpeed, rigidBody2D.velocity.y);
        FlipSprite();
        bool playerisMovingHorzantily = Mathf.Abs(rigidBody2D.velocity.x) > Mathf.Epsilon;
        animator.SetBool("isRunning", playerisMovingHorzantily);
    }
    // Handel Jumping
    private void Jump()
    {
        bool doubleJumpOnce = false;
        bool isGrounded = feetBoxcollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if (isGrounded)
        {
            doubleJumpOnce = true;
            canDoubleJump = true;
            animator.SetBool("isJumping", false);
            //animator.SetBool("isDoubleJumping", false);
        }
        else if (!isGrounded)
        {
            if (doubleJumpOnce)
            {
                canDoubleJump = true;
                doubleJumpOnce = false;

            }
            animator.SetBool("isJumping", true);
        }
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {

                animator.SetBool("isJumping", true);
                Vector2 jumpVelocity = new Vector2(0f, jumpSpeed);
                rigidBody2D.velocity = jumpVelocity;
                canDoubleJump = true;
                //FindObjectOfType<AudioManger>().Play("Jump");
            }
            else if (canDoubleJump)
            {
                canDoubleJump = false;
                //animator.SetBool("isDoubleJumping", true);
                Vector2 jumpVelocity = new Vector2(0f, jumpSpeed);
                rigidBody2D.velocity = jumpVelocity;
                //FindObjectOfType<AudioManger>().Play("Jump");
            }


        }


    }
    // Make sure the player faces the right direction 
    private void FlipSprite()
    {
        bool playerisMovingHorzantily = Mathf.Abs(rigidBody2D.velocity.x) > Mathf.Epsilon;

        if (playerisMovingHorzantily)
        {
            transform.localScale = new Vector3(Mathf.Sign(rigidBody2D.velocity.x), 1, 1);
        }
    }
    // Kill the player if he touches an enemy
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>())
        {
            Die();
        }
    }

    // Method to controll behivor of the player dying.
    public void Die()
    {
        isAlive = false;
        animator.SetBool("isAttacking", false);
        animator.SetBool("isDead", true);

        rigidBody2D.velocity = deathJump;
        StartCoroutine(HandelDeath());
        FindObjectOfType<AudioManager>().Play(AudioManager.Sound.PlayerDeath);

    }
    // Handel the death of the player
    IEnumerator HandelDeath()
    {
        yield return new WaitForSeconds(2);
        FindObjectOfType<GameManger>().handelDeath();

    }
    // Handel the Win of the player
    public void PlayerDisappear()
    {
        StartCoroutine(HandelWin());
    }

    IEnumerator HandelWin()
    {
        //animator.SetBool("isDead", true);
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
