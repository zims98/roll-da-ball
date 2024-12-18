using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameMaster : MonoBehaviour
{
    [SerializeField] GameObject gameOverObject;
    [SerializeField] LevelLoader loaderScript;
    [SerializeField] GameObject victoryObject;
    bool gameIsOver = false;
    float windowPopupDelay = 2f;
    //[SerializeField] Animator anim;

    [SerializeField] TextMeshProUGUI finalTimeText;
    [SerializeField] TextMeshProUGUI endTimeText;
    [SerializeField] TextMeshProUGUI currentTimeText;
    [SerializeField] TextMeshProUGUI currentLevelText;

    [SerializeField] GameObject restartButton;
    [SerializeField] GameObject exitButton;

    public static float time; // Current time in scene
    public static float storedTime; // Stored time when changing scenes
    float finalTime; // Time when game is over

    Scene currentScene;
    int levels;

    void Start()
    {      
        currentScene = SceneManager.GetActiveScene();
        
        if (currentScene.name == "Level 1")
        {
            storedTime = 0;
            levels = 1;
        }           
        else if (currentScene.name == "Level 2")
            levels = 2;
        else if (currentScene.name == "Level 3")
            levels = 3;

        currentLevelText.text = "LEVEL: " + levels + " / " + 3;
    }

    void Update()
    {
        time = Time.timeSinceLevelLoad + storedTime;

        currentTimeText.text = "TIME: " + time.ToString("0.0") + "s";

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if (Input.GetKeyDown(KeyCode.R))
            loaderScript.FadeToLevel(0);
    }

    public void GameOver()
    {
        if (!gameIsOver)
        {
            gameIsOver = true;

            finalTime = time;

            StartCoroutine(WindowDelay());

            currentLevelText.enabled = false;
            currentTimeText.enabled = false;
            restartButton.SetActive(false);
            exitButton.SetActive(false);

        }
    }

    public void Victory()
    {
        currentLevelText.enabled = false;
        currentTimeText.enabled = false;
        restartButton.SetActive(false);
        exitButton.SetActive(false);

        finalTime = time;
        victoryObject.SetActive(true);
        endTimeText.text = "TIME: " + finalTime.ToString("0.0") + "s";
    }

    IEnumerator WindowDelay()
    {
        yield return new WaitForSeconds(windowPopupDelay);

        gameOverObject.SetActive(true);

        finalTimeText.text = "TIME: " + finalTime.ToString("0.0") + "s";
    }

    public void RetryGame()
    {
        loaderScript.FadeToLevel(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
