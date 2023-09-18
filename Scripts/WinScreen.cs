using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    public GameObject winScreen;
    int healthLeft;

    void Start() {

        winScreen.SetActive(false);

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            healthLeft = FindObjectOfType<PlayerHealth>().healthAmount;
            FindObjectOfType<GameSession>().IncreaseScore(20 * healthLeft);            
            FindObjectOfType<FollowCamera>().canMove = false;
            FindObjectOfType<GameSession>().playerBar.SetActive(false);
            BeginWin();

        }
    }

    public void BeginWin()
    {
        winScreen.SetActive(true);
        Time.timeScale = 0f;
        PauseMenu.isPaused = true;
    }

    public void ExitWin()
    {
        winScreen.SetActive(false);
        Time.timeScale = 1f;
        PauseMenu.isPaused = false;
    }

    public void MainMenuWin()
    {
        SceneManager.LoadScene("MainMenu");
        ExitWin();
    }

    public void RestartOnWin()
    {
        FindObjectOfType<GameSession>().ResetGameSession();
        ExitWin();
    }

}
