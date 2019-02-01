using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    public Image[] Slides;
    private int _numberOfSlides;
    private int _currentSlide;
	private string _currentSlideName;

	GameStateManager _gsm;
	SoundManager _sm;

	private void Awake()
	{
		_gsm = FindObjectOfType<GameStateManager>();
		_sm = FindObjectOfType<SoundManager>();
	}

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

				_sm.PlayMenuClick();
				Slides[_currentSlide].gameObject.SetActive(true);

			}
			else
			{
				_gsm.ChangeState(new StartState());
				gameObject.SetActive(false);
			}
		}
	}

	bool _isSwitching;

	void HandleAxis()
	{
		float x = Input.GetAxis("Xbox1Horizontal") + Input.GetAxis("Keyboard1Horizontal");
		if (!_isSwitching && x > 0)
		{
			_isSwitching = true;
			if (NextSlide())
			{
				TrackingManager.StartTimer(TrackingManager.TIMER_HELP);
				_currentSlideName = Slides[_currentSlide].gameObject.name;

				_sm.PlayMenuClick();
				Slides[_currentSlide].gameObject.SetActive(true);
			}
			else
			{
				TrackingManager.Help(_currentSlideName, TrackingManager.StopTimer(TrackingManager.TIMER_HELP));
				_gsm.ChangeState(new StartState());
			}
		}
		else if (!_isSwitching && x < 0)
		{
			_isSwitching = true;
			if (PreviousSlide())
			{
				TrackingManager.StartTimer(TrackingManager.TIMER_HELP);
				_currentSlideName = Slides[_currentSlide].gameObject.name;

				_sm.PlayMenuClick();
				Slides[_currentSlide].gameObject.SetActive(true);
			} else
			{
				TrackingManager.Help(_currentSlideName, TrackingManager.StopTimer(TrackingManager.TIMER_HELP));
				_gsm.ChangeState(new StartState());
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
