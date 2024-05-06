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


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCollected)
        {
            return;
        }
        isCollected = true;
        FindAnyObjectByType<AudioManager>().Play(AudioManager.Sound.PickUp);
        animator.SetBool("isCollected", true);
    }
    public void Collected()
    {
       
        Destroy(this.gameObject);
    }
}
