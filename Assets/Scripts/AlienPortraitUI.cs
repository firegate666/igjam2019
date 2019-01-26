using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlienPortraitUI : MonoBehaviour
{
    public Image portrait;
    public Image element_1;
    public Image element_2;
    
    public void AddAlien(AlienContainer alien)
    {
        portrait.sprite = alien.AlienImage.sprite;
        portrait.gameObject.SetActive(true);
        element_1.sprite = alien.UIElementImage.sprite;
        element_1.gameObject.SetActive(true);
    }
}
