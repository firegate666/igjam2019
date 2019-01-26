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
        
        
    }

    private void OnEnable()
    {
        Slides[_currentSlide].gameObject.SetActive(true);
        StartCoroutine(ScheduleNext());
    }

    IEnumerator ScheduleNext()
    {
        yield return new WaitForSeconds(2);
        Slides[GetCurrentSlideIndex()].gameObject.SetActive(false);

        IncrementSlide();
        
        Slides[GetCurrentSlideIndex()].gameObject.SetActive(true);

        StartCoroutine(Stop());
    }

    void IncrementSlide()
    {
        _currentSlide++;
        if (_currentSlide >= _numberOfSlides)
        {
            _currentSlide = 0;
        }
    }

    IEnumerator Stop()
    {
        yield return new WaitForSeconds(2);
        
        Slides[GetCurrentSlideIndex()].gameObject.SetActive(false);
        IncrementSlide();
        
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
