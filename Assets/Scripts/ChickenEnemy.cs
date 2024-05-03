using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenEnemy : MonoBehaviour
{
    public float moveSpeed = 0.9f;
    // Start is called before the first frame update
    public float jumpForce = 7f; // Jump force
    public bool offPlatform = false;
    private SpriteRenderer spriteRenderer;
    private float moveDirection = -1;
    public Transform player; // Reference to the player
    public float detectionXRange = 10f;
    public float detectionYRange = 0.5f;
    private bool isLerping = false;
    private Vector3 targetPosition;

    public enum eState : int
    {
        forward, //black
        offLedge, //green
        turnAround, //blue
        quickDash, //red
        kNumStates
    }

    public eState chickenState;

    private Color[] stateColors = new Color[(int)eState.kNumStates]
  {
        new Color(0, 0,   0),
        new Color(0,   255, 0),
        new Color(0,   0,   255),
        new Color(255,   0,   0),
  };
    float turnStartTime;
    float recoveryTime = 0.5f;
    private CircleCollider2D circleCollider;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        chickenState = eState.forward;
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private bool IsPlayerWithinRange()
    {
        float xDifference = Mathf.Abs(player.position.x - transform.position.x);
        float yDifference = Mathf.Abs(player.position.y - transform.position.y);

        return xDifference <= detectionXRange && yDifference <= detectionYRange;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPlayerWithinRange())
        {
            Debug.Log("Speeding up within range");
            moveSpeed = 2.25f;
        }
        else
        {
            moveSpeed = 1.5f;
        }
       // spriteRenderer.color = stateColors[(int)chickenState];
        switch (chickenState)
        {

            case eState.forward:
                //maybe an animation for forward idk lol
                moveForward();
                break;
            case eState.offLedge:
                float currentTime = Time.time;
                //pause for the revovery time
                if (currentTime >= turnStartTime + recoveryTime)
                {
                    chickenState = eState.turnAround;
                }
               
                break;
            case eState.turnAround:
                RotateSprite180();
                chickenState = eState.quickDash;
                break;
            case eState.quickDash:
                transform.position = transform.position + new Vector3(moveDirection * 0.25f, 0f, 0f);
                chickenState = eState.forward;
                break;
        }

        //update the state to turn the chicken around if we are about to fall off
        if (offPlatform && chickenState != eState.offLedge && chickenState != eState.turnAround && chickenState !=eState.quickDash)
        {
            chickenState = eState.offLedge;
            //change the direction
            moveDirection *= -1;
            Debug.Log("Off platform");
            offPlatform = false;

        }
    }

    public void moveForward()
    {
        float movement = moveDirection * moveSpeed * Time.deltaTime;
        transform.position = transform.position + new Vector3(movement, 0f, 0f);
    }
    public void RotateSprite180()
    {
        Vector3 currentRotation = transform.localEulerAngles;
        currentRotation.y += 180f;
        transform.localEulerAngles = currentRotation;
        turnStartTime = Time.time;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //when the chicken falls off the ledge we need to say to inform state machine
        //need to hit the player if we are on it, ensure sword also have player TAG!!
        if (!collision.CompareTag("Player"))
        {
            Debug.Log("Player exiting");
            offPlatform = true;
        }
    }


}
