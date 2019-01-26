using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlienUIElement : MonoBehaviour
{
    public Image AlienImage;
    public Image ElementIcon;

    public void SetAlien(AlienContainer alien)
    {
        AlienImage.sprite = alien.AlienImage.sprite;
        ElementIcon.sprite = alien.ElementImage.sprite;
    }
}
