using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class StartUI : MonoBehaviour
{
    public GameObject ButtonArea;
    public Button[] Buttons;
    private int _activeButton;
    private bool _isSwitching;

	public TextMeshProUGUI highScore;

	GameStateManager _gsm;
	SoundManager _sm;

	private void Awake()
	{
		_gsm = FindObjectOfType<GameStateManager>();
		_sm = FindObjectOfType<SoundManager>();
	}

	// Start is called before the first frame update
	void Start()
    {
        if (Buttons.Length > 0)
        {
            Buttons[_activeButton].ToggleState();
        }

		_sm.PlayDefaultAudioLoop();

		ScoreManager TheScore = new ScoreManager();
		TheScore.LoadPlayerProgress();
		int highscore = TheScore.getHighestScore() * 100;
		highScore.text = highscore.ToString();
	}

	public void StartGame(int numberOfPlayers)
    {
        _sm.PlayMenuClick();
		_gsm.ChangeState(new InGameState());
    }

    public void ShowCredits()
    {
        _sm.PlayMenuClick();
		_gsm.ChangeState(new CreditsState());
    }
    
    public void ShowOptions()
    {
        _sm.PlayMenuClick();
        //Debug.Log("Show options");
    }
    
    public void ShowHelp()
    {
        _sm.PlayMenuClick();
		_gsm.ChangeState(new HelpState());
	}
    
    void ButtonDown()
    {
        _sm.PlayMenuClick();
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
        _sm.PlayMenuClick();
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
