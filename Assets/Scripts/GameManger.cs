using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManger : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI enemyText;
    [SerializeField] TextMeshProUGUI fruitText;
    [SerializeField] GameObject winCanvas;
    [SerializeField] GameObject gameOverCanvas;

    bool levelOver = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText();
    }

    // Keep track of how many enemies and fruits there are when they reach zero we win
    private void UpdateText()
    {
        int enemyCount = FindObjectsOfType<Enemy>().Length;
        int fruitCount = FindObjectsOfType<Fruit>().Length;
        enemyText.text = "Enemies left: " + enemyCount;
        fruitText.text = "Fruits left: " + fruitCount;
        if (enemyCount == 0 && fruitCount == 0)
        {
            if (!levelOver)
            {
                levelOver = true;
                handelWin();
            }
            
        }
    }
    // Incase player dies display the gameover canvas
    public void handelDeath()
    {
        if (!levelOver)
        {
            levelOver = true;
            gameOverCanvas.SetActive(true);
            // Pause the game to stop sounds from playing 
            PauseGame();
        }

    }
    // Incase the player win then play sound wait a bit then display canvas
    void handelWin()
    {
        StartCoroutine(Win());
    }
    IEnumerator Win()
    {
        FindObjectOfType<AudioManager>().Play(AudioManager.Sound.NextLevel);
        yield return new WaitForSeconds(2);
        winCanvas.SetActive(true);
        PauseGame();

    }
    void PauseGame()
    {
        Time.timeScale = 0;
    }
    void ResumeGame()
    {
        Time.timeScale = 1;
    }
}
