using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantEnemy : MonoBehaviour

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
    public float attackRange = 8.6f;

    //Tracks the time to regulate the firing rate of bullets
    private float timer;

    //Specifies the duration to wait between consecutive bullet shots
    private float timeBetweenShots = 2f;

    //This represents the starting position of the bullet
    public Transform bulletStartPosition;

    //This is a reference to the bullet prefab
    public GameObject bulletPreFab;

    private Animator animator;

    void Start()
    {
        //Set the enemy's initial state to idle
        enemyThreeCurrentState = EnemyThreeState.Idle;

        animator = GetComponent<Animator>();

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

                //Check if the player is within the attack range and the player is in front of the enemy 
                if (distanceToPlayer < attackRange && IsPlayerInFrontOfEnemy())
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
                    animator.SetBool("isAttacking", true);
                }

                //Get the players distance from the enemy after a bullet is fired
                distanceToPlayer = CheckPlayersDistanceFromyEnemy();

                //Check if the player is out of the attack range or if the player is not front of the enemy 
                if (distanceToPlayer > attackRange || IsPlayerInFrontOfEnemy() == false)
                {
                    
                    //Move to the Idle state
                    enemyThreeCurrentState = EnemyThreeState.Idle;
                }

                break;
        }
    }

    private void ShotBullet()
    {
        //Spawn a bullet prefab and define the start position and rotation for the bullet
        Instantiate(bulletPreFab, bulletStartPosition.position, bulletStartPosition.rotation);
        animator.SetBool("isAttacking", false);

    }

    private float CheckPlayersDistanceFromyEnemy()
    {
        //Calculate the distance between the enemy and the player
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        
        return distanceToPlayer;
    }

    private bool IsPlayerInFrontOfEnemy()
    {
        //Get the direction vector pointing from the enemy to the player
        Vector3 directionToPlayer = (player.transform.position - transform.position);

        //Calculate the angle between the enemy's right direction and the direction to the player
        float angle = Vector3.Angle(transform.right, directionToPlayer);

        //Checks if the player is in front of the enemy
        if(Mathf.Abs(angle) > 90 && Mathf.Abs(angle) < 270)
        {
            return true;
        }

        return false;
    }
}
