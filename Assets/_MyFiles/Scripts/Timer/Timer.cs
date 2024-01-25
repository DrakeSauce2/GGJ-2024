using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    bool pauseTimer = false;

    [Header("Gameplay")]
    [SerializeField] float goalTime = 180f; // In Seconds

    public float minutes { get; private set; }
    public float seconds { get; private set; }

    private void Update()
    {
        ProcessTimer();

        if(goalTime <= 0)
        {
            pauseTimer = true;

            AudienceEnergy.Instance.StopSpawningCycle();
            GameManager.Instance.StopEnemySpawnCycle();

            GameManager.Instance.KillAllInstancedSpawns();

            GameManager.Instance.SpawnEndDoor();
            GameManager.Instance.OpenCurtain();

            gameObject.SetActive(false);
        }

    }

    private void ProcessTimer()
    {
        if (pauseTimer == true) return;

        goalTime -= Time.deltaTime;

        minutes = Mathf.FloorToInt(goalTime / 60);
        seconds = Mathf.FloorToInt(goalTime % 60);

        timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }

}
