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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        int enemyCount = FindObjectsOfType<Enemy>().Length;
        int fruitCount = FindObjectsOfType<Fruit>().Length;
        enemyText.text = "Enemies left: " + enemyCount;
        fruitText.text = "Fruits left: " + fruitCount;
        if (enemyCount == 0 && fruitCount == 0)
        {
            handelWin();
        }
    }
    public void handelDeath()
    {
        gameOverCanvas.SetActive(true);

    }
    void handelWin()
    {
        StartCoroutine(Win());
    }
    IEnumerator Win()
    {
        yield return new WaitForSeconds(2);
        winCanvas.SetActive(true);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);


    }
}
