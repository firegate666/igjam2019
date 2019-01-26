﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StartUI : MonoBehaviour
{
    public Button[] Buttons;
    private int _activeButton;
    private bool _isSwitching;

    // Start is called before the first frame update
    void Start()
    {
        if (Buttons.Length > 0)
        {
            Buttons[_activeButton].ToggleState();
        }
    }

    public void StartGame(int numberOfPlayers)
    {
        Debug.Log("Start Game");
        GameManager.Instance.StartGame(numberOfPlayers);
    }

    public void ShowCredits()
    {
        Debug.Log("Show credits");
    }
    
    public void ShowHelp()
    {
        Debug.Log("Show help");
    }
    
    void ButtonDown()
    {
        Buttons[_activeButton].ToggleState();
        
        _activeButton++;
        if (_activeButton >= Buttons.Length)
        {
            _activeButton = 0;
        }
        
        Buttons[_activeButton].ToggleState();
    }
    
    void ButtonUp()
    {
        Buttons[_activeButton].ToggleState();
        
        _activeButton--;
        if (_activeButton < 0)
        {
            _activeButton = Buttons.Length - 1;
        }
        
        Buttons[_activeButton].ToggleState();
    }

    // Update is called once per frame
    void Update()
    {
        float y = Input.GetAxis("Xbox1Vertical");
        if (!_isSwitching && y > 0)
        {
            _isSwitching = true;
            ButtonDown();
        } else if (!_isSwitching && y < 0)
        {
            _isSwitching = true;
            ButtonUp();
        } else if (_isSwitching && y == 0f)
        {
            _isSwitching = false;
        }

        if (Input.GetButtonDown("Xbox1Drop"))
        {
            Buttons[_activeButton].TriggerAction();
        }
    }
}