using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public enum TimerState
{
    NORMAL,
    WARNING,
    DANGER
}

public class UITimer : MonoBehaviour
{
    public string NormalColor;
    public string WarningColor;
    public string DangerColor;

    public float WarningLimit = 59;
    public float WarningPitch = 1.15f;
    public float DangerLimit = 20;
    public float DangerPitch = 1.3f;

    public ScrollingBackground Background;
    
    public float TimeInSeconds;

    public  bool _isRunning;
    private Action _callback;

    public TextMeshProUGUI TimerDisplay;

    private AudioSource _audioSource;

    private float _currentPitch;
    private float _targetPitch;
    private float t;

    private float _pitchModSpeedUp = 0.25f;
    private float _basePitch = 1f;

    private TimerState _state;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        _currentPitch = 1.0f;
        _targetPitch = 1.0f;
        t = 0;
        
        Background.Speed = new Vector2(0.01f, 0);
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

        if (TimeInSeconds < DangerLimit && _state == TimerState.WARNING)
        {
            _state = TimerState.DANGER;
            Color color;
            ColorUtility.TryParseHtmlString(DangerColor, out color);
            TimerDisplay.color = color;
            ChangePitch(DangerPitch, true);
            Background.Speed = new Vector2(0.05f, 0);
        } 
        else if (TimeInSeconds < WarningLimit && _state == TimerState.NORMAL)
        {
            _state = TimerState.WARNING;
            Color color;
            ColorUtility.TryParseHtmlString(WarningColor, out color);
            TimerDisplay.color = color;
            ChangePitch(WarningPitch, true);
            Background.Speed = new Vector2(0.03f, 0);
        }

        if (TimeInSeconds < 0)
        {
            _isRunning = false;
            _callback.Invoke();
        }
        
        _audioSource.pitch = Mathf.Lerp(_currentPitch, _targetPitch, (t += (Time.deltaTime * _pitchModSpeedUp)));
    }

    private void ChangePitch(float newPitch, bool adjustBasePitch)
    {
        if (adjustBasePitch)
        {
            _basePitch = newPitch;
        }
        t = 0;
        _currentPitch = _audioSource.pitch;
        _targetPitch = newPitch;
    }
    
    public void Pause()
    {
        _isRunning = false;
        _pitchModSpeedUp = 0.5f;
        ChangePitch(0.0f, false);
    }

    public void Unpause()
    {
        _isRunning = true;
        _pitchModSpeedUp = 0.75f;
        ChangePitch(1.0f, false);
    }

    public void SetRunning(float timeInSeconds, Action callback)
    {
        TimeInSeconds = timeInSeconds;
        _isRunning = true;
        _callback = callback;
        _audioSource.Play();
        _state = TimerState.NORMAL;
    }
}
