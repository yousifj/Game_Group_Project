using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBirdMover : MonoBehaviour
{
    // Second point to go twords
    public Transform secondPoint; 
    public float speed = 1.0f;   
    public SpriteRenderer spriteRenderer;

    private Vector3 initialPosition;
    private bool movingToEnd = true;

    void Start()
    {
        // First point in the current positon 
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the target position
        Vector3 targetPosition = movingToEnd ? secondPoint.position : initialPosition;

        // Move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Destory this object if there is no bird to controll
        if (!spriteRenderer)
        {
            Destroy(gameObject);
        }
        else
        {
            // Flip the sprite based on direction
            if (transform.position.x > targetPosition.x)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }
        }

        // Check if the object has reached the target position
        if (transform.position == targetPosition)
        {
            // Toggle the direction
            movingToEnd = !movingToEnd;
        }
       
    }
}
