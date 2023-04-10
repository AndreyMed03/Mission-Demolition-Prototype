using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
    public GameObject menu;
    public GameObject menuteamdevelopers;
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void TeamDevelopers()
    {
        menu.SetActive(false);
        menuteamdevelopers.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Back()
    {
        menu.SetActive(true);
        menuteamdevelopers.SetActive(false);
    }
}
