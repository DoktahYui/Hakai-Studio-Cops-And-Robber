using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Required for UI

public class CountdownTimer : MonoBehaviour
{
    public float countdownTime = 30f; // Countdown time in seconds
    public float bufferTime = 10f; // Buffer time in seconds
    public Text timerText; // Reference to the Text component

    private float timer;
    private bool isBuffering;

    void Start()
    {
        timer = countdownTime;
        isBuffering = false;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            if (!isBuffering)
            {
                // Switch to buffer time
                timer = bufferTime;
                isBuffering = true;
            }
            else
            {
                // Switch back to countdown time
                timer = countdownTime;
                isBuffering = false;
            }
        }

        // Update the Text component
        timerText.text = "Time: " + timer.ToString("F2") + "\nBuffering: " + isBuffering;
    }
}

