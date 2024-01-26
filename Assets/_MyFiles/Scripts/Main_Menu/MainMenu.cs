using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainGui;
    [SerializeField] private GameObject optionsGui;

    bool isLoading = false;

    private void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        StartCoroutine(Transition.Instance.CutIn());
    }

    public void PlayButton()
    {
        if (isLoading) return;

        StartCoroutine(StartLevelOne());
    }

    private IEnumerator StartLevelOne()
    {
        isLoading = true;

        StartCoroutine(Transition.Instance.CutOut());

        yield return new WaitForSeconds(1.5f);

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
