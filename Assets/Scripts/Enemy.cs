using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Get vaule from the editor 
    [SerializeField] int hitsToKill = 1;
    AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>().GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // logic for killing the enemy take hits until point reach 0
    public void killEnemy()
    {
        hitsToKill--;
        if (hitsToKill == 0)
        {
            Die();
        }

    }
    // Logic for dying play sound then destory game object
    private void Die()
    {
        if (GetComponent<ChickenEnemy>())
        {
            audioManager.Play(AudioManager.Sound.ChickenDeath);
        }
        else if(GetComponent<PlantEnemy>())
        {
            audioManager.Play(AudioManager.Sound.PeaShooterDeath);

        }
        else if(GetComponent<BlueBird>())
        {
            audioManager.Play(AudioManager.Sound.BirdDeath);
        }
        else if (GetComponent<RhinoEnemy>())
        {
            audioManager.Play(AudioManager.Sound.RhinoDeath);
        }

        Destroy(this.gameObject);
    }
}
