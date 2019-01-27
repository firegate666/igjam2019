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

    private AudioSource _audioSource;

    private float _currentPitch;
    private float _targetPitch;
    private float t;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        _currentPitch = 1.0f;
        _targetPitch = 1.0f;
        t = 0;
    }

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

        Mathf.Lerp(_currentPitch, _targetPitch, (t += (Time.deltaTime * 0.25f)));
    }

    private void ChangePitch(float newPitch)
    {
        t = 0;
        _currentPitch = _audioSource.pitch;
        _targetPitch = newPitch;
    }
    
    public void Pause()
    {
        _isRunning = false;
        _audioSource.pitch = 0.8f;
    }

    public void Unpause()
    {
        _isRunning = true;
        _audioSource.pitch = 1;
    }

    public void SetRunning(float timeInSeconds, Action callback)
    {
        TimeInSeconds = timeInSeconds;
        _isRunning = true;
        _callback = callback;
        _audioSource.Play();
    }
}
