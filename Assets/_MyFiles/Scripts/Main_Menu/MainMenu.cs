using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainGui;
    [SerializeField] private GameObject optionsGui;

    [Header("Main Menu Theme Songs")]
    [SerializeField] List<AudioClip> mainMenuThemes = new List<AudioClip>();

    private void Awake()
    {
        int randNum = Random.Range(0, mainMenuThemes.Count);
        AudioManager.Instance.SetMusic(mainMenuThemes[randNum]);
    }

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
