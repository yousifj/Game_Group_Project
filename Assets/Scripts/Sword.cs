using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sword : MonoBehaviour
{
    GameObject gameObject;

    public void Attack()
    {
        if (gameObject != null && gameObject.GetComponent<Enemy>())
        {
            gameObject.GetComponent<Enemy>().killEnemy();
            
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            gameObject = collision.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        gameObject = null;
    }

}
