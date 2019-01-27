using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class AdvertisementsUI : MonoBehaviour
{
    public Image[] Slides;
    private int _numberOfSlides;
    private int _currentSlide;

    private void Awake()
    {
        _currentSlide = Random.Range(0, Slides.Length);
        _numberOfSlides = Slides.Length;
    }

    private void OnEnable()
    {
        Slides[_currentSlide].gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        GameManager.Instance.LeaveAdvertisements();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Xbox1Drop") && GameManager.Instance.gameState == GameState.Advertisements)
        {
            SoundManager.Instance.PlayMenuClick();
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
