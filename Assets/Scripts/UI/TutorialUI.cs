using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    public Image[] Slides;
    private int _numberOfSlides;
    private int _currentSlide;
	private string _currentSlideName;

    private void OnEnable()
    {
        _currentSlide = 0;
        _numberOfSlides = Slides.Length;

		TrackingManager.StartTimer(TrackingManager.TIMER_HELP);
		_currentSlideName = Slides[_currentSlide].gameObject.name;
		Slides[_currentSlide].gameObject.SetActive(true);

	}

    private void Update()
    {
		HandleButton();
		HandleAxis();
    }

	void HandleButton()
	{
		if (Input.GetButtonDown("Xbox1Drop"))
		{
			if (NextSlide())
			{
				TrackingManager.StartTimer(TrackingManager.TIMER_HELP);
				_currentSlideName = Slides[_currentSlide].gameObject.name;
				Slides[_currentSlide].gameObject.SetActive(true);
			}
			else
			{
				gameObject.SetActive(false);
			}
		}
	}

	bool _isSwitching;

	void HandleAxis()
	{
		float x = Input.GetAxis("Xbox1Horizontal");
		if (!_isSwitching && x > 0)
		{
			_isSwitching = true;
			if (NextSlide())
			{
				TrackingManager.StartTimer(TrackingManager.TIMER_HELP);
				_currentSlideName = Slides[_currentSlide].gameObject.name;
				Slides[_currentSlide].gameObject.SetActive(true);
			}
			else
			{
				TrackingManager.Help(_currentSlideName, TrackingManager.StopTimer(TrackingManager.TIMER_HELP));
				gameObject.SetActive(false);
			}
		}
		else if (!_isSwitching && x < 0)
		{
			_isSwitching = true;
			if (PreviousSlide())
			{
				TrackingManager.StartTimer(TrackingManager.TIMER_HELP);
				_currentSlideName = Slides[_currentSlide].gameObject.name;
				Slides[_currentSlide].gameObject.SetActive(true);
			} else
			{
				TrackingManager.Help(_currentSlideName, TrackingManager.StopTimer(TrackingManager.TIMER_HELP));
				gameObject.SetActive(false);
			}
		}
		else if (_isSwitching && x == 0f)
		{
			_isSwitching = false;
		}
	}

	bool PreviousSlide()
	{
		SoundManager.Instance.PlayMenuClick();

		Slides[_currentSlide].gameObject.SetActive(false);
		TrackingManager.Help(_currentSlideName, TrackingManager.StopTimer(TrackingManager.TIMER_HELP));

		_currentSlide--;

		return _currentSlide >= 0;
	}

	bool NextSlide()
	{
		SoundManager.Instance.PlayMenuClick();

		Slides[_currentSlide].gameObject.SetActive(false);
		TrackingManager.Help(_currentSlideName, TrackingManager.StopTimer(TrackingManager.TIMER_HELP));

		_currentSlide++;

		return _currentSlide < _numberOfSlides;
	}
}
