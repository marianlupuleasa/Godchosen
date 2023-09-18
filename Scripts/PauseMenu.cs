using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public static bool isPaused = false;
    string sceneName;
    
    void Start()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
    }

    void Update()
    {
        sceneName = FindObjectOfType<GameSession>().sceneName;
        if(sceneName != "StartingScreen" && sceneName != "MainMenu" && sceneName != "Options" && sceneName != "PartSelection")
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if(isPaused)
                {
                    ExitPause();
                }
                else {
                    BeginPause();
                }
            }    
        }
        
    }

    public void BeginPause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ExitPause()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void MainMenuPause()
    {
        FindObjectOfType<GameSession>().MainMenuResetGameSession();
        ExitPause();
    }

    public void RestartOnPause()
    {
        FindObjectOfType<GameSession>().ReloadCurrentPart();
        ExitPause();
    }
        
    public void Quit()
    {
        Application.Quit();
    }
}
