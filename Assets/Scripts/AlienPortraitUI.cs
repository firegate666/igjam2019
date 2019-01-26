using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlienPortraitUI : MonoBehaviour
{
    private Image portrait;
    private Image element_1;
    private Image element_2;
    
    public void AddAlien(AlienContainer alien)
    {
        portrait.sprite = alien.AlienImage.sprite;
        element_1.sprite = alien.ElementImage.sprite;
    }
}
