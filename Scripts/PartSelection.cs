using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PartSelection : MonoBehaviour
{
    public void Part1()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void Part2()
    {
        SceneManager.LoadScene("Level 4");
    }

    public void Part3()
    {
        SceneManager.LoadScene("Level 5");
    }
        
    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
