using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenEnemy : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    // Start is called before the first frame update
    public float jumpForce = 7f; // Jump force
    public bool offPlatform = false;
    private SpriteRenderer spriteRenderer;
    private float moveDirection = -1;
    public Transform player; // Reference to the player
    public float detectionXRange = 10f;
    public float detectionYRange = 0.5f;

    public enum eState : int
    {
        forward, //clear
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
    float recoveryTime = 5.0f;
    private CircleCollider2D circleCollider;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        chickenState = eState.forward;
        spriteRenderer.color = stateColors[(int)chickenState];
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
            moveSpeed = 2.25f;
        }
        else
        {
            moveSpeed = 1.5f;
        }
        spriteRenderer.color = stateColors[(int)chickenState];
        switch (chickenState)
        {

            case eState.forward:
                //maybe an animation for forward idk lol
                moveForward();
                break;
            case eState.offLedge:
                RotateSprite180();
                float currentTime = Time.time;
                if (currentTime >= turnStartTime + recoveryTime)
                {
                    chickenState = eState.turnAround;
                }
                chickenState = eState.forward;
                //make him wait for a little bit of time and then dash forward before 
                //turn him around and then 
                break;
            case eState.quickDash:

                moveForward();
                break;
        }

        //update the state to turn the chicken around if we are about to fall off
        if (offPlatform && chickenState != eState.offLedge)
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
        offPlatform = true;
    }



    /*public void UpdateChickenPosition()
    {
        // Get the mouse position in world coordinates
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Lock the y and z coordinates
        mousePosition.y = transform.position.y;
        mousePosition.z = transform.position.z;

        // Calculate the direction to move based on the mouse position
        Vector3 moveDirection = mousePosition - transform.position;

        // Check if the player is not on top of the mouse
        if (moveDirection.magnitude > 0.1f) // Adjust this threshold as needed
        {
            moveDirection.Normalize(); // Normalize the direction vector to have a magnitude of 1

            // Move the player towards the mouse horizontally
            transform.position += new Vector3(moveDirection.x, 0f, 0f) * moveSpeed * Time.deltaTime;
        }
    }
    private void HandleJump()
    {
        // Check for left mouse button click
        if (Input.GetMouseButtonDown(1))
        {
            // Apply jump force to the player
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }
    } */
}
