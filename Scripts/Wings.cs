using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Wings : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {

            FindObjectOfType<ScenePersist>().ResetScenePersist();
            SceneManager.LoadScene("Level 4");
        }
    }
}
