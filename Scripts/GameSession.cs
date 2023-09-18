using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives;
    [SerializeField] int playerScore;
    [SerializeField] int coinScore;
    [SerializeField] int enemyScore;
    [SerializeField] public TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] float deathTimer;
    [SerializeField] public GameObject loseScreen;
    [SerializeField] public Image livesImage;
    [SerializeField] public GameObject templeBar;
    [SerializeField] public GameObject bossBar;
    [SerializeField] public GameObject playerBar;
    [SerializeField] public Image templeHealth;
    [SerializeField] public Image bossHealth;
    [SerializeField] public Image playerHealth;
    [SerializeField] AudioClip playerDeathSFX;
    [SerializeField] AudioClip playerHurtSFX;

    int score;
    public string sceneName;
    public int gamePart = 0;

    void Awake() 
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;

        if(numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        livesText.text = playerLives.ToString();
        scoreText.text = playerScore.ToString();
        loseScreen.SetActive(false);
        bossBar.SetActive(false);
        templeBar.SetActive(false);
        playerBar.SetActive(false);
    }

    void Update() {

        Scene currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;

        if(sceneName == "Level 1" || sceneName == "Level 2" || sceneName == "Level 3")
        {
            gamePart = 1;
        }
        else if(sceneName == "Level 4")
        {
            gamePart = 2;
            templeBar.SetActive(true);
            livesText.enabled = false;
            livesImage.enabled = false;
        }
        else if(sceneName == "Level 5")
        {
            gamePart = 3;
            playerBar.SetActive(true);
            templeBar.SetActive(false);
            bossBar.SetActive(false);
            livesText.enabled = false;
            livesImage.enabled = false;
        }
    }

    public void ProcessPlayerDeathOnTrap()
    {
        if(playerLives > 1)
        {
            Restart();
        }
        else
        {
            BeginLose();
        }
    }

    public void ProcessPlayerDeathOnEnemy()
    {
        if(playerLives > 1)
        {
            LoseLife();
        }
        else
        {
            BeginLose();
        }
    }

    public void LoseLife()
    {
        AudioSource.PlayClipAtPoint(playerHurtSFX, Camera.main.transform.position);
        playerLives--;
        livesText.text = playerLives.ToString();
    }

    public void Restart()
    {
        AudioSource.PlayClipAtPoint(playerDeathSFX, Camera.main.transform.position);
        playerLives--;
        Invoke("ReloadCurrentScene", deathTimer);

        livesText.text = playerLives.ToString();
    }

    public void ResetGameSession()
    {
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene("Level 1");
        Destroy(gameObject);
    }

    public void MainMenuResetGameSession()
    {
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene("MainMenu");
        Destroy(gameObject);
    }

    public void ReloadCurrentPart()
    {
        FindObjectOfType<ScenePersist>().ResetScenePersist();

        if(gamePart == 1)
        {
            SceneManager.LoadScene("Level 1");
        }
        else if(gamePart == 2)
        {
            SceneManager.LoadScene("Level 4");
        }
        else if(gamePart == 3)
        {
            SceneManager.LoadScene("Level 5");
        }
        
        Destroy(gameObject);
    }

    public void IncreaseScore(int value)
    {
        playerScore += value;
        scoreText.text = playerScore.ToString();
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        FindObjectOfType<Player>().isPlayerAlive = true;
    }

    public void BeginLose()
    {
        playerLives = 0;
        livesText.text = playerLives.ToString();
        loseScreen.SetActive(true);
        Time.timeScale = 0f;
        PauseMenu.isPaused = true;
    }

    public void ExitLose()
    {
        loseScreen.SetActive(false);
        Time.timeScale = 1f;
        PauseMenu.isPaused = false;
    }

    public void MainMenuLose()
    {
        SceneManager.LoadScene("MainMenu");
        ExitLose();
    }

    public void RestartOnLose()
    {
        FindObjectOfType<GameSession>().ReloadCurrentPart();
        ExitLose();
    }

    public void HealPlayer()
    {
        playerLives++;
        livesText.text = playerLives.ToString();
    }

}
