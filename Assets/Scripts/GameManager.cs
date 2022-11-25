using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// This class handles basis game loop and ui stuff.
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Fields
    [Header("Score related:")]
    [SerializeField] TextMeshProUGUI scoreText;
    private int playerScore;

    [Header("Round time related:")]
    [SerializeField] TextMeshProUGUI roundTimeText;
    [SerializeField] float roundTime;
    float roundTimer;
    [SerializeField] GameObject winScreen;

    [Header("Staring related:")]
    [SerializeField] GameObject startButton;
    bool roundStarted;
    #endregion

    #region Singleton
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        // Initialize the score text.
        ChangeScoreValue(0);
    }

    // Update is called once per frame
    void Update()
    {

        if (roundStarted)
        {
            // Update the round timer and round timer text..
            roundTimer += Time.deltaTime;
            roundTimeText.text = "Time: " + (int)roundTimer + "/" + roundTime; 
        }
    }

    public void ChangeScoreValue(int amount)
    {
        playerScore += amount;
        scoreText.text = "Score: " + playerScore;
    }

    private void PlayWinMessage()
    {
        Time.timeScale = 0f;
        winScreen.SetActive(true);
    }

    /// <summary>
    /// Method for the "Reloading play scene" button.
    /// </summary>
    public void ReplayGameScene()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Method for the "Start Round" button.
    /// </summary>
    public void StartRound()
    {
        roundStarted = true;
        Time.timeScale = 1f;
        startButton.SetActive(false);
    }
}
