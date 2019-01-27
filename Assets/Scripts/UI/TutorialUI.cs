using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    public Image[] Slides;
    private int _numberOfSlides;
    private int _currentSlide;

    private void OnEnable()
    {
        _currentSlide = 0;
        _numberOfSlides = Slides.Length;
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
				Slides[_currentSlide].gameObject.SetActive(true);
			}
			else
			{
				gameObject.SetActive(false);
			}
		}
		else if (!_isSwitching && x < 0)
		{
			_isSwitching = true;
			if (PreviousSlide())
			{
				Slides[_currentSlide].gameObject.SetActive(true);
			} else
			{
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
		_currentSlide--;

		return _currentSlide >= 0;
	}

	bool NextSlide()
	{
		SoundManager.Instance.PlayMenuClick();
		Slides[_currentSlide].gameObject.SetActive(false);
		_currentSlide++;

		return _currentSlide < _numberOfSlides;
	}
}
