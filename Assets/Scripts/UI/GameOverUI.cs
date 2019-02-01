using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
	SoundManager _sm;

	private void Awake()
	{
		_sm = FindObjectOfType<SoundManager>();
	}

	private void Start()
	{
		_sm.PlayDefaultAudioLoop();
	}
	// Update is called once per frame
	void Update()
    {
        if (Input.GetButtonDown("Xbox1Drop"))
        {
            SoundManager.Instance.PlayMenuClick();
            GameManager.Instance.Restart();
        }
    }
}
