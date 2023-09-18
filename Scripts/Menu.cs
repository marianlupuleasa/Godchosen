using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Options()
    {
        SceneManager.LoadScene("Options");
    }

    public void Play()
    {
        SceneManager.LoadScene("PartSelection");
    }
        
    public void Quit()
    {
        Application.Quit();
    }
}
