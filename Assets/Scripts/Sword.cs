using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


// Sowrd can only collide with enemy
public class Sword : MonoBehaviour
{
    GameObject gameObject;

    // Attack only if the sword is colliding with an enemy
    public void Attack()
    {
        PlaySounds();
        if (gameObject != null && gameObject.GetComponent<Enemy>())
        {
            gameObject.GetComponent<Enemy>().killEnemy();
            
        }

    }
    // save the current collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            gameObject = collision.gameObject;
        }
    }
    // Remove the collision 
    private void OnTriggerExit2D(Collider2D collision)
    {
        gameObject = null;
    }
    // Play attach sound
    void PlaySounds()
    {
        // Play different sounds depending on whether the sword touches an enemy or not
        if (!gameObject)
        {
            FindObjectOfType<AudioManager>().Play(AudioManager.Sound.PlayerAttack);
        }
        else
        {
            FindObjectOfType<AudioManager>().Play(AudioManager.Sound.PlayerAttackEnemy);
        }
    }

}
