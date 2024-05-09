using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenEnemy : MonoBehaviour
{
    
    private float moveSpeed;
    private float moveDirection = -1;
    private Transform player;
    Animator animationController;
    private bool offPlatform = false;
    private float turnStartTime;
    private float recoveryTime = 0.5f;
    // Tunable variables to allow the user to adjust these at runtime
    [SerializeField] float detectionXRange = 10f;
    [SerializeField] float detectionYRange = 0.5f;
    [SerializeField] float slowMoveSpeed = 0.9f;
    [SerializeField] float moveSpeedFast = 2.25f;

    //State machine states for the chicken
    public enum eState : int
    {
        forward,
        offLedge, 
        turnAround, 
        quickDash, 
        kNumStates
    }

    public eState chickenState;
    
    void Start()
    {
        animationController = GetComponent<Animator>();
        chickenState = eState.forward;
        player = FindAnyObjectByType<PlayerController>().transform;

    }

    // Detects if the chicken is within a specified x and y range by the player
    private bool IsPlayerWithinRange()
    {
        float xDifference = Mathf.Abs(player.position.x - transform.position.x);
        float yDifference = Mathf.Abs(player.position.y - transform.position.y);

        return xDifference <= detectionXRange && yDifference <= detectionYRange;
    }

    void Update()
    {
        //Update the chicken to speed up if we detect the player nearby
        if (IsPlayerWithinRange())
        {
            //Render the chicken sound only when we change states from slow speed to fast speed
            if (moveSpeed != moveSpeedFast)
            {
                FindAnyObjectByType<AudioManager>().Play(AudioManager.Sound.ChickenAttack);
            }
            //Update animation and move speeds based on the 
            animationController.speed = 2.0f;
            moveSpeed = moveSpeedFast;

        }
        else
        {
            animationController.speed = 1.0f;
            moveSpeed = slowMoveSpeed;
        }
       
        //State Machine logic
        switch (chickenState)
        {
            case eState.forward:
                moveForward();
                break;
            //When the chicken is almost off the ledge pauses as part of a timeout 
            case eState.offLedge:
                float currentTime = Time.time;
                
                if (currentTime >= turnStartTime + recoveryTime)
                {
                    chickenState = eState.turnAround;
                }
               
                break;
            //State changes from turning around to quickly dashing
            case eState.turnAround:
                RotateSprite180();
                chickenState = eState.quickDash;
                break;
            //quickly translate the chicken towards the position it is facing by a small amount
            case eState.quickDash:
                transform.position = transform.position + new Vector3(moveDirection * 0.25f, 0f, 0f);
                chickenState = eState.forward;
                break;
        }

        //Change state to off ledge if we are offplatform and moving forward
        if (offPlatform && chickenState != eState.offLedge && chickenState != eState.turnAround && chickenState != eState.quickDash)
        {
            chickenState = eState.offLedge;
            moveDirection *= -1;
            offPlatform = false;

        }
    }

    //Translate the position over time
    public void moveForward()
    {
        float movement = moveDirection * moveSpeed * Time.deltaTime;
        transform.position = transform.position + new Vector3(movement, 0f, 0f);
    }

    //Rotates the Chicken in other direction so it does not fall of ledge
    public void RotateSprite180()
    {
        Vector3 currentRotation = transform.localEulerAngles;
        currentRotation.y += 180f;
        transform.localEulerAngles = currentRotation;
        turnStartTime = Time.time;
    }

    //when the chicken is about to fall off the ledge we need to say to inform state machine to turn around
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            offPlatform = true;
        }
    }


}
