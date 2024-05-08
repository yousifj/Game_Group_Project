using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    Animator animator;
    bool isCollected = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Collect the fruit when a player touches the collider of the fruit
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            if (isCollected)
            {
                return;
            }
            // play animation and sound
            isCollected = true;
            FindAnyObjectByType<AudioManager>().Play(AudioManager.Sound.PickUp);
            animator.SetBool("isCollected", true);
        }
    }
    public void Collected()
    {
       
        Destroy(this.gameObject);
    }
}
