using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthBar : MonoBehaviour
{
    public int health;
    public int numberOfHearts;

    public Image[] hearts;
    public Sprite fullHealth;
    public Sprite noHealth;

    void Update()
    {
        if (health > numberOfHearts)
        {
            health = numberOfHearts;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if(i < health)
            {
                hearts[i].sprite = fullHealth;
            }
            else
            {
                hearts[i].sprite = noHealth;
            }

            if (i < numberOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }

        // If player is at no health, then the player dies and triggers a failed screen for level 1
        if (health == 0 && SceneManager.GetActiveScene().name == "Scene1")
        {
            SceneManager.LoadScene("FailedLevel1");
        }

        // If player is at no health, then the player dies and triggers a failed screen for level 2
        if (health == 0 && SceneManager.GetActiveScene().name == "Scene2")
        {
            SceneManager.LoadScene("FailedLevel2");
        }
    }

    /*public Slider slider;

    public Gradient healthGradient;
    public Image fill;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = healthGradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        slider.value = health;

        fill.color = healthGradient.Evaluate(slider.normalizedValue);
    } */
}
