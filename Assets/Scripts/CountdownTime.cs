using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountdownTime : MonoBehaviour
{
    public float totalTime = 80f; // total time for countdown in seconds
    private float timeLeft; // time left for countdown in seconds
    private float minutes;
    private float seconds;

    private TextMeshProUGUI countdownText; // text object to display countdown

    void Start()
    {
        timeLeft = totalTime;
        countdownText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;

        
        if (timeLeft > 0)
        {
            seconds = Mathf.FloorToInt(timeLeft % 60);
            minutes = Mathf.FloorToInt(timeLeft / 60);
            countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            seconds = Mathf.FloorToInt(-1 * timeLeft % 60);
            minutes = Mathf.Ceil(-1 * timeLeft / 60);
            countdownText.text = "-" + string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}
