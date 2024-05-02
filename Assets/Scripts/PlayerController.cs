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
        mycollider2D = GetComponent<Collider2D>();
        feetBoxcollider2D = GetComponent<BoxCollider2D>();
        gravityStart = rigidBody2D.gravityScale;
        sword = FindObjectOfType<Sword>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!isAlive) {
            return; 
        }
        Run();
        Jump();
        Clime();
        Attack();
        CheckHazards();
        
    }

    private void CheckHazards()
    {
        if (feetBoxcollider2D.IsTouchingLayers(LayerMask.GetMask("Hazard")))
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
    // take input from the user
    private void Attack()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetBool("isAttacking", true);
            
        }
        
    }
    // trigger the actual killing on an enemy
    private void KillEnemy()
    {
        
        sword.Attack();

    }

    //stop the attack 
    private void StopAttack()
    {
        animator.SetBool("isAttacking", false);
    }

    // handel running
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
    // make sure the player faces the right direction 
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

    //Yousif I changed this to public because I needed to use it when enemy's bullets kill the player -Mario 
    public void Die()
    {
        isAlive = false;
        //animator.SetBool("isDoubleJumping", false);
        animator.SetBool("isDead", true);

        rigidBody2D.velocity = deathJump;
        StartCoroutine(HandelDeath());
        //FindObjectOfType<AudioManger>().Play("Death");

    }
    IEnumerator HandelDeath()
    {
        yield return new WaitForSeconds(2);
        FindObjectOfType<GameManger>().handelDeath();
        //reset level 
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);


    }
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
