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
        if (Input.GetButtonDown("Xbox1Drop"))
        {
            SoundManager.Instance.PlayMenuClick();
            Slides[_currentSlide].gameObject.SetActive(false);
            _currentSlide++;

            if (_currentSlide >= _numberOfSlides)
            {
                Debug.Log("All slides played");
                gameObject.SetActive(false);
            }
            else
            {

                Slides[_currentSlide].gameObject.SetActive(true);
            }
        }
    }
}
