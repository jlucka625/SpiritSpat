using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelTimer : MonoBehaviour
{
    public int durationInSeconds;

    private float timeRemaining;
    private bool isPaused = false;
    private Text timeText;

    private const int SECONDS_IN_MINUTE = 60;

    void Start()
    {
        timeRemaining = (float)durationInSeconds;
        timeText = GetComponent<Text>();
    }

    void Update()
    {
        if (!isPaused)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0)
            {
                // Time exceeded, stop timer
                timeRemaining = 0.0f;
                isPaused = true;
                GameOver.endGame();
            }

            FormatTime();
         }
    }

    private void FormatTime()
    {
        int numMinutes = (int)timeRemaining / SECONDS_IN_MINUTE;
        int numSeconds = (int)timeRemaining % SECONDS_IN_MINUTE;
        string secondsString = numSeconds.ToString();
        if (numSeconds < 10)
        {
            // Pad the digits 0 - 9 with a leading zero
            secondsString = "0" + numSeconds.ToString();
        }
        timeText.text = numMinutes.ToString() + ":" + secondsString;
    }

    public float GetTimeRemaining()
    {
        return timeRemaining;
    }

    public bool IsTimeUp()
    {
        return (timeRemaining <= 0);
    }

    public void Pause()
    {
        isPaused = true;
    }

    public void Resume()
    {
        isPaused = false;
    }

    public void Reset()
    {
        isPaused = true;
        timeRemaining = durationInSeconds;
    }
}
