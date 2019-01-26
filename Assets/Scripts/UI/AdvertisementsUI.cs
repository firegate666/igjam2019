using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdvertisementsUI : MonoBehaviour
{
    public Image[] Slides;
    private int _numberOfSlides;
    private int _currentSlide;

    private void Awake()
    {
        _currentSlide = 0;
        _numberOfSlides = Slides.Length;
        Slides[_currentSlide].gameObject.SetActive(true);
        
    }

    private void OnEnable()
    {
        StartCoroutine(ScheduleNext());
    }

    IEnumerator ScheduleNext()
    {
        yield return new WaitForSeconds(2);
        Slides[GetCurrentSlideIndex()].gameObject.SetActive(false);
        Slides[GetNextSlideIndex()].gameObject.SetActive(true);

        _currentSlide++;
        if (_currentSlide >= _numberOfSlides)
        {
            _currentSlide = 0;
        }

        StartCoroutine(Stop());
    }

    IEnumerator Stop()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }

    int GetCurrentSlideIndex()
    {
        return _currentSlide;
    }
    
    int GetNextSlideIndex()
    {
        int nextSlide = _currentSlide + 1;
        if (nextSlide >= _numberOfSlides)
        {
            nextSlide = 0;
        }
        return nextSlide;
    }
}
