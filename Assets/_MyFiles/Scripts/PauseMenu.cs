using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;

    [SerializeField] GameObject mainGUI;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject optionsMenu;

    bool pauseCooldown = false;

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(Instance);
    }

    public void Pause()
    {
        if (pauseCooldown || LoseScreen.Instance.gameOver == true) return;

        StartCoroutine(PauseCoroutine());
    }


    public IEnumerator PauseCoroutine()
    {
        pauseCooldown = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
        mainGUI.SetActive(false);

        yield return new WaitForSeconds(0.1f);

        pauseCooldown = false;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        mainGUI.SetActive(true);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void MainMenu()
    {
        Destroy(Player.Instance.gameObject);
        Destroy(gameObject);

        SceneManager.LoadScene("MainMenu");
    }

    public void Options()
    {
        pauseMenu.SetActive(false);
        mainGUI.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void Back()
    {
        pauseMenu.SetActive(true);
        mainGUI.SetActive(false);
        optionsMenu.SetActive(false);
    }

}
