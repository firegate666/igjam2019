using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StartUI : MonoBehaviour
{
    public GameObject ButtonArea;
    public Button[] Buttons;
    private int _activeButton;
    private bool _isSwitching;

    public GameObject TutorialOverlay;
    public GameObject CreditsOverlay;

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
        SoundManager.Instance.PlayMenuClick();
        //Debug.Log("Start Game");
        GameManager.Instance.StartGame(numberOfPlayers);
    }

    public void ShowCredits()
    {
        SoundManager.Instance.PlayMenuClick();
        ButtonArea.SetActive((false));
        CreditsOverlay.SetActive(true);
    }
    
    public void ShowOptions()
    {
        SoundManager.Instance.PlayMenuClick();
        //Debug.Log("Show options");
    }
    
    public void ShowHelp()
    {
        SoundManager.Instance.PlayMenuClick();
        ButtonArea.SetActive((false));
        TutorialOverlay.SetActive(true);
    }
    
    void ButtonDown()
    {
        SoundManager.Instance.PlayMenuClick();
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
        SoundManager.Instance.PlayMenuClick();
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
        if (TutorialOverlay.activeSelf || CreditsOverlay.activeSelf)
        {
            // start menu blocked if overlay
        }
        else
        {
            if (!ButtonArea.activeSelf && !CreditsOverlay.activeSelf)
            {
                ButtonArea.SetActive((true));
            }

            float y = Input.GetAxis("Xbox1Vertical") + Input.GetAxis("Keyboard1Vertical");
            if (!_isSwitching && y > 0)
            {
                _isSwitching = true;
                ButtonDown();
            }
            else if (!_isSwitching && y < 0)
            {
                _isSwitching = true;
                ButtonUp();
            }
            else if (_isSwitching && y == 0f)
            {
                _isSwitching = false;
            }

            if (Input.GetButtonDown("Xbox1Drop"))
            {
                Buttons[_activeButton].TriggerAction();
            }
        }
    }
}
