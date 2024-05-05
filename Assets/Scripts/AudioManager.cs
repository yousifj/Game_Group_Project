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
        PlayerAttack,
        PlayerDeath,
        ChickenAttack,
        ChickenDeath,
        BirdAttack,
        BirdDeath,
        PeaShooterAttack,
        PeaShooterDeath,
        RhinoAttack,
        RhinoDamage,
        RhinoDeath
    }

    //select sounds in unity editor
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private AudioClip nextLevelSound;
    [SerializeField] private AudioClip playerAttackSound;
    [SerializeField] private AudioClip playerDeathSound;
    [SerializeField] private AudioClip chickenAttackSound;
    [SerializeField] private AudioClip chickenDeathSound;
    [SerializeField] private AudioClip birdAttackSound;
    [SerializeField] private AudioClip birdDeathSound;
    [SerializeField] private AudioClip peaShooterAttackSound;
    [SerializeField] private AudioClip peaShooterDeathSound;
    [SerializeField] private AudioClip rhinoAttackSound;
    [SerializeField] private AudioClip rhinoDamageSound;
    [SerializeField] private AudioClip rhinoDeathSound;

    private AudioSource audioSource;

    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }
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
            case Sound.PlayerAttack:
                audioSource.PlayOneShot(playerAttackSound);
                break;
            case Sound.PlayerDeath:
                audioSource.PlayOneShot(playerDeathSound);
                break;
            case Sound.ChickenAttack:
                audioSource.PlayOneShot(chickenAttackSound);
                break;
            case Sound.ChickenDeath:
                audioSource.PlayOneShot(chickenDeathSound);
                break;
            case Sound.BirdAttack:
                audioSource.PlayOneShot(birdAttackSound);
                break;
            case Sound.BirdDeath:
                audioSource.PlayOneShot(birdDeathSound);
                break;
            case Sound.PeaShooterAttack:
                audioSource.PlayOneShot(peaShooterAttackSound);
                break;
            case Sound.PeaShooterDeath:
                audioSource.PlayOneShot(peaShooterDeathSound);
                break;
            case Sound.RhinoAttack:
                audioSource.PlayOneShot(rhinoAttackSound);
                break;
            case Sound.RhinoDamage:
                audioSource.PlayOneShot(rhinoDamageSound);
                break;
            case Sound.RhinoDeath:
                audioSource.PlayOneShot(rhinoDeathSound);
                break;
            default:
                break;
        }
    }
}
