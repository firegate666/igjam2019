using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsUI : MonoBehaviour
{
	GameStateManager _gsm;
	SoundManager _sm;

	private void Awake()
	{
		_gsm = GameObject.FindObjectOfType<GameStateManager>();
		_sm = GameObject.FindObjectOfType<SoundManager>();
	}

	private void OnEnable()
	{
		TrackingManager.Credits();
	}

	private void Update()
    {
        if (Input.GetButtonDown("Xbox1Drop"))
        {
            _sm.PlayMenuClick();
			_gsm.ChangeState(new StartState());
        }
    }
}
