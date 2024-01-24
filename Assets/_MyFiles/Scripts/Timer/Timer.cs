using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float elapsedTime = 0;
    bool pauseTimer = false;

    [Header("Gameplay")]
    [SerializeField] bool useGoal = false;
    [SerializeField] float goalTime = 180f; // In Seconds

    public float minutes { get; private set; }
    public float seconds { get; private set; }

    private void Update()
    {
        ProcessTimer();

        if (useGoal == false) return;

        if(elapsedTime >= goalTime)
        {
            pauseTimer = true;

            AudienceEnergy.Instance.StopSpawningCycle();
            GameManager.Instance.StopEnemySpawnCycle();

            GameManager.Instance.KillAllInstancedSpawns();

            // Do outro stuff to boss battle
        }

    }

    private void ProcessTimer()
    {
        if (pauseTimer == true) return;

        elapsedTime += Time.deltaTime;
        minutes = Mathf.FloorToInt(elapsedTime / 60);
        seconds = Mathf.FloorToInt(elapsedTime % 60);

        timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }

}
