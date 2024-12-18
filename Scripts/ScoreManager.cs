using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    int currentScore;
    [SerializeField] int targetScore;

    Animator anim;
    [SerializeField] TextMeshProUGUI scoreText;

    [SerializeField] Animator doorUnlockedAnim;

    [HideInInspector] public bool targetScoreReached = false;

    Scene currentScene;

    void Awake()
    {
        currentScene = SceneManager.GetActiveScene();
    }

    void Start()
    {
        if (currentScene.name == "Level 1")
            targetScore = 5;
        else if (currentScene.name == "Level 2")
            targetScore = 8;
        else if (currentScene.name == "Level 3")
            targetScore = 12;

        scoreText.text = currentScore + " / " + targetScore.ToString();
        anim = GetComponent<Animator>();
    }

    public void IncrementScore() // Called by Player script when collecting pickups
    {
        currentScore++;
        scoreText.text = currentScore + " / " + targetScore.ToString();
        anim.SetTrigger("score_up");

        if (currentScore >= targetScore)
        {
            targetScoreReached = true;
            doorUnlockedAnim.SetTrigger("Unlocked");
        }
    }
}
