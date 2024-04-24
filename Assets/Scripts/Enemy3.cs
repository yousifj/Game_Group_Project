using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : MonoBehaviour

{
    //This enum represents all of the states for the enemy 
    public enum EnemyThreeState
    {
        Idle,
        Attack
    }

    //This variable keeps track of the enemy's current state 
    public EnemyThreeState enemyThreeCurrentState;

    //This variable is a reference to the player game object 
    private GameObject player;

    //This keep track of how close the player is to the enemy 
    private float distanceToPlayer;

    //This variable determines how close the player can get before the enemy switches to the attack state
    public float attackRange = 2.15f;

    //Tracks the time to regulate the firing rate of bullets
    private float timer;

    //Specifies the duration to wait between consecutive bullet shots
    private float timeBetweenShots = 2f;

    //This represents the starting position of the bullet
    public Transform bulletStartPosition;

    //This is a reference to the bullet prefab
    public GameObject bulletPreFab;

    void Start()
    {
        //Set the enemy's initial state to idle
        enemyThreeCurrentState = EnemyThreeState.Idle;

        //Find the player object by name
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {   
        switch (enemyThreeCurrentState)
        {
            case EnemyThreeState.Idle:
                //Get the players distance from the enemy
                distanceToPlayer = CheckPlayersDistanceFromyEnemy();
                
                //Check if the player is within the attack range
                if (distanceToPlayer < attackRange)
                {
                    //Move to the Attack state
                    enemyThreeCurrentState = EnemyThreeState.Attack;
                }
                
                break;

            case EnemyThreeState.Attack:

                //Increment the timer 
                timer += Time.deltaTime;
                
                //Checks if enough time has passed so the enemy can fire another bullet
                if (timer > timeBetweenShots)
                {
                    //Reset the timer
                    timer = 0;

                    //Spawn a bullet prefab and define the start position and rotation for the bullet
                    Instantiate(bulletPreFab, bulletStartPosition.position, bulletStartPosition.rotation);
                }

                //Get the players distance from the enemy after a bullet is fired
                distanceToPlayer = CheckPlayersDistanceFromyEnemy();

                //Check if the player is out of the attack range
                if (distanceToPlayer > attackRange)
                {
                    //Move to the Idle state
                    enemyThreeCurrentState = EnemyThreeState.Idle;
                }

                break;
        }
    }

    private float CheckPlayersDistanceFromyEnemy()
    {
        //Calculate the distance between the enemy and the player
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        
        return distanceToPlayer;
    }
}
