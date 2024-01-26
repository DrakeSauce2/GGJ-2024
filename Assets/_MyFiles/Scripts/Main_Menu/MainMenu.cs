using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainGui;
    [SerializeField] private GameObject optionsGui;

    public void PlayButton()
    {
        SceneManager.LoadScene("Level_1");
    }

    public void OptionsButton()
    {
        optionsGui.SetActive(true);
        mainGui.SetActive(false);
    }

    public void QuitButton()
    {
        Application.Quit();

        Debug.Log("Quitting Game...");
    }

    public void BackButton()
    {
        optionsGui.SetActive(false);
        mainGui.SetActive(true);
    }

}
