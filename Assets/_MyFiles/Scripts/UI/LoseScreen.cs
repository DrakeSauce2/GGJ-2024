using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseScreen : MonoBehaviour
{
    public static LoseScreen Instance;

    [SerializeField] private GameObject loseScreen;
    [SerializeField] private GameObject mainGUI;

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

        mainGUI.SetActive(false);
        loseScreen.SetActive(true);
    }

}
