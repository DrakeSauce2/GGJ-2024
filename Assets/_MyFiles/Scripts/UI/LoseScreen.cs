using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScreen : MonoBehaviour
{
    public static LoseScreen Instance;

    [SerializeField] private GameObject loseScreen;
    [SerializeField] private GameObject mainGUI;

    [Header("Audio")]
    [SerializeField] AudioClip loseAudio;

    public bool gameOver { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(Instance);

        gameOver = false;
    }

    public void Lose()
    {
        gameOver = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        AudioManager.Instance.SetMusic(loseAudio);

        Time.timeScale = 0f;      
        mainGUI.SetActive(false);
        loseScreen.SetActive(true);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
