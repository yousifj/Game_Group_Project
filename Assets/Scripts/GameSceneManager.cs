using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    // Load main menu
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    // Go to the next level
    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
    // Exit the game
    public void Quit()
    {
        Application.Quit();
    }
    // Reload Level
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
