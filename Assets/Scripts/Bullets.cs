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

    //This represents the speed for each bullet
    public float bulletSpeed = 6.0f;

    //Time in seconds before the bullet is destroyed if it hasn't collided with anything 
    public float bulletLifetime = 5.0f;

    //Timer to track the bullet's lifespan
    private float lifespanTimer;

    //This represents the audio manager game object 
    AudioManager audioManager;

    void Start()
    {
        //Find the player object by name
        player = GameObject.Find("Player");

        //Get the rigid body 2D component of the bullet
        bulletRigidbody = GetComponent<Rigidbody2D>();

        //This ensures that the bullet's rotation gets set to 0 so the bullet shoots straight 
        transform.rotation = Quaternion.identity;

        //Set the velocity of the bullet to move straight
        bulletRigidbody.velocity = -transform.right * bulletSpeed;

        //Find the audio manager game object
        audioManager = FindAnyObjectByType<AudioManager>().GetComponent<AudioManager>();

        //Play the peashooter attack sound
        audioManager.Play(AudioManager.Sound.PeaShooterAttack);
    }

    void Update()
    {
        //Update the lifespan timer
        lifespanTimer += Time.deltaTime;

        //Check if the bullet's lifespan exceeds the specified lifetime basically destroys the bullet if it goes offscreen after five seconds 
        if (lifespanTimer >= bulletLifetime)
        {
            //Destroy the bullet
            Destroy(gameObject);
        }
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
            Destroy(gameObject);
        }
    }
}
