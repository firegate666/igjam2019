using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class AdvertisementsUI : MonoBehaviour
{
    public Image[] Slides;
    private int _numberOfSlides;
    private int _currentSlide;
    private Remote _remote;

	private float remoteXDistance;
	private float remoteYDistance;

    private void Awake()
    {
        _currentSlide = Random.Range(0, Slides.Length);
        _numberOfSlides = Slides.Length;
        _remote = GetComponentInChildren<Remote>();
        _remote.GetComponent<RectTransform>().SetParent(GetComponent<RectTransform>().parent);

		remoteXDistance = 0;
		remoteYDistance = 0;
    }

    private void OnEnable()
    {
		TrackingManager.StartTimer(TrackingManager.TIMER_AD);
		Slides[_currentSlide].gameObject.SetActive(true);
        _remote.gameObject.SetActive(true);
        
    }

    private void OnDisable()
    {
		GameManager.Instance.LeaveAdvertisements();
        _remote.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Xbox1Drop") && GameManager.Instance.gameState == GameState.Advertisements)
        {
            SoundManager.Instance.PlayMenuClick();
			TrackingManager.Ad(Slides[_currentSlide].gameObject.name, TrackingManager.StopTimer(TrackingManager.TIMER_AD), _remote.GetDistanceMoved());
			Stop();
        }
    }

    void IncrementSlide()
    {
        _currentSlide++;
        if (_currentSlide >= _numberOfSlides)
        {
            _currentSlide = 0;
        }
    }

    private void Stop()
    {
        Slides[GetCurrentSlideIndex()].gameObject.SetActive(false);
        IncrementSlide();
        gameObject.SetActive(false);
    }

    int GetCurrentSlideIndex()
    {
        return _currentSlide;
    }
}
