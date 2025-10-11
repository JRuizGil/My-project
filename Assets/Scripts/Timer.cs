using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TMP_Text text;
    public float time = 180f; // Tiempo total en segundos (3 minutos)
    private float currentTime;
    private bool isRunning = false;

    void Start()
    {
        currentTime = time;
        UpdateTimerDisplay();
    }

    void Update()
    {
        if (isRunning)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerDisplay();

            if (currentTime <= 0f)
            {
                currentTime = 0f;
                isRunning = false;
                TimerEnded();
            }
        }
    }

    public void StartTimer()
    {
        if (!isRunning)
            isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void RestartTimer()
    {
        currentTime = time;
        isRunning = true;
        UpdateTimerDisplay();
    }

    public void TimerEnded()
    {
        Debug.Log(" El tiempo se ha terminado.");
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        text.text = $"{minutes:00}:{seconds:00}";
    }
}