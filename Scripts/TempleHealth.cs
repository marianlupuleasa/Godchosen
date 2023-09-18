using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TempleHealth : MonoBehaviour
{
    public int healthAmount = 100;

    public void Damage(int damage)
    {
        healthAmount -= damage;
        FindObjectOfType<GameSession>().templeHealth.fillAmount = healthAmount / 100f;
        if(healthAmount <= 0)
        {
            FindObjectOfType<GameSession>().BeginLose();
        }
    }

    public void Heal(int heal)
    {
        healthAmount += heal;
        FindObjectOfType<GameSession>().templeHealth.fillAmount = healthAmount / 100f;
    }
}
