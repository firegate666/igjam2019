using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITimer : MonoBehaviour
{
    public string NormalColor;
    public string WarningColor;
    public string DangerColor;

    public float WarningLimit = 59;
    public float DangerLimit = 20;
    
    public float TimeInSeconds;

    public  bool _isRunning;
    private Action _callback;

    public TextMeshProUGUI TimerDisplay;
    
    // Update is called once per frame
    void Update()
    {
        if (_isRunning)
        {
            TimeInSeconds -= Time.deltaTime;
            
            TimeSpan time = TimeSpan.FromSeconds(TimeInSeconds);
            string str = time .ToString(@"mm\:ss");
            TimerDisplay.text = str;
        }

        if (TimeInSeconds < DangerLimit)
        {
            Color color;
            ColorUtility.TryParseHtmlString(DangerColor, out color);
            TimerDisplay.color = color;
        } else if (TimeInSeconds < WarningLimit)
        {
            Color color;
            ColorUtility.TryParseHtmlString(WarningColor, out color);
            TimerDisplay.color = color;
        }

        if (TimeInSeconds < 0)
        {
            _isRunning = false;
            _callback.Invoke();
        }
    }

    public void SetRunning(float timeInSeconds, Action callback)
    {
        TimeInSeconds = timeInSeconds;
        _isRunning = true;
        _callback = callback;
    }
}
