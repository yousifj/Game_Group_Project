/*
 * This script handles collisions involving bullets for this enemy in the following way:
 * 
 * If the player is attacking with the sword and a bullet hits them, the player can destroy the bullet.
 * If the player is not attacking and a bullet hits them, the player dies.
 * If a bullet hits an object that's not the player (e.g., a wall), the bullet gets destroyed.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    //This variable is a reference to the player game object 
    private GameObject player;

    //This variable is a reference to the Rigidbody2D component attached to the bullet
    private Rigidbody2D bulletRigidbody;

    //This represents the player's direction
    private Vector2 playerDirection;

    //This represents the speed for each bullet
    public float bulletSpeed = 6.0f;
    void Start()
    {
        //Find the player object by name
        player = GameObject.Find("Player");

        //Get the rigid body 2D component of the bullet
        bulletRigidbody = GetComponent<Rigidbody2D>();

        //Calculate the player's direction
        playerDirection = (player.transform.position - transform.position).normalized;

        //Set the velocity of the bullet to move in the calculated direction
        bulletRigidbody.velocity = new Vector2(playerDirection.x, playerDirection.y).normalized * bulletSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Check if the bullet collided with the player 
        if (collision.gameObject == player)
        {
            // Check if the player is attacking
            if (Input.GetButton("Fire1"))
            {
                // Player is swinging the sword, destroy the bullet
                Destroy(gameObject);
            }
            else
            {
                //Player is not swinging the sword and got hit with a bullet player dies call the Die function from the playerControllerScript
                collision.gameObject.GetComponent<PlayerController>().Die();
            }
        }

        //This check if bullets hit anything other than the player like walls etc 
        else if (collision.gameObject != player)
        {

        }
    }
}
