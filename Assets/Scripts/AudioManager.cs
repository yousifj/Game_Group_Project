using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum Sound
    {
        BackgroundMusic,
        GameOver,
        NextLevel,
        PlayerWalk,
        PlayerAttack,
        PlayerDeath,
        EnemyWalk,
        EnemyDeath,
        EnemyDamage
    }

    //select sounds in unity editor
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private AudioClip nextLevelSound;
    [SerializeField] private AudioClip playerWalkSound;
    [SerializeField] private AudioClip playerAttackSound;
    [SerializeField] private AudioClip playerDeathSound;
    [SerializeField] private AudioClip enemyWalkSound;
    [SerializeField] private AudioClip enemyDeathSound;
    [SerializeField] private AudioClip enemyDamageSound;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    //method to play sounds based on enum
    public void Play(Sound sound)
    {
        switch (sound)
        {
            case Sound.BackgroundMusic:
                audioSource.PlayOneShot(backgroundMusic);
                break;
            case Sound.GameOver:
                audioSource.PlayOneShot(gameOverSound);
                break;
            case Sound.NextLevel:
                audioSource.PlayOneShot(nextLevelSound);
                break;
            case Sound.PlayerWalk:
                audioSource.PlayOneShot(playerWalkSound);
                break;
            case Sound.PlayerAttack:
                audioSource.PlayOneShot(playerAttackSound);
                break;
            case Sound.PlayerDeath:
                audioSource.PlayOneShot(playerDeathSound);
                break;
            case Sound.EnemyWalk:
                audioSource.PlayOneShot(enemyWalkSound);
                break;
            case Sound.EnemyDeath:
                audioSource.PlayOneShot(enemyDeathSound);
                break;
            case Sound.EnemyDamage:
                if (enemyDamageSound != null) // Optional
                    audioSource.PlayOneShot(enemyDamageSound);
                break;
            default:
                break;
        }
    }
}
