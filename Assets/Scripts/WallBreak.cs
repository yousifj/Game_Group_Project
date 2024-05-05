using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
public class WallBreak : MonoBehaviour
{
    public GameObject enemy; // Assign the enemy object in the Unity inspector

    void Update()
    {
        if (enemy == null) // Check if the enemy has been destroyed
        {
            Destroy(gameObject); // Destroy this dependent object
        }
    }
}